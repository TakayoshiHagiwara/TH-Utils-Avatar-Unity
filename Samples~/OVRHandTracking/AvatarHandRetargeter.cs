// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Retargets finger rotations from Meta Quest / OVR hand tracking to a Humanoid avatar's hands.
// --------------------------------------------------

using System;
using UnityEngine;

namespace TH.Utils.Avatar
{
    /// <summary>
    /// Retargets finger rotations from Meta Quest / OVR hand tracking to a Humanoid avatar's hands.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For each finger joint, the script stores the initial source rotation and the initial target
    /// rotation. At runtime, it calculates the source rotation delta, applies a manual axis
    /// correction, and then applies the result to the corresponding avatar bone.
    /// </para>
    /// <para>
    /// When using the XR Hands backend, <see cref="OVRSkeleton.BoneId"/> may not match the actual
    /// order or meaning of the underlying Transform list. For that reason, this implementation
    /// searches source bones by Transform name, such as <c>XRHand_IndexProximal</c>,
    /// <c>XRHand_IndexIntermediate</c>, and <c>XRHand_IndexDistal</c>.
    /// </para>
    /// <para>
    /// Assign <c>leftOVRSkeleton</c>, <c>rightOVRSkeleton</c>, and <c>avatarAnimator</c> from the
    /// Inspector. If only one hand is needed, the unused OVRSkeleton field can be left empty.
    /// </para>
    /// </remarks>
    public sealed class AvatarHandRetargeter : MonoBehaviour
    {
        /// <summary>
        /// Stores rotation-axis correction values for the three joints of a single finger.
        /// </summary>
        /// <remarks>
        /// Values are interpreted as Euler angles. They can be edited in the Inspector during Play
        /// Mode, and changes are applied immediately on the next update.
        /// </remarks>
        [Serializable]
        public sealed class JointOffsets
        {
            public Vector3 Proximal;
            public Vector3 Intermediate;
            public Vector3 Distal;
        }

        /// <summary>
        /// Stores rotation-axis correction values for all five fingers of one hand.
        /// </summary>
        [Serializable]
        public sealed class HandOffsets
        {
            public JointOffsets Thumb = new JointOffsets();
            public JointOffsets Index = new JointOffsets();
            public JointOffsets Middle = new JointOffsets();
            public JointOffsets Ring = new JointOffsets();
            public JointOffsets Little = new JointOffsets();
        }

        /// <summary>
        /// Holds runtime data for one finger.
        /// </summary>
        /// <remarks>
        /// The source Transforms come from OVR / XR Hands. The target Transforms come from the
        /// Humanoid avatar. Initial rotations are stored so that runtime rotation deltas can be
        /// applied relative to the startup pose.
        /// </remarks>
        private sealed class FingerRuntime
        {
            public Transform SourceProximal;
            public Transform SourceIntermediate;
            public Transform SourceDistal;
            public Transform TargetProximal;
            public Transform TargetIntermediate;
            public Transform TargetDistal;
            public Quaternion SourceInitialProximal;
            public Quaternion SourceInitialIntermediate;
            public Quaternion SourceInitialDistal;
            public Quaternion TargetInitialProximal;
            public Quaternion TargetInitialIntermediate;
            public Quaternion TargetInitialDistal;
            public JointOffsets Offsets;
        }

        /// <summary>
        /// Holds runtime data for one hand.
        /// </summary>
        private sealed class HandRuntime
        {
            public OVRSkeleton Skeleton;
            public bool Initialized;
            public readonly FingerRuntime Thumb = new FingerRuntime();
            public readonly FingerRuntime Index = new FingerRuntime();
            public readonly FingerRuntime Middle = new FingerRuntime();
            public readonly FingerRuntime Ring = new FingerRuntime();
            public readonly FingerRuntime Little = new FingerRuntime();
        }

        [Header("References")]
        [SerializeField] private OVRSkeleton _leftOVRSkeleton;
        [SerializeField] private OVRSkeleton _rightOVRSkeleton;
        [SerializeField] private Animator _avatarAnimator;

        [Header("Left Hand Offsets")]
        [SerializeField] private HandOffsets _leftHandOffsets = new HandOffsets();

        [Header("Right Hand Offsets")]
        [SerializeField] private HandOffsets _rightHandOffsets = new HandOffsets();

        [Header("Options")]
        [SerializeField] private bool _applyInLateUpdate = true;
        [SerializeField] private bool _logInitialization = true;

        private readonly HandRuntime _leftHand = new HandRuntime();
        private readonly HandRuntime _rightHand = new HandRuntime();

        private void Start()
        {
            if (_avatarAnimator == null || _avatarAnimator.avatar == null ||
                !_avatarAnimator.avatar.isValid || !_avatarAnimator.avatar.isHuman)
            {
                Debug.LogError("AvatarHandRetargeter: Please assign a valid Humanoid Animator.", this);
                enabled = false;
                return;
            }

            _leftHand.Skeleton = _leftOVRSkeleton;
            _rightHand.Skeleton = _rightOVRSkeleton;

            SetupTargetBones();
            AssignOffsets();
        }

        private void Update()
        {
            if (!_applyInLateUpdate) UpdateHands();
        }

        private void LateUpdate()
        {
            if (_applyInLateUpdate) UpdateHands();
        }

        private void UpdateHands()
        {
            UpdateHand(_leftHand, "Left Hand");
            UpdateHand(_rightHand, "Right Hand");
        }

        /// <summary>
        /// Updates retargeting for a single hand.
        /// </summary>
        /// <param name="hand">Runtime data for the hand to update.</param>
        /// <param name="label">Display label used for debug logs.</param>
        private void UpdateHand(HandRuntime hand, string label)
        {
            if (hand.Skeleton == null || !hand.Skeleton.IsInitialized) return;
            if (!hand.Initialized && !TryInitializeHand(hand, label)) return;

            ApplyFinger(hand.Thumb);
            ApplyFinger(hand.Index);
            ApplyFinger(hand.Middle);
            ApplyFinger(hand.Ring);
            ApplyFinger(hand.Little);
        }

        /// <summary>
        /// Finds source bones and captures initial rotations for one hand.
        /// </summary>
        /// <param name="hand">Runtime data for the hand to initialize.</param>
        /// <param name="label">Display label used for debug logs.</param>
        /// <returns>True if initialization succeeds; otherwise, false.</returns>
        private bool TryInitializeHand(HandRuntime hand, string label)
        {
            FindSourceBones(hand);

            if (!IsFingerReady(hand.Thumb) || !IsFingerReady(hand.Index) ||
                !IsFingerReady(hand.Middle) || !IsFingerReady(hand.Ring) ||
                !IsFingerReady(hand.Little)) return false;

            CaptureInitialRotations(hand.Thumb);
            CaptureInitialRotations(hand.Index);
            CaptureInitialRotations(hand.Middle);
            CaptureInitialRotations(hand.Ring);
            CaptureInitialRotations(hand.Little);

            hand.Initialized = true;
            if (_logInitialization)
                Debug.Log($"AvatarHandRetargeter: {label} initialized.", this);
            return true;
        }

        /// <summary>
        /// Gets all target finger bones from the Humanoid avatar.
        /// </summary>
        private void SetupTargetBones()
        {
            SetupTargetFinger(_leftHand.Thumb, HumanBodyBones.LeftThumbProximal, HumanBodyBones.LeftThumbIntermediate, HumanBodyBones.LeftThumbDistal);
            SetupTargetFinger(_leftHand.Index, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexDistal);
            SetupTargetFinger(_leftHand.Middle, HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftMiddleIntermediate, HumanBodyBones.LeftMiddleDistal);
            SetupTargetFinger(_leftHand.Ring, HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftRingDistal);
            SetupTargetFinger(_leftHand.Little, HumanBodyBones.LeftLittleProximal, HumanBodyBones.LeftLittleIntermediate, HumanBodyBones.LeftLittleDistal);

            SetupTargetFinger(_rightHand.Thumb, HumanBodyBones.RightThumbProximal, HumanBodyBones.RightThumbIntermediate, HumanBodyBones.RightThumbDistal);
            SetupTargetFinger(_rightHand.Index, HumanBodyBones.RightIndexProximal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexDistal);
            SetupTargetFinger(_rightHand.Middle, HumanBodyBones.RightMiddleProximal, HumanBodyBones.RightMiddleIntermediate, HumanBodyBones.RightMiddleDistal);
            SetupTargetFinger(_rightHand.Ring, HumanBodyBones.RightRingProximal, HumanBodyBones.RightRingIntermediate, HumanBodyBones.RightRingDistal);
            SetupTargetFinger(_rightHand.Little, HumanBodyBones.RightLittleProximal, HumanBodyBones.RightLittleIntermediate, HumanBodyBones.RightLittleDistal);
        }

        /// <summary>
        /// Gets the three target avatar bones for a single finger.
        /// </summary>
        /// <param name="finger">Runtime data for the finger to populate.</param>
        /// <param name="proximal">Humanoid bone for the first joint.</param>
        /// <param name="intermediate">Humanoid bone for the second joint.</param>
        /// <param name="distal">Humanoid bone for the third joint.</param>
        private void SetupTargetFinger(FingerRuntime finger, HumanBodyBones proximal, HumanBodyBones intermediate, HumanBodyBones distal)
        {
            finger.TargetProximal = _avatarAnimator.GetBoneTransform(proximal);
            finger.TargetIntermediate = _avatarAnimator.GetBoneTransform(intermediate);
            finger.TargetDistal = _avatarAnimator.GetBoneTransform(distal);

            if (finger.TargetProximal == null || finger.TargetIntermediate == null || finger.TargetDistal == null)
            {
                Debug.LogError($"AvatarHandRetargeter: 指ボーンを取得できません: {proximal}, {intermediate}, {distal}", this);
                enabled = false;
            }
        }

        /// <summary>
        /// Assigns Inspector-configured offset values to runtime finger data.
        /// </summary>
        private void AssignOffsets()
        {
            _leftHand.Thumb.Offsets = _leftHandOffsets.Thumb;
            _leftHand.Index.Offsets = _leftHandOffsets.Index;
            _leftHand.Middle.Offsets = _leftHandOffsets.Middle;
            _leftHand.Ring.Offsets = _leftHandOffsets.Ring;
            _leftHand.Little.Offsets = _leftHandOffsets.Little;

            _rightHand.Thumb.Offsets = _rightHandOffsets.Thumb;
            _rightHand.Index.Offsets = _rightHandOffsets.Index;
            _rightHand.Middle.Offsets = _rightHandOffsets.Middle;
            _rightHand.Ring.Offsets = _rightHandOffsets.Ring;
            _rightHand.Little.Offsets = _rightHandOffsets.Little;
        }

        /// <summary>
        /// Checks whether all required source and target Transforms for a finger are available.
        /// </summary>
        /// <param name="finger">Runtime data for the finger to check.</param>
        /// <returns>True if all required Transforms are available; otherwise, false.</returns>
        private static bool IsFingerReady(FingerRuntime finger)
        {
            return finger.SourceProximal != null && finger.SourceIntermediate != null && finger.SourceDistal != null &&
                   finger.TargetProximal != null && finger.TargetIntermediate != null && finger.TargetDistal != null;
        }

        /// <summary>
        /// Captures the initial local rotations for one finger.
        /// </summary>
        /// <param name="finger">Runtime data for the finger to capture.</param>
        private static void CaptureInitialRotations(FingerRuntime finger)
        {
            finger.SourceInitialProximal = finger.SourceProximal.localRotation;
            finger.SourceInitialIntermediate = finger.SourceIntermediate.localRotation;
            finger.SourceInitialDistal = finger.SourceDistal.localRotation;
            finger.TargetInitialProximal = finger.TargetProximal.localRotation;
            finger.TargetInitialIntermediate = finger.TargetIntermediate.localRotation;
            finger.TargetInitialDistal = finger.TargetDistal.localRotation;
        }

        /// <summary>
        /// Applies corrected rotations to the three target joints of one finger.
        /// </summary>
        /// <param name="finger">Runtime data for the finger to apply.</param>
        private static void ApplyFinger(FingerRuntime finger)
        {
            ApplyRotation(finger.SourceProximal, finger.TargetProximal, finger.SourceInitialProximal, finger.TargetInitialProximal, finger.Offsets.Proximal);
            ApplyRotation(finger.SourceIntermediate, finger.TargetIntermediate, finger.SourceInitialIntermediate, finger.TargetInitialIntermediate, finger.Offsets.Intermediate);
            ApplyRotation(finger.SourceDistal, finger.TargetDistal, finger.SourceInitialDistal, finger.TargetInitialDistal, finger.Offsets.Distal);
        }

        /// <summary>
        /// Applies a source rotation delta to a target bone with an axis correction.
        /// </summary>
        /// <param name="source">Source bone from OVR / XR Hands.</param>
        /// <param name="target">Target bone from the avatar.</param>
        /// <param name="sourceInitial">Initial local rotation of the source bone.</param>
        /// <param name="targetInitial">Initial local rotation of the target bone.</param>
        /// <param name="rotationOffset">Euler-angle axis correction applied to the source delta.</param>
        private static void ApplyRotation(Transform source, Transform target, Quaternion sourceInitial, Quaternion targetInitial, Vector3 rotationOffset)
        {
            Quaternion delta = Quaternion.Inverse(sourceInitial) * source.localRotation;
            Quaternion axisCorrection = Quaternion.Euler(rotationOffset);
            Quaternion correctedDelta = axisCorrection * delta * Quaternion.Inverse(axisCorrection);
            target.localRotation = targetInitial * correctedDelta;
        }

        /// <summary>
        /// Finds source finger bones by XR Hands Transform names.
        /// </summary>
        /// <remarks>
        /// This intentionally avoids relying on <see cref="OVRSkeleton.BoneId"/>, because when using
        /// the XR Hands backend, BoneId values may not correspond to the actual Transform list.
        /// </remarks>
        /// <param name="hand">Runtime data for the hand to populate.</param>
        private static void FindSourceBones(HandRuntime hand)
        {
            if (hand.Skeleton.Bones == null)
            {
                return;
            }

            foreach (OVRBone bone in hand.Skeleton.Bones)
            {
                if (bone == null || bone.Transform == null)
                {
                    continue;
                }

                string boneName = bone.Transform.name;

                AssignByName(hand.Thumb, bone.Transform, boneName,
                    "XRHand_ThumbMetacarpal",
                    "XRHand_ThumbProximal",
                    "XRHand_ThumbDistal");

                AssignByName(hand.Index, bone.Transform, boneName,
                    "XRHand_IndexProximal",
                    "XRHand_IndexIntermediate",
                    "XRHand_IndexDistal");

                AssignByName(hand.Middle, bone.Transform, boneName,
                    "XRHand_MiddleProximal",
                    "XRHand_MiddleIntermediate",
                    "XRHand_MiddleDistal");

                AssignByName(hand.Ring, bone.Transform, boneName,
                    "XRHand_RingProximal",
                    "XRHand_RingIntermediate",
                    "XRHand_RingDistal");

                AssignByName(hand.Little, bone.Transform, boneName,
                    "XRHand_LittleProximal",
                    "XRHand_LittleIntermediate",
                    "XRHand_LittleDistal");

                AssignByName(hand.Little, bone.Transform, boneName,
                    "XRHand_PinkyProximal",
                    "XRHand_PinkyIntermediate",
                    "XRHand_PinkyDistal");
            }
        }

        /// <summary>
        /// Assigns a source Transform to a finger if its name matches one of the expected joint names.
        /// </summary>
        /// <param name="finger">Runtime data for the finger to populate.</param>
        /// <param name="source">Candidate source Transform.</param>
        /// <param name="actualName">Actual Transform name.</param>
        /// <param name="proximalName">Expected name for the first joint.</param>
        /// <param name="intermediateName">Expected name for the second joint.</param>
        /// <param name="distalName">Expected name for the third joint.</param>
        private static void AssignByName(
            FingerRuntime finger,
            Transform source,
            string actualName,
            string proximalName,
            string intermediateName,
            string distalName)
        {
            if (NameMatches(actualName, proximalName))
            {
                finger.SourceProximal = source;
            }
            else if (NameMatches(actualName, intermediateName))
            {
                finger.SourceIntermediate = source;
            }
            else if (NameMatches(actualName, distalName))
            {
                finger.SourceDistal = source;
            }
        }

        /// <summary>
        /// Checks whether a Transform name matches an expected XR Hands bone name.
        /// </summary>
        /// <remarks>
        /// Some prefabs or SDK versions may add prefixes to Transform names, so this method allows
        /// both exact matches and suffix matches.
        /// </remarks>
        /// <param name="actualName">Actual Transform name.</param>
        /// <param name="expectedName">Expected XR Hands bone name.</param>
        /// <returns>True if the names match; otherwise, false.</returns>
        private static bool NameMatches(string actualName, string expectedName)
        {
            return actualName == expectedName ||
                   actualName.EndsWith(expectedName, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets initialization for both hands.
        /// </summary>
        /// <remarks>
        /// This can be executed from the Inspector context menu. Use it after changing references,
        /// replacing an OVRHand prefab, or when you want to recapture the initial hand pose.
        /// </remarks>
        [ContextMenu("Reinitialize Hands")]
        private void ReinitializeHands()
        {
            ClearHand(_leftHand);
            ClearHand(_rightHand);
        }

        /// <summary>
        /// Clears initialization state and source bone references for one hand.
        /// </summary>
        /// <param name="hand">Runtime data for the hand to clear.</param>
        private static void ClearHand(HandRuntime hand)
        {
            hand.Initialized = false;
            hand.Thumb.SourceProximal = hand.Thumb.SourceIntermediate = hand.Thumb.SourceDistal = null;
            hand.Index.SourceProximal = hand.Index.SourceIntermediate = hand.Index.SourceDistal = null;
            hand.Middle.SourceProximal = hand.Middle.SourceIntermediate = hand.Middle.SourceDistal = null;
            hand.Ring.SourceProximal = hand.Ring.SourceIntermediate = hand.Ring.SourceDistal = null;
            hand.Little.SourceProximal = hand.Little.SourceIntermediate = hand.Little.SourceDistal = null;
        }

        private void OnDisable()
        {
            ClearHand(_leftHand);
            ClearHand(_rightHand);
        }
    }
}
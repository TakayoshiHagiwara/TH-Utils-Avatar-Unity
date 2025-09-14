// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (NNCT)
// Created: 2025/9/10
// Summary: Motion retargeter. To reflect movements from any avatar to another avatar.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace TH.Utils.Avatar
{
    public class MotionRetargeter : MonoBehaviour
    {
        public enum RetargetMode
        {
            AllBones = 0,
            SelectedBones = 1,
        }
        [Header("General settings")]
        public RetargetMode retargetMode;

        [Tooltip("The Transform of the avatar object that is the source of the movement. An Animator must be attached.")]
        public Transform OriginalRoot;
        [Tooltip("The Transform of the avatar object to reflect movement. An Animator must be attached.")]
        public Transform TargetRoot;
        [Tooltip("(Optional) Use when RetargetMode is set to AllBones. Objects with this name or later will not reflect the motion.")]
        public string EndBoneName;


        [Space(10), Header("Advanced Settings for SelectedBones Mode")]
        public Vector3 PositionOffset;
        [Tooltip("Attach a RotationConstraint to the specified target avatar's bone. The source of the movement will be the corresponding bone of the original avatar.")]
        public List<HumanBodyBones> RetargetHumanBodyBones = new List<HumanBodyBones>();

        // Start is called before the first frame update
        void Start()
        {
            switch (retargetMode)
            {
                case RetargetMode.AllBones:
                    break;
                case RetargetMode.SelectedBones:
                    AddPositionConstraint(OriginalRoot, TargetRoot, PositionOffset);
                    AddRotationConstraint(OriginalRoot.GetComponent<Animator>(), TargetRoot.GetComponent<Animator>(), RetargetHumanBodyBones);
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (retargetMode)
            {
                case RetargetMode.AllBones:
                    RetargetAll(OriginalRoot, TargetRoot, EndBoneName);
                    break;
                case RetargetMode.SelectedBones:
                    break;
            }

        }

        /// <summary>
        /// Reflects all local positions and rotations of bones from the original avatar to the target avatar.
        /// The original avatar and the target avatar must be objects with the same hierarchical structure.
        /// Apply to all bones using recursion.
        /// </summary>
        /// <param name="original">Original avatar transform.</param>
        /// <param name="target">Target avatar transform.</param>
        /// <param name="endName">(Optional) Objects with this name or later will not reflect the motion.</param>
        private void RetargetAll(Transform original, Transform target, string endName = "")
        {
            target.localPosition = original.localPosition;
            target.localRotation = original.localRotation;

            for (int iChild = 0; iChild < target.childCount; iChild++)
            {
                if (target.GetChild(iChild).name.Equals(EndBoneName))
                    break;

                RetargetAll(original.GetChild(iChild), target.GetChild(iChild));
            }
        }

        /// <summary>
        /// Attach a PositionConstraint to the target object.
        /// The source is the original object.
        /// </summary>
        /// <param name="original">Source transform.</param>
        /// <param name="target">Object to which a PositionConstraint is attached.</param>
        /// <param name="offset">PositionConstraint offset.</param>
        private void AddPositionConstraint(Transform original, Transform target, Vector3 offset)
        {
            PositionConstraint posConstraint = target.gameObject.AddComponent<PositionConstraint>();
            ConstraintSource constraintSource = new ConstraintSource();

            posConstraint.constraintActive = true;
            posConstraint.locked = true;
            posConstraint.translationOffset = offset;

            constraintSource.sourceTransform = original;
            constraintSource.weight = 1;
            posConstraint.AddSource(constraintSource);
        }

        /// <summary>
        /// Attach a RotationConstraint to the target object.
        /// The source is the original object.
        /// </summary>
        /// <param name="original">Source transform.</param>
        /// <param name="target">Object to which a RotationConstraint is attached.</param>
        private void AddRotationConstraint(Transform original, Transform target)
        {
            RotationConstraint rotConstraint = target.gameObject.AddComponent<RotationConstraint>();
            ConstraintSource constraintSource = new ConstraintSource();

            rotConstraint.constraintActive = true;
            rotConstraint.locked = true;
            rotConstraint.rotationOffset = target.rotation.eulerAngles;

            constraintSource.sourceTransform = original;
            constraintSource.weight = 1;
            rotConstraint.AddSource(constraintSource);
        }

        /// <summary>
        /// Attach a RotationConstraint to the target object.
        /// Get the specified Humanoid avatar bone from the Animator and attach a RotationConstraint to that bone.
        /// </summary>
        /// <param name="original">Source avatar animator.</param>
        /// <param name="target">Target avatar animator.</param>
        /// <param name="bones">HumanoidBodyBones list.</param>
        private void AddRotationConstraint(Animator original, Animator target, List<HumanBodyBones> bones)
        {
            foreach (HumanBodyBones bone in bones)
            {
                Transform originalBone = original.GetBoneTransform(bone);
                Transform targetBone = target.GetBoneTransform(bone);

                if (originalBone != null && targetBone != null)
                    AddRotationConstraint(originalBone, targetBone);
            }
        }
    }
}
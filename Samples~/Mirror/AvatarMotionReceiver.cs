// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Receives avatar motion messages over the network
//              and applies the latest local position and local rotation values to the target avatar bones.
// --------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace TH.Utils.Avatar
{
    /// <summary>
    /// Receives avatar motion messages over the network and applies the latest
    /// local position and local rotation values to the target avatar bones.
    /// </summary>
    [DefaultExecutionOrder(10000)]
    [RequireComponent(typeof(AvatarBoneList))]
    public sealed class AvatarMotionReceiver : MonoBehaviour
    {
        // The ID of the avatar whose pose messages should be applied.
        // Messages for other avatars are ignored.
        [SerializeField]
        private int _targetAvatarId = 0;

        private IReadOnlyList<Transform> _bones;

        private Vector3[] _receivedPositions;
        private Quaternion[] _receivedRotations;

        private uint _lastSequence;

        // Indicates whether at least one valid pose message has been received.
        private bool _hasReceivedMotion;

        private void Awake()
        {
            _bones = GetComponent<AvatarBoneList>().Bones;
        }

        private void OnEnable()
        {
            NetworkClient.RegisterHandler<AvatarMotionMessage>(OnReceiveMotion, requireAuthentication: false);
        }

        private void OnDisable()
        {
            if (NetworkClient.active)
                NetworkClient.UnregisterHandler<AvatarMotionMessage>();
        }

        /// <summary>
        /// Validates an incoming motion message and stores it as the latest motion.
        /// Messages for other avatars, invalid bone counts, and old or
        /// out-of-order messages are ignored.
        /// </summary>
        /// <param name="message">The received avatar motion message.</param>
        private void OnReceiveMotion(AvatarMotionMessage message)
        {
            if (message.AvatarId != _targetAvatarId)
                return;

            if (message.LocalPositions == null || message.LocalRotations == null)
                return;

            if (message.LocalPositions.Length != _bones.Count ||
                message.LocalRotations.Length != _bones.Count)
            {
                Debug.LogError($"Avatar {_targetAvatarId}: The number of bones on the sender and receiver do not match.");
                return;
            }

            // Ignore old packet
            if (_hasReceivedMotion && !IsNewer(message.Sequence, _lastSequence))
                return;

            _lastSequence = message.Sequence;

            _receivedPositions = message.LocalPositions;
            _receivedRotations = message.LocalRotations;

            _hasReceivedMotion = true;
        }

        private void LateUpdate()
        {
            if (!_hasReceivedMotion)
                return;

            for (int i = 0; i < _bones.Count; i++)
                _bones[i].SetLocalPositionAndRotation(_receivedPositions[i], _receivedRotations[i]);
        }

        /// <summary>
        /// Determines whether a sequence number is newer than the previously
        /// accepted sequence number, including unsigned integer wraparound.
        /// </summary>
        /// <param name="value">The newly received sequence number.</param>
        /// <param name="previous">The previously accepted sequence number.</param>
        /// <returns>
        /// True when <paramref name="value"/> is newer; otherwise, false.
        /// </returns>
        private static bool IsNewer(uint value, uint previous)
        {
            return unchecked((int)(value - previous)) > 0;
        }
    }
}
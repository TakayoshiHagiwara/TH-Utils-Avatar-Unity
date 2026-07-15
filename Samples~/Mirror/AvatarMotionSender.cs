// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Captures the local position and local rotation of all avatar bones
//              and sends the motion data over the network using Mirror.
// --------------------------------------------------

using Mirror;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Captures the local position and local rotation of all avatar bones
/// and sends the motion data over the network using Mirror.
/// </summary>
[DefaultExecutionOrder(10000)]
[RequireComponent(typeof(AvatarBoneList))]
public sealed class AvatarMotionSender : MonoBehaviour
{
    // Identifies the avatar controlled by this sender.
    // The receiver applies only messages whose avatar ID matches its target ID.
    [SerializeField]
    private int _avatarId = 0;

    // Maximum number of motion messages sent per second.
    // The actual send rate cannot exceed Unity's frame rate.
    [SerializeField, Min(1f)]
    private float _sendRate = 90f;

    private IReadOnlyList<Transform> _bones;
    private Vector3[] _positions;
    private Quaternion[] _rotations;

    // Sequence number assigned to each outgoing motion message.
    // Increments by 1 with each transmission.
    // It can be used by receivers to detect old or out-of-order messages.
    private uint _sequence;

    // The earliest time at which the next motion message may be sent.
    private float _nextSendTime;

    private void Awake()
    {
        _bones = GetComponent<AvatarBoneList>().Bones;

        _positions = new Vector3[_bones.Count];
        _rotations = new Quaternion[_bones.Count];
    }

    private void LateUpdate()
    {
        bool _canSend = NetworkServer.active || (NetworkClient.active && NetworkClient.isConnected);

        if (!_canSend)
            return;

        // Limits the number of transmissions to the specified sendRate, regardless of the PC's frame rate
        if (Time.unscaledTime < _nextSendTime)
            return;

        _nextSendTime = Time.unscaledTime + 1f / _sendRate;

        CaptureMotion();
        SendMotion();
    }

    /// <summary>
    /// Captures the current local position and local rotation of every bone.
    /// </summary>
    private void CaptureMotion()
    {
        for (int i = 0; i < _bones.Count; i++)
        {
            _positions[i] = _bones[i].localPosition;
            _rotations[i] = _bones[i].localRotation;
        }
    }

    /// <summary>
    /// Creates an avatar motion message and sends it through Mirror's
    /// unreliable channel.
    ///
    /// When running as a host, the server sends the message to all clients.
    /// When running as a regular client, the message is sent to the server.
    /// </summary>
    private void SendMotion()
    {
        AvatarMotionMessage message = new AvatarMotionMessage
        {
            AvatarId = _avatarId,
            Sequence = _sequence++,

            LocalPositions = _positions,
            LocalRotations = _rotations
        };

        // Host
        if (NetworkServer.active)
        {
            NetworkServer.SendToAll(message, Channels.Unreliable);
        }
        // Client
        else
        {
            NetworkClient.Send(message, Channels.Unreliable);
        }
    }
}

// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Stores local positions and rotations for each avatar bone.
// --------------------------------------------------

using Mirror;
using UnityEngine;

namespace TH.Utils.Avatar
{
    public struct AvatarMotionMessage : NetworkMessage
    {
        public int AvatarId;
        public uint Sequence;

        public Vector3[] LocalPositions;
        public Quaternion[] LocalRotations;
    }
}
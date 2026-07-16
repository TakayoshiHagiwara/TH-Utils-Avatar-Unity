// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/14
// Summary:     Stores recorded local positions and rotations for each avatar bone.
// --------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Utils.Avatar
{
    /// <summary>
    /// Stores recorded local positions and rotations for each avatar bone.
    /// </summary>
    public sealed class AvatarMotionData
    {
        /// <summary>
        /// Gets the elapsed recording time for each frame.
        /// </summary>
        public List<float> Times { get; }

        /// <summary>
        /// Gets the number of recorded frames.
        /// </summary>
        public int FrameCount => Times.Count;

        /// <summary>
        /// Gets the recorded local positions indexed by bone name.
        /// </summary>
        public Dictionary<string, List<Vector3>> Positions { get; }

        /// <summary>
        /// Gets the recorded local rotations indexed by bone name.
        /// </summary>
        public Dictionary<string, List<Quaternion>> Rotations { get; }

        /// <summary>
        /// Initializes a new avatar motion data container.
        /// </summary>
        /// <param name="bones">The bones included in the recording.</param>
        /// <param name="initialCapacity">The initial frame capacity allocated for each bone track.</param>
        public AvatarMotionData(IReadOnlyList<Transform> bones, int initialCapacity)
        {
            if (bones == null)
                throw new ArgumentNullException(nameof(bones));

            Times = new List<float>(initialCapacity);

            Positions = new Dictionary<string, List<Vector3>>(bones.Count);
            Rotations = new Dictionary<string, List<Quaternion>>(bones.Count);

            foreach (Transform bone in bones)
            {
                if (bone == null)
                    throw new ArgumentException("The bone list contains a null transform.", nameof(bones));

                if (Positions.ContainsKey(bone.name))
                    throw new ArgumentException($"The bone name '{bone.name}' is duplicated.", nameof(bones));

                Positions.Add(bone.name, new List<Vector3>(initialCapacity));
                Rotations.Add(bone.name, new List<Quaternion>(initialCapacity));
            }
        }

        /// <summary>
        /// Clears all recorded frames while preserving allocated list capacities.
        /// </summary>
        public void Clear()
        {
            Times.Clear();

            foreach (List<Vector3> positions in Positions.Values)
                positions.Clear();

            foreach (List<Quaternion> rotations in Rotations.Values)
                rotations.Clear();
        }
    }
}
// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/14
// Summary:     Collects and exposes humanoid bone transforms from the attached component.
// --------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collects and exposes humanoid bone transforms from the attached
/// <see cref="Animator"/> component.
/// </summary>
public sealed class AvatarBoneList : MonoBehaviour
{
    /// <summary> 
    /// Stores the humanoid bone transforms collected from the attached avatar.
    /// </summary>
    [SerializeField]
    private List<Transform> _bones = new();
    /// <summary>
    /// Gets the collected humanoid bone transforms.
    /// </summary>
    public IReadOnlyList<Transform> Bones => _bones;

    /// <summary>
    /// Specifies the humanoid bone types that should be excluded from collection.
    /// </summary>
    [SerializeField]
    private List<HumanBodyBones> _bonesToExclude = new()
    {
        HumanBodyBones.Jaw,
        HumanBodyBones.LeftEye,
        HumanBodyBones.RightEye,
        HumanBodyBones.LeftToes,
        HumanBodyBones.RightToes,
        HumanBodyBones.LastBone,
        HumanBodyBones.LeftThumbDistal,
        HumanBodyBones.LeftIndexDistal,
        HumanBodyBones.LeftMiddleDistal,
        HumanBodyBones.LeftRingDistal,
        HumanBodyBones.LeftLittleDistal,
        HumanBodyBones.RightThumbDistal,
        HumanBodyBones.RightIndexDistal,
        HumanBodyBones.RightMiddleDistal,
        HumanBodyBones.RightRingDistal,
        HumanBodyBones.RightLittleDistal
    };

    private Animator _animator;

    /// <summary>
    /// Initializes the component and collects the avatar's humanoid bones
    /// when the component is first added or reset in the Unity Editor.
    /// </summary>
    private void Reset()
    {
        TryGetComponent(out _animator);
        RefreshBones();
    }

    /// <summary>
    /// Rebuilds the bone list using the humanoid avatar assigned to the
    /// attached <see cref="Animator"/>.
    /// </summary>
    /// <remarks>
    /// Bone types listed in <c>_bonesToExclude</c> are omitted.
    /// The list remains empty when the animator or its humanoid avatar
    /// is unavailable or invalid.
    /// </remarks>
    [ContextMenu("Refresh Bones")]
    public void RefreshBones()
    {
        _bones ??= new List<Transform>();
        _bones.Clear();

        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_animator == null || _animator.avatar == null || !_animator.avatar.isValid || !_animator.avatar.isHuman)
            return;

        for (int i = 0; i < (int)HumanBodyBones.LastBone; i++)
        {
            HumanBodyBones boneType = (HumanBodyBones)i;

            if (_bonesToExclude.Contains(boneType))
                continue;

            Transform bone = _animator.GetBoneTransform(boneType);

            if (bone != null)
                _bones.Add(bone);
        }
    }
}
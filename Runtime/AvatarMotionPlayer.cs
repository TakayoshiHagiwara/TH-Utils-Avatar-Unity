// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Plays recorded avatar motion at a fixed update rate.
// Important:   Use the same Time.fixedDeltaTime value for both recording and playback.
// --------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Utils.Avatar
{
    /// <summary>
    /// Plays recorded avatar motion at a fixed update rate.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AvatarBoneList))]
    [RequireComponent(typeof(Animator))]
    public sealed class AvatarMotionPlayer : MonoBehaviour
    {
        [Header("Input"), SerializeField]
        private KeyCode _startPlayingKey = KeyCode.P;
        [SerializeField]
        private KeyCode _stopPlayingKey = KeyCode.O;

        [Header("Playback"), SerializeField]
        private bool _loop;

        [SerializeField, Min(0.0f)]
        private float _playbackSpeed = 1.0f;

        [Header("Input File"), SerializeField]
        private string _dataPath = "/Resources/AvatarMotionData/";
        [SerializeField]
        private string _fileName = "AvatarMotionData";

        private IReadOnlyList<Transform> _bones;
        private List<Vector3>[] _positionTracks;
        private List<Quaternion>[] _rotationTracks;

        private AvatarMotionData _motionData;
        private Animator _animator;
        private int _currentFrame;
        private float _playbackTime;
        private bool _isPlaying;

        /// <summary>Gets a value indicating whether motion playback is active.</summary>
        public bool IsPlaying => _isPlaying;

        /// <summary>Gets or sets whether motion playback loops.</summary>
        public bool Loop { get => _loop; set => _loop = value; }

        /// <summary>Gets or sets the playback speed multiplier.</summary>
        public float PlaybackSpeed { get => _playbackSpeed; set => _playbackSpeed = Mathf.Max(0.0f, value); }

        /// <summary>Gets or sets the motion data directory path.</summary>
        public string DataPath { get => _dataPath; set => _dataPath = value; }

        /// <summary>Gets or sets the motion data file name.</summary>
        public string FileName { get => _fileName; set => _fileName = value; }

        /// <summary>
        /// Initializes the avatar bones and loads the motion data.
        /// </summary>
        private void Awake()
        {
            AvatarBoneList avatarBoneList = GetComponent<AvatarBoneList>();
            avatarBoneList.RefreshBones();

            _bones = avatarBoneList.Bones;
            _animator = GetComponent<Animator>();

            LoadMotionData();
        }

        /// <summary>
        /// Processes playback input.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(_startPlayingKey))
                StartPlaying();

            if (Input.GetKeyDown(_stopPlayingKey))
                StopPlaying();
        }

        /// <summary>
        /// Applies the current motion frame and advances playback time.
        /// </summary>
        private void FixedUpdate()
        {
            if (!_isPlaying)
                return;

            ApplyFrame(_currentFrame);

            if (_currentFrame == _motionData.FrameCount - 1)
            {
                if (_loop)
                {
                    _currentFrame = 0;
                    _playbackTime = 0.0f;
                }
                else
                    StopPlaying();

                return;
            }

            _playbackTime += Time.fixedDeltaTime * _playbackSpeed;

            while (_currentFrame + 1 < _motionData.FrameCount && _motionData.Times[_currentFrame + 1] <= _playbackTime)
                _currentFrame++;
        }

        /// <summary>
        /// Applies the specified motion frame to the avatar bones.
        /// </summary>
        /// <param name="frameIndex">The frame index to apply.</param>
        private void ApplyFrame(int frameIndex)
        {
            for (int i = 0; i < _bones.Count; i++)
            {
                _bones[i].localPosition = _positionTracks[i][frameIndex];
                _bones[i].localRotation = _rotationTracks[i][frameIndex];
            }
        }

        /// <summary>
        /// Starts motion playback from the first frame.
        /// </summary>
        public void StartPlaying()
        {
            if (_isPlaying || _motionData == null || _motionData.FrameCount == 0)
                return;

            _currentFrame = 0;
            _playbackTime = 0.0f;
            _isPlaying = true;
            _animator.enabled = false;

            Debug.Log("Avatar motion playback started.");
        }

        /// <summary>
        /// Stops motion playback.
        /// </summary>
        public void StopPlaying()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;
            _animator.enabled = true;

            Debug.Log("Avatar motion playback stopped.");
        }

        /// <summary>
        /// Loads the motion data and caches each bone track.
        /// </summary>
        [ContextMenu("Load Motion Data")]
        public void LoadMotionData()
        {
            if (_isPlaying)
                StopPlaying();

            try
            {
                _motionData = CsvManager.ReadMotionData(_bones, _dataPath, _fileName);
                InitializeMotionTracks();

                Debug.Log($"Avatar motion data was loaded. Frames: {_motionData.FrameCount}");
            }
            catch (Exception exception)
            {
                _motionData = null;
                Debug.LogException(exception);
            }
        }

        /// <summary>
        /// Returns a copy of the currently loaded motion data.
        /// </summary>
        /// <returns>A copy of the loaded motion data, or null if no data is loaded.</returns>
        public AvatarMotionData GetMotionDataCopy()
        {
            return _motionData?.Clone();
        }

        /// <summary>
        /// Sets the motion data used for playback.
        /// </summary>
        /// <param name="motionData">The motion data to use for playback.</param>
        public void SetMotionData(AvatarMotionData motionData)
        {
            if (motionData == null)
                throw new ArgumentNullException(nameof(motionData));

            if (_isPlaying)
                StopPlaying();

            _motionData = motionData.Clone();
            InitializeMotionTracks();
        }

        /// <summary>
        /// Caches the motion tracks for each avatar bone.
        /// </summary>
        private void InitializeMotionTracks()
        {
            _positionTracks = new List<Vector3>[_bones.Count];
            _rotationTracks = new List<Quaternion>[_bones.Count];

            for (int i = 0; i < _bones.Count; i++)
            {
                string boneName = _bones[i].name;
                _positionTracks[i] = _motionData.Positions[boneName];
                _rotationTracks[i] = _motionData.Rotations[boneName];
            }
        }
    }
}
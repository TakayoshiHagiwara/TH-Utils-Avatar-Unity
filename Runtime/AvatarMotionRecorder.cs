// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/14
// Summary:     Records avatar bone poses at a fixed update rate.
// --------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Utils.Avatar
{
    /// <summary>
    /// Records avatar bone poses at a fixed update rate.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AvatarBoneList))]
    [RequireComponent(typeof(Animator))]
    public class AvatarMotionRecorder : MonoBehaviour
    {
        [Header("Input"), SerializeField]
        private KeyCode _startRecordingKey = KeyCode.R;
        [SerializeField]
        private KeyCode _stopRecordingKey = KeyCode.T;

        [Header("Recording"), SerializeField, Min(1)]
        private int _maximumRecordingSeconds = 300;

        [Header("Output"), SerializeField]
        private string _dataPath = "/Resources/AvatarMotionData/";
        [SerializeField]
        private string _fileName = "AvatarMotionData";


        private IReadOnlyList<Transform> _bones;
        private List<Vector3>[] _positionTracks;
        private List<Quaternion>[] _rotationTracks;

        private AvatarMotionData _motionData;
        private bool _isRecording;
        private bool _isFirstRecordingFrame;
        private float _recordingStartTime;

        /// <summary>
        /// Gets a value indicating whether motion recording is active.
        /// </summary>
        public bool IsRecording => _isRecording;

        private void Awake()
        {
            AvatarBoneList avatarBoneList = GetComponent<AvatarBoneList>();

            avatarBoneList.RefreshBones();
            _bones = avatarBoneList.Bones;

            InitializeMotionData();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(_startRecordingKey))
                StartRecording();

            if (Input.GetKeyDown(_stopRecordingKey))
            {
                StopRecording();
                SaveMotionData();
            }    
        }

        private void FixedUpdate()
        {
            if (!_isRecording)
                return;

            if (_isFirstRecordingFrame)
            {
                _recordingStartTime = Time.fixedTime;
                _isFirstRecordingFrame = false;
            }

            float elapsedTime = Time.fixedTime - _recordingStartTime;
            _motionData.Times.Add(elapsedTime);

            for (int i = 0; i < _bones.Count; i++)
            {
                Transform bone = _bones[i];

                _positionTracks[i].Add(bone.localPosition);
                _rotationTracks[i].Add(bone.localRotation);
            }
        }

        /// <summary>
        /// Starts a new fixed-rate motion recording.
        /// </summary>
        public void StartRecording()
        {
            if (_isRecording)
                return;

            _motionData.Clear();

            _isFirstRecordingFrame = true;
            _isRecording = true;

            Debug.Log("Avatar motion recording started.");
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        public void StopRecording()
        {
            if (!_isRecording)
                return;

            _isRecording = false;

            Debug.Log("Avatar motion recording stopped.");
        }

        /// <summary>
        /// Writes the captured motion to a CSV file.
        /// </summary>
        public void SaveMotionData()
        {
            if (_motionData.FrameCount == 0)
            {
                Debug.LogWarning("No motion frames were recorded.");
                return;
            }

            try
            {
                string filePath = CsvManager.WriteMotionData(_motionData, _dataPath, _fileName);
                Debug.Log($"Avatar motion data was saved to: {filePath}");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
            }
        }

        /// <summary>
        /// Initializes motion tracks and caches their list references.
        /// </summary>
        private void InitializeMotionData()
        {

            int initialCapacity = Mathf.CeilToInt(_maximumRecordingSeconds / Time.fixedDeltaTime);

            _motionData = new AvatarMotionData(_bones, initialCapacity);

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
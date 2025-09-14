// --------------------------------------------------
// Author:      Takayoshi Hagiwara (Toyohashi University of Technology)
// Created:     2019/8/27
// Summary:     Apply delays to the movements of avatars, etc. The original and target should be the same object structure.
// --------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace TH.Utils.Avatar
{
    public class MotionDelayer : MonoBehaviour
    {
        [SerializeField, Tooltip("Objects that are the source of movement")]
        private Transform _originalRoot = default;
        [SerializeField, Tooltip("Objects that reflect movement with delay")]
        private Transform _targetRoot = default;

        [SerializeField, Tooltip("Delay [s]")]
        private float _delay = 0;
        private float _elapsedTime;

        // Temporarily save the Transform of a GameObject with the same name in a dictionary type.
        private Dictionary<string, List<Vector3>> _positionBuffer = new Dictionary<string, List<Vector3>>();
        private Dictionary<string, List<Quaternion>> _rotationBuffer = new Dictionary<string, List<Quaternion>>();

        private bool _isDelayStart = false;

        // Use this for initialization
        void Start()
        {
            InitializeDictionary(_originalRoot);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RecordParameter(_originalRoot);

            if (_isDelayStart)
                ApplyParameter(_targetRoot);
            else
                CheckCurrentTime();
        }

        /// <summary>
        /// Temporarily save the original Transform.
        /// </summary>
        /// <param name="original">Original transform</param>
        public void RecordParameter(Transform original)
        {
            if (_positionBuffer.ContainsKey(original.name))
            {
                _positionBuffer[original.name].Add(original.position);
                _rotationBuffer[original.name].Add(original.rotation);
            }

            for (int iChild = 0; iChild < original.childCount; iChild++)
            {
                RecordParameter(original.GetChild(iChild));
            }
        }

        /// <summary>
        /// If the target object contains a GameObject with the same name as the original,
        /// the temporarily saved Transform is applied to the target object.
        /// </summary>
        /// <param name="target">The Transform to which the Transform is retargeted.</param>
        public void ApplyParameter(Transform target)
        {
            if (_rotationBuffer.ContainsKey(target.name))
            {
                target.position = _positionBuffer[target.name][0];
                target.rotation = _rotationBuffer[target.name][0];

                _positionBuffer[target.name].RemoveAt(0);
                _rotationBuffer[target.name].RemoveAt(0);
            }

            for (int iChild = 0; iChild < target.childCount; iChild++)
            {
                ApplyParameter(target.GetChild(iChild));
            }
        }

        /// <summary>
        /// Clear and initialize dictionary for temporarily save the Transform of a GameObject.
        /// Initialize with the structure and name of the original object.
        /// </summary>
        /// <param name="original">Original transform</param>
        private void InitializeDictionary(Transform original)
        {
            _positionBuffer.Clear();
            _rotationBuffer.Clear();

            InitializeDictionaryKeyValue(original);
        }

        /// <summary>
        /// Initialize dictionary key and value for temporarily save the Transform of a GameObject.
        /// Initialize with the structure and name of the original object.
        /// </summary>
        /// <param name="original">Original transform</param>
        private void InitializeDictionaryKeyValue(Transform original)
        {
            _positionBuffer.Add(original.name, new List<Vector3>());
            _rotationBuffer.Add(original.name, new List<Quaternion>());

            for (int iChild = 0; iChild < original.childCount; iChild++)
                InitializeDictionaryKeyValue(original.GetChild(iChild));
        }

        /// <summary>
        /// Check current time duration.
        /// If elapsedTime exceeds the set delay time, the delay start flag is set to True.
        /// </summary>
        private void CheckCurrentTime()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _delay)
                _isDelayStart = true;
        }

        /// <summary>
        /// Reset all parameters.
        /// When you update the delay, please run this methods.
        /// </summary>
        /// <param name="original">Original transform</param>
        public void ResetAll(Transform original)
        {
            InitializeDictionary(original);
            _elapsedTime = 0;
            _isDelayStart = false;
        }
    }
}
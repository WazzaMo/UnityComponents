/*
 * GyroTracking Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Tools.Common;

using Actor.Events;

namespace Actor.Inputs {

    public class GyroTracking : MonoBehaviour {
        [Tooltip("For updates on Device Orientiation")]
        [SerializeField] private GyroEvent GyroOrientationListeners;

        [Header("Editor Simulation")]
        [Tooltip("Use keyboard to simulate?")]
        [SerializeField] private bool UseKeyBoardToGyro = true;

        [Tooltip("Keyboard arrangement for simulation in Editor")]
        [SerializeField]
        private GyroSimConfig KeyConfiguration = new GyroSimConfig() {
            XAxis = new AxisConfig() { Clockwise = KeyCode.DownArrow, Anticlockwise = KeyCode.UpArrow },
            YAxis = new AxisConfig() { Clockwise = KeyCode.RightArrow, Anticlockwise = KeyCode.LeftArrow },
            ZAxis = new AxisConfig() { Clockwise = KeyCode.A, Anticlockwise = KeyCode.Z }
        };

        private IGyroReader _GyroReader;

        public GyroEvent GyroListeners { get { return GyroOrientationListeners; } }
        public bool IsReady { get { return _GyroReader != null; } }

        void Start() {
            SetupTrackingSources();
            if (HasListeners) {
                Debug.Log("GyroTracking has associated listeners.");
            } else {
                Debug.LogWarning("No listeners associated with GyroTracking.");
            }
        }

        void Update() {
            if ( HasListeners && IsReady ){
                Quaternion value = TakeReading();
                NotifyListeners(value);
            }
        }

        private bool HasListeners {
            get { return GyroOrientationListeners.GetPersistentEventCount() > 0; }
        }

        private Quaternion TakeReading() {
            return _GyroReader.TakeOrientationReading();
        }

        private void NotifyListeners(Quaternion value) {
            GyroOrientationListeners.Invoke(value);
        }

        private void SetupTrackingSources() {
            if (Application.isMobilePlatform) {
                _GyroReader = new GyroHardwareReader();
            } else if (UseKeyBoardToGyro) {
                _GyroReader = new GyroSimulatedReader(KeyConfiguration);
                Debug.Log("GyroTracking configured for Keyboard input");
            } else {
                _GyroReader = null;
                Debug.Log("GyroTracking not configured for input!");
            }
        }
    }

}
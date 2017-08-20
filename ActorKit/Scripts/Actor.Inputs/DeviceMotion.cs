/*
 * DeviceMotion Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

/*
* Definition for Pitch, Yaw and Roll from Wikipedia https://en.wikipedia.org/wiki/Aircraft_principal_axes
* Where change in Pitch makes you look up or down, change in yaw makes you look in a different
* direction relative to the ground and Roll makes you lose balance and end up on your side.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Tools.Common;

using Actor.Events;

namespace Actor.Inputs {

    public class DeviceMotion : MonoBehaviour {
        [Header("For ArcadeMotionListenerXXX Type")]
        [Tooltip(".\nVector3 pointing to ground")]
        [SerializeField] private DeviceMotionEvent ArcadeMotionListeners;

        [Header("For RelativeInputEvent Handlers")]
        [Tooltip("Roll 0 to +1 left; 0 to -1 right")]
        [SerializeField] private RelativeInputEvent RollRelativeListeners;

        [Tooltip("Pitch 0 to +1 look down; 0 to -1 up")]
        [SerializeField] private RelativeInputEvent PitchRelativeListeners;

        [Tooltip("Yaw (unreliable) 0 to +1 turn right; 0 to -1 left")]
        [SerializeField] private RelativeInputEvent YawRelativeListeners;

        [SerializeField] private Vector3 SensitivityFactor = Vector3.one;

        [Header("Editor Simulation")]
        [Tooltip("Use Z,X and Arrow keys?")]
        [SerializeField] private bool UseKeyBoardToSimulateAccelerometer = true;

        [SerializeField] private DeviceMotionKeyboardSim _MotionSimulator = null;

        private DeviceMotionToRelativeInput _RelativeInput;


        void Start() {
            if (Input.gyro != null) {
                UiDebug.Log("Gyro available: {0}", Input.gyro.attitude);
            } else {
                UiDebug.Log("No gyro (null)");
            }
            SetupForRelativeEvents();
            if (UseKeyBoardToSimulateAccelerometer) {
                SetupKeyboardSim();
            }
        }

        void Update() {
            if (Input.gyro != null) {
                UiDebug.Log("Gyro available: {0}", Input.gyro.attitude);
            } else {
                UiDebug.Log("No gyro (null)");
            }
            if ( HasListeners ){
                Vector3 Direction = GetDeviceOrSimulatedDirectionTowardGravity();
                NotifyListeners(Direction);
                if (_RelativeInput != null) {
                    _RelativeInput.NotifyDirection(Direction);
                }
            }
        }

        private void NotifyListeners(Vector3 Direction) {
            ArcadeMotionListeners.Invoke(Direction);
        }

        private bool HasListeners {
            get { return ArcadeMotionListeners.GetPersistentEventCount() > 0; }
        }

        private Vector3 GetDeviceOrSimulatedDirectionTowardGravity() {
            if (IsDesktop() && UseKeyBoardToSimulateAccelerometer) {
                return GetSimulatedDirectionTowardGravity();
            } else {
                return GetDeviceDirectionTowardGravity();
            }
        }

        private bool IsDesktop() {
            return
                Application.platform == RuntimePlatform.OSXEditor
                || Application.platform == RuntimePlatform.OSXPlayer
                || Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.LinuxEditor
                || Application.platform == RuntimePlatform.LinuxPlayer
                ;
        }

        private Vector3 GetDeviceDirectionTowardGravity() {
            AccelerationEvent accelEvent;
            Vector3 sumRotations = Vector3.zero;
            Vector3 direction;

            for (int index = 0; index < Input.accelerationEventCount; index++) {
                accelEvent = Input.accelerationEvents[index];
                sumRotations += accelEvent.deltaTime * accelEvent.acceleration;
            }
            direction.y = sumRotations.y * SensitivityFactor.y;
            direction.x = sumRotations.x * SensitivityFactor.x;
            direction.z = sumRotations.z * SensitivityFactor.z;
            direction.Normalize();
            return direction;
        }

        private Vector3 GetSimulatedDirectionTowardGravity() {
            if (_MotionSimulator != null) {
                _MotionSimulator.SimulateAccelerometer();
                return _MotionSimulator.GetSimulatedDirectionTowardGravity();
            } else {
                return Vector3.zero;
            }
        }

        private void SetupKeyboardSim() {
            _MotionSimulator = new DeviceMotionKeyboardSim();
        }

        private void SetupForRelativeEvents() {
            _RelativeInput = new DeviceMotionToRelativeInput(
                RollRelativeListeners,
                PitchRelativeListeners,
                YawRelativeListeners
            );
        }
    }

}
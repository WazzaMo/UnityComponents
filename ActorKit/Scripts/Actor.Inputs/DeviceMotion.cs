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

        [SerializeField] private RotationEvent _RotationListeners;

        [Header("For RelativeInputEvent Handlers")]
        [Tooltip("Roll 0 to +1 left; 0 to -1 right")]
        [SerializeField] private RelativeInputEvent RollRelativeListeners;

        [Tooltip("Pitch 0 to +1 look down; 0 to -1 up")]
        [SerializeField] private RelativeInputEvent PitchRelativeListeners;

        [Tooltip("Yaw (unreliable) 0 to +1 turn right; 0 to -1 left")]
        [SerializeField] private RelativeInputEvent YawRelativeListeners;


        [Header("Editor Simulation")]
        [Tooltip("Use Z,X and Arrow keys?")]
        [SerializeField] private bool UseKeyBoardToSimulateAccelerometer = true;

        [SerializeField] private DeviceMotionKeyboardSim _MotionSimulator = new DeviceMotionKeyboardSim();
        [SerializeField] private DeviceMotionSourceAccelGyro _MobileMotion = new DeviceMotionSourceAccelGyro();

        public RotationEvent RotationListeners { get { return _RotationListeners; } }

        private DeviceMotionToRelativeInput _RelativeInput;


        void Start() {
            SetupForRelativeEvents();
            SetupMotionSources();
            if (HasListeners) {
                Debug.Log("DeviceMotion has associated listeners.");
            } else {
                Debug.LogWarning("No listeners associated with DeviceMotion.");
            }
        }

        void Update() {
            if ( HasListeners ){
                Vector3 Direction = GetDeviceDirectionTowardGravity();
                NotifyArcadeListeners(Direction);
                if (_RelativeInput != null) {
                    _RelativeInput.NotifyDirection(Direction);
                }
            }
        }

        private void NotifyArcadeListeners(Vector3 Direction) {
            ArcadeMotionListeners.Invoke(Direction);
        }

        private bool HasListeners {
            get { return _RelativeInput.HasListeners || ArcadeMotionListeners.GetPersistentEventCount() > 0; }
        }


        private Vector3 GetDeviceDirectionTowardGravity() {
            Debug.LogFormat("UseKB? {0} _MoSim.HW? {1}", UseKeyBoardToSimulateAccelerometer, _MotionSimulator.IsHardwareAvailable);
            if (UseKeyBoardToSimulateAccelerometer && _MotionSimulator.IsHardwareAvailable) {
                Vector3 v = _MotionSimulator.GetDeviceDirectionTowardGravity();
                Debug.LogFormat("New direction {0}", v);
                return v;
            } else {
                return _MobileMotion.GetDeviceDirectionTowardGravity();
            }
        }

        private void SetupMotionSources() {
            SetupDeviceIfConfigured(_MotionSimulator);
            SetupDeviceIfConfigured(_MobileMotion);
        }

        private void SetupDeviceIfConfigured(IDeviceMotionSource device) {
            if (device.IsHardwareAvailable) {
                if (device.IsConfiguredCorrectly) {
                    device.SetupSource();
                } else {
                    Debug.LogWarning(device.GetType().Name + ":" + device.GetConfigurationMessage());
                }
            }
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
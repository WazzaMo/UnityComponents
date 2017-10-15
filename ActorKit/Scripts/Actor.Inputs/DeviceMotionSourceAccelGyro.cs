/*
 * DeviceMotion Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Actor.Inputs {

    public class DeviceMotionSourceAccelGyro : IDeviceMotionSource {
        [SerializeField] private Vector3 SensitivityFactor = Vector3.one;

        private bool _HasGyro;

        public bool IsHardwareAvailable { get { return IsDeviceWithHardware(); } }

        public bool IsConfiguredCorrectly { get { return true; } }

        public void SetupSource() {
            try {
                _HasGyro = true;
                Input.gyro.enabled = true;
            } catch(Exception) {
                _HasGyro = false;
            }
        }

        public Vector3 GetDeviceDirectionTowardGravity() {
            Vector3 toGravity;
            if (_HasGyro) {
                toGravity = Vector3.Normalize(ReadAccelerometer() + ReadGyro());
            } else {
                toGravity = ReadAccelerometer();
            }
            return toGravity;
        }

        public string GetConfigurationMessage() {
            if (IsConfiguredCorrectly) {
                return "";
            } else {
                return "";
            }
        }

        internal DeviceMotionSourceAccelGyro() { }

        private Vector3 ReadGyro() {
            return AdjustGyro(Input.gyro.attitude) * Vector3.down;
        }

        private Quaternion AdjustGyro(Quaternion gyroRotation) {
            return new Quaternion(gyroRotation.x, gyroRotation.y, -gyroRotation.z, -gyroRotation.w);
        }

        private Vector3 ReadAccelerometer() {
            AccelerationEvent accelEvent;
            Vector3 sumRotations = Vector3.zero;
            Vector3 direction;

            for (int index = 0; index < Input.accelerationEventCount; index++) {
                accelEvent = Input.accelerationEvents[index];
                sumRotations += accelEvent.deltaTime * accelEvent.acceleration;
            }
            direction.y = sumRotations.y;
            direction.x = sumRotations.x;
            direction.z = sumRotations.z;
            direction.Normalize();
            return direction;
        }

        private bool IsDeviceWithHardware() {
            return Application.isMobilePlatform
                || Application.platform == RuntimePlatform.Android
                || Application.platform == RuntimePlatform.IPhonePlayer
                || Application.platform == RuntimePlatform.TizenPlayer
                ;
        }
    }

}

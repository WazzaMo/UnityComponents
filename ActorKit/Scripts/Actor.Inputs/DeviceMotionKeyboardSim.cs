/*
 * DeviceMotionKeyboardSim Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Inputs {

    [Serializable]
    public class DeviceMotionKeyboardSim : IDeviceMotionSource {

        public KeyCode
            ROLL_CLOCK = KeyCode.RightArrow,
            ROLL_ANTI = KeyCode.LeftArrow,

            PITCH_CLOCK = KeyCode.UpArrow,
            PITCH_ANTI = KeyCode.DownArrow,

            YAW_CLOCK = KeyCode.Z,
            YAW_ANTI = KeyCode.X,
            
            RESET = KeyCode.Space;

        [SerializeField] private float Increment = 5f;

        private Vector3 _Direction;
        private float _RollAngle;
        private float _PitchAngle;
        private float _YawAngle;

        public bool IsHardwareAvailable { get { return IsDesktop(); } }

        public bool IsConfiguredCorrectly { get { return true; } }

        public void SetupSource() {
            ResetAllState();
        }

        public string GetConfigurationMessage() {
            if (IsConfiguredCorrectly) {
                return "";
            } else {
                return "";
            }
        }

        public Vector3 GetDeviceDirectionTowardGravity() {
            SimulateAccelerometer();
            return _Direction;
        }

        internal DeviceMotionKeyboardSim() {
            ResetAllState();
        }

        private void SimulateAccelerometer() {
            CheckResetKey();
            UpdateAngles();
            UpdateOrientation();
        }

        private void UpdateAngles() {
            _RollAngle += CheckRollKeys() * Increment * Time.deltaTime;
            _PitchAngle += CheckPitchKeys() * Increment * Time.deltaTime;
            _YawAngle += CheckYawKeys() * Increment * Time.deltaTime;
        }

        private float CheckRollKeys() {
            if (Input.GetKeyDown(ROLL_CLOCK)) { return 1; }
            if (Input.GetKeyDown(ROLL_ANTI)) { return -1; }
            return 0;
        }

        private float CheckPitchKeys() {
            if (Input.GetKey(PITCH_CLOCK)) { return 1; }
            if (Input.GetKey(PITCH_ANTI)) { return -1; }
            return 0;
        }

        private float CheckYawKeys() {
            if (Input.GetKey(YAW_CLOCK)) { return 1; }
            if (Input.GetKey(YAW_ANTI)) { return -1; }
            return 0;
        }

        private void CheckResetKey() {
            if (Input.GetKeyDown(RESET)) {
                ResetAllState();
            }
        }

        private void UpdateOrientation() {
            Quaternion orientation = Quaternion.Euler(_PitchAngle, _YawAngle, _RollAngle);
            _Direction = orientation * _Direction;
        }

        private void ResetAllState() {
            ResetDirectionState();
            ResetAngles();
        }

        private void ResetDirectionState() {
            _Direction = Vector3.down;
        }

        private void ResetAngles() {
            _RollAngle = 0;
            _PitchAngle = 0;
            _YawAngle = 0;
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
    }

}

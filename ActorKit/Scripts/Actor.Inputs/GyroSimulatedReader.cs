/*
 * GyroSimulatedReader
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;

using UnityEngine;

namespace Actor.Inputs {

    [Serializable]
    public class AxisConfig {
        public KeyCode Clockwise;
        public KeyCode Anticlockwise;
    }

    [Serializable]
    public class GyroSimConfig {
        public AxisConfig XAxis;
        public AxisConfig YAxis;
        public AxisConfig ZAxis;
    }

    public class GyroSimulatedReader : IGyroReader {
        const float ANGLE_EXTENT = 180f;

        private GyroSimConfig _Config;
        private AxisSim _X;
        private AxisSim _Y;
        private AxisSim _Z;

        public GyroSimulatedReader(GyroSimConfig config) {
            _Config = config;
            _X = new AxisSim(_Config.XAxis);
            _Y = new AxisSim(_Config.YAxis);
            _Z = new AxisSim(_Config.ZAxis);
        }

        public Quaternion TakeOrientationReading() {
            float x, y, z;
            x = _X.TakeReading() * ANGLE_EXTENT;
            y = _Y.TakeReading() * ANGLE_EXTENT;
            z = _Z.TakeReading() * ANGLE_EXTENT;
            return Quaternion.Euler(x, y, z);
        }

        internal class AxisSim {
            const float
                FALLING_RATIO = 0.9f,
                INCREMENT = 0.05f,
                MAXVALUE = 1f,
                MINVALUE = 0.01f;

            enum KeyState {
                Released,
                Pressed
            }

            private float _Value;
            private AxisConfig _Config;
            private KeyState _ClockwiseState;
            private KeyState _AnticlockwiseState;

            internal AxisSim(AxisConfig config) {
                _Value = 0f;
                _Config = config;
                _ClockwiseState = KeyState.Released;
                _AnticlockwiseState = KeyState.Released;
            }

            public float TakeReading() {
                UpdateAxis();
                return _Value;
            }

            private void UpdateAxis() {
                UpdateKeyState();
                if (IsNonePressed()) {
                    _Value = 0.9f * _Value;
                } else if (IsClockwise()) {
                    _Value += INCREMENT;
                } else if (IsAnticlockwise()) {
                    _Value -= INCREMENT;
                }
                if (_Value > MAXVALUE) _Value = MAXVALUE;
                else if (_Value < -MAXVALUE) _Value = -MAXVALUE;
                else if (Mathf.Abs(_Value) < MINVALUE) _Value = 0f;
            }

            private bool IsNonePressed() { return _ClockwiseState == KeyState.Released && _AnticlockwiseState == KeyState.Released; }
            private bool IsClockwise() { return _ClockwiseState == KeyState.Pressed && _AnticlockwiseState != KeyState.Pressed; }
            private bool IsAnticlockwise() { return _AnticlockwiseState == KeyState.Pressed && _ClockwiseState != KeyState.Pressed; }

            private void UpdateKeyState() {
                KeyState? maybeState;
                maybeState = GetState(_Config.Clockwise);
                _ClockwiseState = maybeState != null ? (KeyState)maybeState : _ClockwiseState;

                maybeState = GetState(_Config.Anticlockwise);
                _AnticlockwiseState = maybeState != null ? (KeyState)maybeState : _AnticlockwiseState;
            }

            private KeyState? GetState(KeyCode code) {
                if (Input.GetKeyDown(code)) {
                    return KeyState.Pressed;
                } else if (Input.GetKeyUp(code)) {
                    return KeyState.Released;
                }
                return null;
            }
        }
    }

}

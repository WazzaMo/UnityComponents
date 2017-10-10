/*
 * GyroHardwareReader
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Actor.Inputs {

    public class GyroHardwareReader : IGyroReader {
        private static readonly Vector3 _ViewDirection = Vector3.back;

        public GyroHardwareReader() {
            Input.gyro.enabled = true;
        }

        public Quaternion TakeOrientationReading() {
            return FromDownToForward(GyroToUnity(Input.gyro.attitude));
        }

        private static Quaternion GyroToUnity(Quaternion q) {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }

        private Quaternion FromDownToForward(Quaternion qIn) {
            var adjustment = Quaternion.FromToRotation(Vector3.down, _ViewDirection);
            return adjustment * qIn;
        }

    }

}

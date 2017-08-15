/*
 * ArcadeMotionListenerCannon Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tools.Common;

namespace Actor.Inputs {

    [RequireComponent(typeof(Rigidbody))]
    public class ArcadeMotionListenerCannon : MonoBehaviour {

        private Rigidbody _RigidBody;

        void Start() {
            SetupComponentReferences();
        }

        public void MotionUpdateWithDownDirection(Vector3 DownDirection) {
            CannonMotion(DownDirection);
        }

        private void CannonMotion(Vector3 DownVector) {
            float yawAngle = YawAngle(DownVector.x);
            float pitchAngle = PitchAngle(DownVector.z);
            Quaternion orientation = Quaternion.Euler(pitchAngle, yawAngle, 0);
            _RigidBody.rotation = orientation;
        }

        private float YawAngle(float angularSpeed) {
            return 180 * angularSpeed;
        }

        private float PitchAngle(float angularSpeed) {
            return 45 - 45 * angularSpeed;
        }

        private void SetupComponentReferences() {
            _RigidBody = GetComponent<Rigidbody>();
        }
    }

}
/*
 * ArcadeMotionListenerPlanar Unity Component
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
    public class ArcadeMotionListenerPlanar : MonoBehaviour {
        [SerializeField] private float ForceFactor = 100f;

        private Rigidbody _RigidBody;

        void Start() {
            SetupComponentReferences();
        }

        public void MotionUpdateWithDownDirection(Vector3 DownDirection) {
            PlanarMotion(DownDirection);
        }

        private void PlanarMotion(Vector3 DownVector) {
            Vector3 forceVector = Vector3.zero;
            forceVector.x = ForceFactor * DownVector.x;
            forceVector.z = -1 * ForceFactor * DownVector.z;
            ApplyForce(forceVector);
        }

        private void ApplyForce(Vector3 forceVector) {
            _RigidBody.AddForce(forceVector);
        }

        private void SetupComponentReferences() {
            _RigidBody = GetComponent<Rigidbody>();
        }
    }

}
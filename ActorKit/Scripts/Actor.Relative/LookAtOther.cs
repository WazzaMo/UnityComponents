/*
 * LookAtOther Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Relative {

    public class LookAtOther : MonoBehaviour {
        [SerializeField] GameObject Other = null;

        void Start() {
            Setup();
        }

        void Update() {
            UpdateOrientation();
        }

        public bool IsReady { get { return Other != null; } }

        private Transform _TransformOther;
        private Transform _Transform;
        private Rigidbody _RigidBody;

        private Quaternion NewOrientation() {
            Vector3 relativeDirection = _TransformOther.position - _Transform.position;
            return Quaternion.LookRotation(relativeDirection, Vector3.up);
        }

        private bool IsByRigidBody() {
            return _RigidBody != null;
        }

        private void UpdateOrientation() {
            if (IsReady) {
                if (IsByRigidBody()) {
                    UpdateOrientationByRigidBody(NewOrientation());
                } else {
                    UpdateOrientationByTransform(NewOrientation());
                }
            }
        }

        private void UpdateOrientationByRigidBody(Quaternion orientation) {
            _RigidBody.rotation = orientation;
        }

        private void UpdateOrientationByTransform(Quaternion orientation) {
            _Transform.rotation = orientation;
        }

        private void Setup() {
            _Transform = GetComponent<Transform>();
            _RigidBody = GetComponent<Rigidbody>();
            if (IsReady) {
                _TransformOther = Other.GetComponent<Transform>();
            }
        }
    }

}
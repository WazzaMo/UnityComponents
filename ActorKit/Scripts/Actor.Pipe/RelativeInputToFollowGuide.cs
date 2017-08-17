/*
 * RelativeInputToFollowGuide Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actor.Relative;

namespace Actor.Pipe {

    public class RelativeInputToFollowGuide : MonoBehaviour {
        private Transform _Transform;
        private Rigidbody _RigidBody;
        private IRelativeInputToPosition _RelativeInputToPosition;

        public bool IsReady { get { return _RelativeInputToPosition != null; } }

        public void RelativeInputIn(float RelativeInput) {
            if (IsReady) {
                Vector3 position = _RelativeInputToPosition.GetPoint(RelativeInput);
                MoveTo(position);
            } else {
                Debug.Log("Getting relative input but not ready to move!");
            }
        }

        void Start() {
            Setup();
        }

        private void Setup() {
            _Transform = GetComponent<Transform>();
            _RigidBody = GetComponent<Rigidbody>();
            _RelativeInputToPosition = FindRelativeInputToPositionComponent();
        }

        private IRelativeInputToPosition FindRelativeInputToPositionComponent() {
            IRelativeInputToPosition component = null;
            component = GetComponent<BetweenTwoPoints>();
            return component;
        }

        private void MoveTo(Vector3 NewPos) {
            if (IsToUseRigidBody) {
                MoveByRigidBody(NewPos);
            } else {
                MoveByTransform(NewPos);
            }
        }

        private bool IsToUseRigidBody { get { return _RigidBody != null; } }

        private void MoveByTransform(Vector3 NewPos) {
            _Transform.position = NewPos;
        }

        private void MoveByRigidBody(Vector3 NewPos) {
            _RigidBody.position = NewPos;
        }
    }

}

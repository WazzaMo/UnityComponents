/*
 * FollowGuide Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Actor.Inputs;

namespace Actor.Relative {

    public class FollowGuide : MonoBehaviour {

        [SerializeField] WayPointTrack TrackToFollow;

        private Transform _Transform;
        private Rigidbody _RigidBody;

        void Start() {
            if (IsReady) {
                Setup();
            } else {
                Debug.Log("Needs a reference to a WayPointTrack.");
            }
        }

        public void PositionEvent(float progressPosition) {
            Vector3 NewPos;
            if (IsReady) {
                NewPos = TrackToFollow.GetPointOnTrack(progressPosition);
                MoveTo(NewPos);
            }
        }

        public bool IsReady { get { return TrackToFollow != null; } }

        private void Setup() {
            _Transform = GetComponent<Transform>();
            _RigidBody = GetComponent<Rigidbody>();
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
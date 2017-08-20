/*
 * CircularTrack Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Relative {

    public class CircularTrack : BaseTrack {
        public const float MAX_ANGLE = 360;

        [Tooltip("Radius for the Arc/Circle")]
        [SerializeField] float _Radius = 2f;

        [Tooltip("Degrees")]
        [SerializeField] float _TrackAngle = 180f; //degrees

        [SerializeField] private Quaternion _TrackOrientation = Quaternion.identity;

        private Transform _Transform;


        public float Radius { get { return _Radius; } set { _Radius = value; } }

        public float TrackAngle { get { return _TrackAngle; } set { _TrackAngle = value; } }

        public Quaternion TrackOrientation {
            get { return _TrackOrientation; }
            set { _TrackOrientation = value; }
        }


        public override Vector3 GetPointFromRelativeInput(float relativeInput) {
            return DeterminePointInSpaceFromRelativeInput(relativeInput);
        }

        public static Vector3 GetZeroInputPoint(CircularTrack track) {
            return track.Radius * (track.TrackOrientation * Vector3.forward);
        }

        void Start() {
            Setup();
        }

        private Vector3 DeterminePointInSpaceFromRelativeInput(float relativeInput) {
            Vector3 point;
            float angle;

            angle = GetRotationAngleFromRelative(relativeInput);
            point = RotatePointByAngle(angle);
            return point + _Transform.position;
        }

        private Vector3 RotatePointByAngle(float angle) {
            Quaternion rotation = Quaternion.AngleAxis(angle, GetNormalAxis());
            return rotation * GetZeroPoint();
        }

        private float GetRotationAngleFromRelative(float relativeInput) {
            return _TrackAngle * relativeInput;
        }

        private Vector3 GetZeroPoint() {
            return GetZeroInputPoint(this);// _Radius * (_TrackOrientation * Vector3.forward);
        }

        private Vector3 GetNormalAxis() {
            return _TrackOrientation * Vector3.up;
        }

        private void Setup() {
            _Transform = GetComponent<Transform>();
        }
    }

}

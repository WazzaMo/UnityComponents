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

using Actor.Events;

namespace Actor.Relative {

    public class CircularTrack : BaseTrack {
        public const float MAX_ANGLE = 360;

        [Tooltip("Radius for the Arc/Circle")]
        [SerializeField] float _Radius = 2f;

        [Tooltip("Degrees")]
        [SerializeField] private float _TrackAngle = 180f; //degrees

        [SerializeField] private Quaternion _TrackOrientation = Quaternion.identity;

        [Tooltip("Full range is -1..0..+1")]
        [SerializeField] private bool _FullRange = false;

        private Transform _Transform;


        public float Radius { get { return _Radius; } set { _Radius = value; } }

        public float TrackAngle {
            get { return _FullRange ? _TrackAngle / 2 : _TrackAngle; }
            set { _TrackAngle = _FullRange ? value * 2 : value; }
        }

        public Quaternion TrackOrientation {
            get { return _TrackOrientation; }
            set { _TrackOrientation = value; }
        }

        public bool FullRange {
            get { return _FullRange; }
            set { _FullRange = value; }
        }


        public override Vector3 GetPointFromRelativeInput(float relativeInput) {
            float internalRelative;
            if (_FullRange) {
                internalRelative = RelativeInputEvent.WithinFullRange(relativeInput);
            } else {
                internalRelative = RelativeInputEvent.WithinPositiveRange(relativeInput);
            }
            return DeterminePointInSpaceFromRelativeInput(internalRelative);
        }

        public static Vector3 GetZeroInputPoint(CircularTrack track) {
            if (track._FullRange) {
                return track.Radius * (track.TrackOrientation * Vector3.right);
            } else {
                return track.Radius * (track.TrackOrientation * Vector3.forward);
            }
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
            if (_FullRange) {
                return _TrackAngle * relativeInput / 2;
            } else {
                return _TrackAngle * relativeInput;
            }
        }

        private Vector3 GetZeroPoint() {
            return GetZeroInputPoint(this);
        }

        private Vector3 GetNormalAxis() {
            return _TrackOrientation * Vector3.up;
        }

        private void Setup() {
            _Transform = GetComponent<Transform>();
        }
    }

}

/*
 * CircularTrackEditor Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Actor.Relative {

    [CustomEditor(typeof(CircularTrack))]
    public class CircularTrackEditor : Editor {

        const float SEMI_ALPHA = 0.2f;
        static readonly Color SemiYellow = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, SEMI_ALPHA);
        static readonly Color SemiOrange = new Color(1.0f, 0.4f, 0f, SEMI_ALPHA);

        private CircularTrack _Track;
        private Transform _Transform;
        private float _TrackAngle;
        private float _Radius;

        private void OnSceneGUI() {
            Setup();

            if (IsReady) {
                Orientation = Handles.RotationHandle(Orientation, _Transform.position);
                DrawTrackArc();
                DrawCircle();
            }
        }

        private Quaternion Orientation {
            get { return _Track != null ? _Track.TrackOrientation : Quaternion.identity; }

            set { if (_Track != null) {
                    _Track.TrackOrientation = value;
                }
            }
        }

        private Vector3 Position { get { return IsReady ? _Transform.position : Vector3.zero; } }
        private Vector3 Normal { get { return Orientation * Vector3.up; } }

        private Vector3 Point { get { return IsReady ? CircularTrack.GetZeroInputPoint(_Track) : Vector3.zero; } }

        private void Setup() {
            _Track = target as CircularTrack;
            _Transform = IsReady ? _Track.gameObject.transform : null;
            _TrackAngle = IsReady ? _Track.TrackAngle : 0;
            _Radius = IsReady ? _Track.Radius : 0f;
        }

        private bool IsReady { get { return _Track != null; } }

        private void DrawCircle() {
            Handles.color = Color.white;
            Handles.DrawWireDisc(Position, Normal, _Radius);
        }

        private void DrawTrackArc() {
            Handles.color = SemiYellow;
            Handles.DrawSolidArc(Position, Normal, Point, _TrackAngle, _Radius);
            if (_Track.FullRange) {
                Handles.color = SemiOrange;
                Handles.DrawSolidArc(Position, Normal, Point, - _TrackAngle, _Radius);
            }
        }
    }

}
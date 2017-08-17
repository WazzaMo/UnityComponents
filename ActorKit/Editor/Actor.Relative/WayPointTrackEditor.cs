/*
 * FollowGuide Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Actor.Relative {

    [CustomEditor(typeof(WayPointTrack))]
    public class WayPointTrackEditor : Editor {

        public void OnSceneGUI() {
            WayPointTrack track = target as WayPointTrack;
            if (track != null) {
                Vector3[] points = GetPointsFrom(track);
                Handles.DrawPolyLine(points);
            }
        }

        private Vector3[] GetPointsFrom(WayPointTrack track) {
            WayPoint[] points = track.Points;
            Vector3[] positions = new Vector3[points.Length + 1];
            for(int index = 0; index < points.Length; index++) {
                positions[index] = points[index].transform.position;
            }
            positions[positions.Length - 1] = points[0].transform.position;
            return positions;
        }

    }

}

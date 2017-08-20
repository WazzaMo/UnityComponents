/*
 * AutoWayPointTrackEditor Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


namespace Actor.Relative {

    [CustomEditor(typeof(AutoWayPointTrack))]
    public class AutoWayPointTrackEditor : Editor {

        public void OnSceneGUI() {
            WayPoint[] wayPoints = GetWayPoints();
            if (wayPoints != null && wayPoints.Length > 0) {
                BasePointTrack track = target as BasePointTrack;
                if (track != null) {
                    Vector3[] points = GetPositionsOfPoints(wayPoints);
                    Handles.DrawPolyLine(points);
                }
            }
        }

        private WayPoint[] GetWayPoints() {
            AutoWayPointTrack autoTrack = target as AutoWayPointTrack;
            return autoTrack.GetComponentsInChildren<WayPoint>();
        }

        private Vector3[] GetPositionsOfPoints(WayPoint[] points) {
            Vector3[] positions = new Vector3[points.Length + 1];
            for (int index = 0; index < points.Length; index++) {
                positions[index] = points[index].transform.position;
            }
            positions[positions.Length - 1] = points[0].transform.position;
            return positions;
        }

    }

}

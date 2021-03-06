﻿/*
 * WayPointTrackEditor Unity Component
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

    [CustomEditor(typeof(WayPointTrack))]
    public class WayPointTrackEditor : Editor {

        public void OnSceneGUI() {
            WayPoint[] wayPoints = GetPoints();
            if (wayPoints != null && wayPoints.Length > 0) {
                Vector3[] points = GetPositionsOfPoints(wayPoints);
                Handles.DrawPolyLine(points);
            }
        }

        private WayPoint[] GetPoints() {
            WayPointTrack wayTrack = target as WayPointTrack;
            return wayTrack.Points;
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

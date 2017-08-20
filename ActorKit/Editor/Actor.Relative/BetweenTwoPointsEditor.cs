/*
 * BetweenTwoPointsEditor Unity Component
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

    [CustomEditor(typeof(BetweenTwoPoints))]
    public class BetweenTwoPointsEditor : Editor {
        private WayPoint FirstPoint;
        private WayPoint SecondPoint;

        public void OnSceneGUI() {
            SetupPoints();
            if (FirstPoint != null && SecondPoint != null) {
                Vector3 firstPos = FirstPoint.transform.position;
                Vector3 secondPos = SecondPoint.transform.position;
                Handles.DrawDottedLine(firstPos, secondPos, HandleUtility.GetHandleSize(firstPos));
                //Handles.DrawLine(firstPos, secondPos);
            }
        }

        private void SetupPoints() {
            BetweenTwoPoints betweenPoints = target as BetweenTwoPoints;
            FirstPoint = betweenPoints.TheFirstPoint;
            SecondPoint = betweenPoints.TheSecondPoint;
        }
    }

}

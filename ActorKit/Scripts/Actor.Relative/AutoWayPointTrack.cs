/*
 * AutoWayPointTrack Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Relative {

    public class AutoWayPointTrack : BasePointTrack {
        public class PositionOnTrackEvent : UnityEvent<Vector3> { };

        void Start() {
            Setup();
            if (TrackVisibility) {
                SetupLineRenderer();
            }
        }

        void Update() {
            if (TrackVisibility) {
                UpdatePointsToLineRenderer();
            }
        }

        public new bool IsReady { get { return base.IsReady; } }


        private void Setup() {
            GetAllChildrenWithWayPointComponents();
            if (IsReady) {
                ConfigureWayPointNodes();
            } else {
                Debug.LogWarningFormat(
                    "GameObject {0} needs many child objects with a WayPoint component",
                    name
                );
            }
        }

        private void GetAllChildrenWithWayPointComponents() {
            SetWayPointNodes( GetComponentsInChildren<WayPoint>() );
        }
    }

}
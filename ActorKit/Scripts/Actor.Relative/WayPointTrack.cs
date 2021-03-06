﻿/*
 * WayPointTrack Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Relative {

    public class WayPointTrack : BasePointTrack {

        [SerializeField] private WayPoint[] WayPointNodes = new WayPoint[0];

        public WayPoint[] Points { get { return WayPointNodes; } }

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

        public new bool IsReady { get { return WayPointNodes != null && WayPointNodes.Length > 2 && base.IsReady; } }

        private void Setup() {
            SetWayPointNodes(WayPointNodes);
            if (IsReady) {
                ConfigureWayPointNodes();
            }
        }

   }

}
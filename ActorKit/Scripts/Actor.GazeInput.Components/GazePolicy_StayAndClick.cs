/*
 * GazePolicy_StayAndClick Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Tools.Common;
using Actor.GazeInput;

namespace Actor.GazeInput.Components {
    public class GazePolicy_StayAndClick : MonoBehaviour, IGazeEventPolicy {
        [Tooltip("How many seconds of gaze before click registered?")]
        public float _SecondsLeadingToClick = 1f;

        private GazeData _LastGaze;

        void Start() {
            Setup();
        }

        public void ApplyGesturePolicy(GazeData data, List<GazeData> eventsToBroadcast) {
            if (_LastGaze.GazeTarget != null && data.GazeTarget == null) {
                _LastGaze = data;
            } else if (data.GazeTarget != null && data.GazeHandler != null) {
                LookForContinuedGazeOrTimerExpiry(data, eventsToBroadcast);
            } else {
                UpdateLastGazeToNewData(data);
            }
        }

        private void Setup() {
            _LastGaze = new GazeData() {
                GazeHandler = null,
                GazeTarget = null,
                EventKind = GazeEventKind.NoEvent,
                TimeGazing = 0f
            };
        }

        private void LookForContinuedGazeOrTimerExpiry(GazeData data, List<GazeData> events) {
            if (data.GazeTarget == _LastGaze.GazeTarget ) {
                UpdateContinuedGazeTimer(data, events);
            } else {
                UpdateLastGazeToNewData(data);
            }
        }

        private void UpdateLastGazeToNewData(GazeData data) {
            _LastGaze = data;
        }

        private void UpdateContinuedGazeTimer(GazeData data, List<GazeData> events) {
            GazeData eventData = new GazeData() {
                GazeHandler = data.GazeHandler,
                GazeTarget = data.GazeTarget
            };

            _LastGaze.TimeGazing += Time.deltaTime;
            eventData.TimeGazing = _LastGaze.TimeGazing;

            if (_LastGaze.TimeGazing > _SecondsLeadingToClick) {
                eventData.EventKind = GazeEventKind.OnGazeClick;
                ResetGazeTime();
            } else {
                eventData.EventKind = GazeEventKind.OnGazeStay;
            }
            events.Add(eventData);
        }

        private void ResetGazeTime() {
            _LastGaze.TimeGazing = 0f;
        }
    }
}

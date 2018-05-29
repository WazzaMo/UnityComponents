/*
 * GazePolicy_EnterExit Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;


using UnityEngine;

using Tools.Common;



namespace Actor.GazeInput.Components {

    public class GazePolicy_EnterExit : MonoBehaviour, IGazeEventPolicy {
        private GazeData _LastGazeData;

        void Start() {
            _LastGazeData = default(GazeData);
            _LastGazeData.GazeTarget = null;
        }

        public void ApplyGesturePolicy(GazeData data, List<GazeData> eventsToBroadcast) {
            if (_LastGazeData.GazeTarget == null && data.GazeTarget == null) {
                PolicyDoesNothingWithSameTarget();
            } else if (_LastGazeData.GazeTarget == null && data.GazeTarget != null) {
                var enterData = NewTarget(data, eventsToBroadcast);
                ChangeFocusTo(enterData);
            } else if (_LastGazeData.GazeTarget != null && data.GazeTarget == null) {
                var emptyData = AbandonedTarget(data, eventsToBroadcast);
                ChangeFocusTo(emptyData);
            } else if (data.GazeTarget != _LastGazeData.GazeTarget) {
                var enterData = DifferentTarget(data, eventsToBroadcast);
                ChangeFocusTo(enterData);
            } else if (data.GazeTarget == _LastGazeData.GazeTarget) {
                PolicyDoesNothingWithSameTarget();
            }
        }

        private void ChangeFocusTo(GazeData focus) {
            _LastGazeData = focus;
        }

        private void PolicyDoesNothingWithSameTarget() {}

        private GazeData DifferentTarget(GazeData data, List<GazeData> eventsToBroadcast) {
            var exitData = CreateExitData(_LastGazeData);
            var enterData = CreateEnterData(data);
            eventsToBroadcast.Add(exitData);
            eventsToBroadcast.Add(enterData);
            return enterData;
        }

        private GazeData NewTarget(GazeData data, List<GazeData> eventsToBroadcast) {
            GazeData diffData = CreateEnterData(data);
            eventsToBroadcast.Add(diffData);
            return diffData;
        }

        private GazeData AbandonedTarget(GazeData data, List<GazeData> eventsToBroadcast) {
            var info = CreateExitData(_LastGazeData);
            eventsToBroadcast.Add(info);
            return default(GazeData);
        }

        private GazeData CreateEnterData(GazeData baseData) {
            return new GazeData() {
                GazeTarget = baseData.GazeTarget,
                EventKind = GazeEventKind.OnGazeEnter,
                GazeHandler = baseData.GazeHandler,
                TimeGazing = 0f
            };
        }

        private GazeData CreateExitData(GazeData baseData) {
            return new GazeData() {
                GazeTarget = baseData.GazeTarget,
                EventKind = GazeEventKind.OnGazeExit,
                GazeHandler = baseData.GazeHandler,
                TimeGazing = Time.deltaTime + baseData.TimeGazing
            };
        }
    }

}

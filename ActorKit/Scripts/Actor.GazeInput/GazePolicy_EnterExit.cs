/*
 * GazePolicy_EnterExit Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Tools.Common;



namespace Actor.GazeInput {

    public class GazePolicy_EnterExit : MonoBehaviour, IGazeEventPolicy {
        private GazeData _LastGazeData;

        void Start() {
            _LastGazeData = default(GazeData);
            _LastGazeData.GazeTarget = null;
        }

        public void ApplyGesturePolicy(GazeData data, List<GazeData> eventsToBroadcast) {
            if (_LastGazeData.GazeTarget == null) {
                NewTarget(data, eventsToBroadcast);
            } else if (data.GazeTarget != _LastGazeData.GazeTarget) {
                DifferentTarget(data, eventsToBroadcast);
            } else if (data.GazeTarget == null) {
                AbandonedTarget(data, eventsToBroadcast);
            } else if (data.GazeTarget == _LastGazeData.GazeTarget) {
                PolicyDoesNothingWithSameTarget();
            }
            _LastGazeData = data;
        }

        private void PolicyDoesNothingWithSameTarget() {}

        private void DifferentTarget(GazeData data, List<GazeData> eventsToBroadcast) {
            NewTarget(data, eventsToBroadcast);
            AbandonedTarget(data, eventsToBroadcast);
            Logging.Log<GazePolicy_EnterExit>("Different target looked at!");
        }

        private void NewTarget(GazeData data, List<GazeData> eventsToBroadcast) {
            GazeData diffData = new GazeData() {
                GazeTarget = data.GazeTarget,
                EventKind = GazeEventKind.OnGazeEnter,
                TimeGazing = 0f,
                GazeHandler = data.GazeHandler
            };
            eventsToBroadcast.Add(diffData);
            Logging.Log<GazePolicy_EnterExit>("Now looking at new target {0}", data.GazeTarget.name);
        }

        private void AbandonedTarget(GazeData data, List<GazeData> eventsToBroadcast) {
            GazeData info = new GazeData() {
                GazeTarget = _LastGazeData.GazeTarget,
                EventKind = GazeEventKind.OnGazeExit,
                GazeHandler = _LastGazeData.GazeHandler,
                TimeGazing = Time.deltaTime + _LastGazeData.TimeGazing
            };
            eventsToBroadcast.Add(info);
            Logging.Log<GazePolicy_EnterExit>("Abandoned target {0}", data.GazeTarget.name);
        }

    }

}

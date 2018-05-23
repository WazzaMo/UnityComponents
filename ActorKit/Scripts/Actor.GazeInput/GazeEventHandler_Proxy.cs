/*
 * GazeEventSource Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using Tools.Common;


namespace Actor.GazeInput {

    class GazeEventHandler_Proxy : MonoBehaviour, IGazeEventHandler {
        public UnityEvent _OnGazeEnter { get; private set; }
        public UnityEvent _OnGazeExit { get; private set; }
        public UnityEvent _OnGazeStay { get; private set; }
        public UnityEvent _OnGazeClick { get; private set; }

        public void OnGazeClick(GazeData data) { CallSafe(_OnGazeClick); }

        public void OnGazeContinue(GazeData data) { CallSafe(_OnGazeStay); }

        public void OnGazeEnter(GazeData data) { CallSafe(_OnGazeEnter); }

        public void OnGazeLeave(GazeData data) { CallSafe(_OnGazeExit); }

        private void CallSafe(UnityEvent _event) {
            if (_event != null) {
                _event.Invoke();
            }
        }
    }

}

/*
 * GazeEventSource Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using UnityEngine;
using UnityEngine.Events;

using Tools.Common;


namespace Actor.GazeInput.Components {

    class GazeEventHandler_Proxy : MonoBehaviour, IGazeEventHandler {
        public UnityEvent _OnGazeEnter = null;
        public UnityEvent _OnGazeExit = null;
        public UnityEvent _OnGazeStay = null;
        public UnityEvent _OnGazeClick = null;

        public void OnGazeClick(GazeData data) { CallSafe(_OnGazeClick); }

        public void OnGazeContinue(GazeData data) { CallSafe(_OnGazeStay); }

        public void OnGazeEnter(GazeData data) { CallSafe(_OnGazeEnter); }

        public void OnGazeLeave(GazeData data) { CallSafe(_OnGazeExit); }

        void Start() {
            InitIfNull(ref _OnGazeClick);
            InitIfNull(ref _OnGazeEnter);
            InitIfNull(ref _OnGazeExit);
            InitIfNull(ref _OnGazeStay);
        }

        private void CallSafe(UnityEvent _event) {
            if (_event != null) {
                _event.Invoke();
            }
        }

        private void InitIfNull(ref UnityEvent _event) {
            if (_event == null) {
                _event = new UnityEvent();
            }
        }
    }

}

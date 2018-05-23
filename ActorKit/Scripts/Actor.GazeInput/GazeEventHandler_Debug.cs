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

    class GazeEventHandler_Debug : MonoBehaviour, IGazeEventHandler {

        public void OnGazeClick(GazeData data) {
            Logging.Log<GazeEventHandler_Debug>("OnGazeClick({0})", data.GazeTarget.name);
        }

        public void OnGazeContinue(GazeData data) {
            Logging.Log<GazeEventHandler_Debug>("OnGazeContinue({0})", data.GazeTarget.name);
        }

        public void OnGazeEnter(GazeData data) {
            Logging.Log<GazeEventHandler_Debug>("OnGazeEnter({0})", data.GazeTarget.name);
        }

        public void OnGazeLeave(GazeData data) {
            Logging.Log<GazeEventHandler_Debug>("OnGazeLeave({0})", data.GazeTarget.name);
        }
    }

}

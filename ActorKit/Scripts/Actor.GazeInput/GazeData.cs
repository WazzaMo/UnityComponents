/*
 * GazeData Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Actor.GazeInput {

    public struct GazeData {
        public GameObject GazeTarget;
        public float TimeGazing;
        public IGazeEventHandler GazeHandler;
        public GazeEventKind EventKind;
    }

}

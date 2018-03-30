/*
 * Pulse Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Common {

    using UnityEngine;

    public class Pulse {
        private float _Time;
        private float _LastLerp;

        public float LastLerp { get { return _LastLerp; } }

        public Pulse() {
            _Time = 0f;
        }

        public float GetPulseLerpValue(float pulseDuration) {
            _Time = _Time + Time.deltaTime;
            var progress = 2 * ((_Time / pulseDuration) - Mathf.Round(_Time / pulseDuration));
            _LastLerp = progress > 0 ? progress : -progress;
            return _LastLerp;
        }
    }

}

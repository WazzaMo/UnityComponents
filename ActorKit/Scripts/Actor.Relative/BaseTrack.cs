/*
 * BaseTrack Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Actor.Relative {

    public abstract class BaseTrack : MonoBehaviour {
        public abstract Vector3 GetPointFromRelativeInput(float relativeInput);
    }

}

/*
 * IRelativeInputToPosition Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using UnityEngine;

namespace Actor.Relative {

    public abstract class RelativeInputToPosition : MonoBehaviour {
        public abstract Vector3 GetPointFromRelativeInput(float RelativeInput);
    }

}

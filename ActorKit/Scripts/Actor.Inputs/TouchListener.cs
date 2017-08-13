/*
 * TouchListener Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tools.Common;

namespace Actor.Inputs {

    public class TouchListener : MonoBehaviour {
        void Start() {
        }

        public virtual void TouchEvent(float domainValue) {
            UiDebug.Log("TouchEvent [{0}]", domainValue);
            Debug.LogFormat("TouchEvent [{0}]", domainValue);
        }
    }

}
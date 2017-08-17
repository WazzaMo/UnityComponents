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

        public void TouchEvent(float domainValue) {
            string message = string.Format("GameObject '{0}' Touch Event [{1}]", name, domainValue);
            UiDebug.Log(message);
            Debug.Log(message);
        }
    }

}
/*
 * DeviceMotionListener Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Tools.Common;

namespace Actor.Inputs {


    public class DeviceMotionListener : MonoBehaviour {
        public void MotionUpdateWithDownDirection(Vector3 DownDirection) {
            UiDebug.Log("New downward direction {0}", DownDirection);
        }
    }

}
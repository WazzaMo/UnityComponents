/*
 * GyroEvent Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace Actor.Events {

    [Serializable]
    public class GyroEvent : UnityEvent<Quaternion> {
    }

}
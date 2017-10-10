/*
 * RotationEvent Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Events {

    [Serializable]
    public class RotationEvent : UnityEvent<Quaternion> {}

}

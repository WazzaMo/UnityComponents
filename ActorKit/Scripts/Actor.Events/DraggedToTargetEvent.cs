/*
 * DraggedToTargetEvent Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using UnityEngine;
using UnityEngine.Events;


using Actor.UI;

namespace Actor.Events {

    [System.Serializable]
    public class DraggedToTargetEvent : UnityEvent<DragData> { }

}
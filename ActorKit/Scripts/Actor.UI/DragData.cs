/*
 * DragData Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */



using System;
using UnityEngine;


namespace Actor.UI {

    public struct DragData {
        public DraggableIcon Icon;
        public DragToTarget Target;
        public Vector2 DragPosition;
    }

}

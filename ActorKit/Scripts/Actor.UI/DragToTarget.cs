/*
 * DragToTarget Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tools.Common;

namespace Actor.UI {

    [AddComponentMenu("UI/DragToTarget")]
    [RequireComponent(typeof(RectTransform))]
    public class DragToTarget : MonoBehaviour {
        private RectTransform _RectTransform;

	    void Start () {
            _RectTransform = this.GetComponentOrWarn<RectTransform>();
	    }
	
        public bool IsPointInRectangle(Vector2 screenPos) {
            return RectTransformUtility.RectangleContainsScreenPoint(_RectTransform, screenPos);
        }
    }

}
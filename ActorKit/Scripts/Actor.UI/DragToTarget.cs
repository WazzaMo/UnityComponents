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

using Actor.Events;

namespace Actor.UI {

    [AddComponentMenu("UI/DragToTarget")]
    [RequireComponent(typeof(RectTransform))]
    public class DragToTarget : MonoBehaviour {
        public DraggedToTargetEvent _IconDraggedToTarget;

        [Tooltip("Restart to see behaviour")]
        public bool _SnapToTargetCentre = false;

        private RectTransform _RectTransform;

	    void Start () {
            _RectTransform = this.GetComponentOrWarn<RectTransform>();
            RegisterSnapHandlerIfSnapToTargetEnabled();
	    }
	
        public bool IsPointInRectangle(Vector2 screenPos) {
            return RectTransformUtility.RectangleContainsScreenPoint(_RectTransform, screenPos);
        }

        internal void IconStoppedDraggingOverTarget(DraggableIcon icon) {
            if (_IconDraggedToTarget != null) {
                DragData data = new DragData() {
                    Icon = icon,
                    Target = this,
                    DragPosition = icon.DragPosition
                };
                _IconDraggedToTarget.Invoke(data);
            }
        }

        private void RegisterSnapHandlerIfSnapToTargetEnabled() {
            if (_SnapToTargetCentre ) {
                if (_IconDraggedToTarget != null) {
                    _IconDraggedToTarget.AddListener(SnapToDragHandler);
                }
            }
        }

        private void SnapToDragHandler(DragData data) {
            Logging.Log<DragToTarget>("Icon dragged - trying to snap");
            var pos = _RectTransform.GetWorldPos();
            data.Icon.SetSnapToPosition(pos);
        }
    }

}
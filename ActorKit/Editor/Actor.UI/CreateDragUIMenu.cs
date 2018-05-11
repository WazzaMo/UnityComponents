/*
 * AutoWayPointTrackEditor Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using Tools.Common;

namespace Actor.UI {

    public static class CreateDragUIMenu {
        const int BASE_PRIORITY = 1;

        [MenuItem("GameObject/UI/Drag Origin",false, priority: BASE_PRIORITY + 0)]
        public static void CreateDragOrigin() {
            WhenUiObjectSelected(parent => {
                UiObjectUtil.CreateUiObject<Image, DragOrigin>("Drag Origin", parent, Vector2.zero, (a, b) => {
                    a.color = Color.red;
                    Logging.Warning<DragOrigin>("Need to set the draggable icon prefab");
                }, PositionIsFixed: false);
            });
        }

        [MenuItem("GameObject/UI/Draggable Icon", false, priority: BASE_PRIORITY + 1)]
        public static void CreateDraggableIcon() {
            WhenUiObjectSelected(parent => {
                UiObjectUtil.CreateUiObject<DraggableIcon>("Draggable Icon", parent, Vector2.zero, dIcon => dIcon._PivotPoint = new Vector2(0.5f, 0.5f), PositionIsFixed: false);
            });
        }

        [MenuItem("GameObject/UI/Drag Target", false, priority: BASE_PRIORITY + 2)]
        public static void CreateDragToTarget() {
            WhenUiObjectSelected(parent => {
                UiObjectUtil.CreateUiObject<DragToTarget, Image>(
                    "Drag Target", parent, Vector2.zero,
                    (target, img) => {
                        target._SnapToTargetCentre = true;
                        img.color = new Color(1, 1, 1, 0.5f);
                    },
                    PositionIsFixed: false
                );
            });
        }

        private static void WhenUiObjectSelected(Action<RectTransform> withSelected) {
            RectTransform rectTransform;
            if (IsUiObjectSelected(out rectTransform)) {
                withSelected(rectTransform);
            }
        }

        private static bool IsUiObjectSelected(out RectTransform uiTransform) {
            var uiTransforms = Selection.transforms.Where(trans => trans is RectTransform);
            if (uiTransforms.Any()) {
                RectTransform selected = uiTransforms.First() as RectTransform;
                uiTransform = selected;
                return true;
            }
            uiTransform = null;
            return false;
        }
    }

}

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
        const float SHADE = 47f/256f, ALPHA = 147f/256f;

        private static readonly Sprite _TargetSprite;
        private static readonly Sprite _BackgroundSprite;
        private static readonly Vector2 _CentrePivot;
        private static readonly Color _DefaultBackgroundColor;

        static CreateDragUIMenu() {
            _TargetSprite = Resources.Load<Sprite>("Images/DragTarget");
            _BackgroundSprite = Resources.Load<Sprite>("Images/Background-Bordered");
            _CentrePivot = new Vector2(0.5f, 0.5f);
            _DefaultBackgroundColor = new Color(SHADE, SHADE, SHADE, ALPHA);
        }

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
                UiObjectUtil.CreateUiObject<DraggableIcon>(
                    "Draggable Icon",
                    parent,
                    Vector2.zero,
                    dIcon => dIcon._PivotPoint = _CentrePivot,
                    PositionIsFixed: false
                );
            });
        }

        [MenuItem("GameObject/UI/Drag Target (Target Image)", false, priority: BASE_PRIORITY + 2)]
        public static void CreateDragToTarget_TargetImage() {
            WhenUiObjectSelected(parent => {
                UiObjectUtil.CreateUiObject<DragToTarget, Image>(
                    "Drag Target", parent, Vector2.zero,
                    (target, img) => {
                        target._SnapToTargetCentre = true;
                        img.sprite = _TargetSprite;
                    },
                    PositionIsFixed: false
                );
            });
        }

        [MenuItem("GameObject/UI/Drag Target - Panel", false, priority: BASE_PRIORITY + 3)]
        public static void CreateDragToTarget_Panel() {
            GameObject panel;

            WhenUiObjectSelected(parent => {
                panel= UiObjectUtil.CreateUiObject<Image, DragToTarget>(
                    "Panel (Drag Target)", parent, Vector2.zero,
                    (img, target) => {
                        target._SnapToTargetCentre = true;
                        img.sprite = _BackgroundSprite;
                        img.type = Image.Type.Sliced;
                    },
                    PositionIsFixed: false
                );
                var image = panel.GetComponent<Image>();
                image.color = _DefaultBackgroundColor;
                var rect = panel.GetComponent<RectTransform>();
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
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

/*
 * TouchRegion Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Linq;

using Tools.Common;

namespace Actor.Inputs {

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class TouchRegion : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler, IDragHandler, IPointerExitHandler {
        public enum Direction {
            Vertical,
            Horizontal
        }

        const int
            POINTER_MOUSE = -1,
            POINTER_TOUCH_MIN = 0;

        [SerializeField] private bool EnableMouseToSimulateTouch = false;
        [SerializeField] private Direction TouchDirection = Direction.Horizontal;
        [SerializeField] private TouchRegionEvent TouchEventListeners;
        [SerializeField] private Camera TouchCamera;

        private Image _TouchRegion;
        private RectTransform _TouchRect;
        private Rect _ScreenRect;
        private bool _IsWithinRegion;

        void Start() {
            SetupRegion();
        }

        public bool IsReady {
            get {
                return _TouchRegion != null && _TouchRect != null
                    && TouchEventListeners.GetPersistentEventCount() > 0
                    && TouchCamera != null
                    ;
            }
        }

        private void FireTouchEvents(float domainValue) {
            TouchEventListeners.Invoke(domainValue);
            UiDebug.Log("Fire event {0}", domainValue);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
            _IsWithinRegion = true;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
            _IsWithinRegion = true;
            HandleTouchLikeEvent(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            if (_IsWithinRegion) {
                HandleTouchLikeEvent(eventData);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
            _IsWithinRegion = false;
        }

        private void HandleTouchLikeEvent(PointerEventData eventData) {
            SetupScreenRect();
            FireTouchEvents(TouchDomainValue(eventData));
        }

        private float TouchDomainValue(PointerEventData pointerData) {
            Vector2 screenPos = pointerData.pointerPressRaycast.screenPosition;
            if (TouchDirection == Direction.Horizontal) {
                return (screenPos.x - _ScreenRect.xMin) / _ScreenRect.width;
            }
            if (TouchDirection == Direction.Vertical) {
                return (screenPos.y - _ScreenRect.yMin) / _ScreenRect.height;
            }
            return 0.0f;
        }

        private bool IsTouchLikeEvent(PointerEventData eventData) {
            return eventData.pointerId >= POINTER_TOUCH_MIN
                || (eventData.pointerId == POINTER_MOUSE && EnableMouseToSimulateTouch)
                ;
        }

        private void SetupRegion() {
            _TouchRegion = GetComponent<Image>();
            _TouchRect = GetComponent<RectTransform>();
            _IsWithinRegion = false;
        }

        private Rect GetPixelRect() {
            Canvas canvas = GetComponentInParent<Canvas>();
            Rect pixels = default(Rect);
            if (canvas != null) {
                pixels = RectTransformUtility.PixelAdjustRect(_TouchRect, canvas);
            } else {
                Debug.Log("Couldn't find canvas! ?");
            }
            return pixels;
        }

        private void SetupScreenRect() {
            Rect pixels = GetPixelRect();
            float xLeft = _TouchRect.position.x + pixels.xMin;
            float yBottom = _TouchRect.position.y + pixels.yMin;
            _ScreenRect = new Rect(xLeft, yBottom, pixels.width, pixels.height);
        }

    }

}
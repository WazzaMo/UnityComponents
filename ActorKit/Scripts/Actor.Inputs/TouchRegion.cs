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
        [SerializeField] private TouchListener[] TouchEventListeners = new TouchListener[0];
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
                && TouchEventListeners != null && TouchEventListeners.Length > 0
                && TouchCamera != null
                ;
            }
        }

        private void FireTouchEvents(float domainValue) {
            foreach (var listener in TouchEventListeners) {
                listener.TouchEvent(domainValue);
            }
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
            if (IsTouchLikeEvent(eventData)) {
                float domainValue = ComputeDomainValue(eventData.position);
                FireTouchEvents(domainValue);
            }
        }

        private bool IsTouchLikeEvent(PointerEventData eventData) {
            return eventData.pointerId >= POINTER_TOUCH_MIN
                || (eventData.pointerId == POINTER_MOUSE && EnableMouseToSimulateTouch)
                ;
        }

        private float ComputeDomainValue(Vector2 rectanglePos) {
            float axisValue = 0f;

            if (TouchDirection == Direction.Horizontal) {
                axisValue =(rectanglePos.x - AxisRangeZeroX) / AxisRange;
            } else {
                axisValue = 1 + ((rectanglePos.y - AxisRangeZeroY) / AxisRange);
            }
            return axisValue;
        }

        private float AxisRangeZeroX { get { return _ScreenRect.x; } }
        private float AxisRangeZeroY { get { return _ScreenRect.y; } }

        private float ComputeAxisValue(Vector2 rectanglePos) {
            return TouchDirection == Direction.Horizontal ? rectanglePos.x : rectanglePos.y;
        }

        private float AxisRange {
            get { return TouchDirection == Direction.Horizontal ? _TouchRect.rect.width : _TouchRect.rect.height; }
        }

        private void SetupRegion() {
            _TouchRegion = GetComponent<Image>();
            _TouchRect = GetComponent<RectTransform>();
            if (! IsReady ) {
                UiDebug.Log("GameObject {0} needs UnityEngine.UI Image, RectTransform and values for TouchListeners and the Camera.");
            } else {
                _ScreenRect = ConvertImageRectTransformToRect();
                UiDebug.Log("Touch Region: {0}",_ScreenRect );
            }
            _IsWithinRegion = false;
        }

        private Rect ConvertImageRectTransformToRect() {
            Vector3[] WorldCorners = new Vector3[4];
            Vector2 BtmLeft, TopLeft, TopRight;

            _TouchRect.GetWorldCorners(WorldCorners);
            BtmLeft = WorldCorners[0] ;
            TopLeft = WorldCorners[1] ;
            TopRight = WorldCorners[2] ;

            float width = TopRight.x - TopLeft.x;
            float height = TopLeft.y - BtmLeft.y;
            
            return new Rect(
                TopLeft.x, TopLeft.y,
                width, height
            );
        }

    }

}
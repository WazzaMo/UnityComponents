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
    public class TouchRegion : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler, IDragHandler {
        public enum Direction {
            Vertical,
            Horizontal
        }

        [SerializeField] private Direction TouchDirection = Direction.Horizontal;
        [SerializeField] private TouchListener[] TouchEventListeners = new TouchListener[0];
        [SerializeField] private Camera TouchCamera;

        private Image _TouchRegion;
        private RectTransform _TouchRect;
        private Rect _ScreenRect;

        void Start() {
            SetupRegion();
        }

        void Update() {
            if (IsReady) {
                FireTouchEvents();
                FireMouseEvents();
            } else {
                Debug.LogWarning("Not ready");
            }
        }

        public bool IsReady {
            get {
                return _TouchRegion != null && _TouchRect != null
                && TouchEventListeners != null && TouchEventListeners.Length > 0
                && TouchCamera != null
                ;
            }
        }

        private void FireTouchEvents() {
            //foreach (Touch aTouch in FindRelevantTouches()) {
            //    foreach (var listener in TouchEventListeners) {
            //        listener.TouchEvent(DomainValueFor(aTouch));
            //    }
            //}
        }

        private void FireMouseEvents() {
            float val = DomainValueFor(Input.mousePosition);
            if (val > 0) {
                foreach(var listener in TouchEventListeners) {
                    listener.TouchEvent(val);
                }
            }
        }

        private float DomainValueFor(Vector3 mousePosition) {
            float domainVal = -1f;
            if(EventSystem.current.IsPointerOverGameObject() ) {
                //Debug.LogFormat("Object under mouse at {0}", mousePosition);
                //domainVal = 
            }
            return domainVal;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
            UiDebug.Log("Enter Event on TouchRegion: {0}", ComputeDomainValue(eventData.position));
            Debug.LogFormat("OnPointerEnter: at World {0} Relative {1}\n",
                eventData.pointerCurrentRaycast.worldNormal,
                eventData.position
            );
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
            UiDebug.Log("Click Event on TouchRegion: {0}", ComputeDomainValue(eventData.position));
            Debug.LogFormat("OnPointerClick: at World {0} Relative {1}\n",
                eventData.pointerCurrentRaycast.worldNormal,
                eventData.position
            );
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            UiDebug.Log("Drag Event on TouchRegion: {0}", ComputeDomainValue(eventData.position) );
            Debug.LogFormat("OnDrag: at World {0} Relative {1}\n",
                eventData.pointerCurrentRaycast.worldNormal,
                eventData.position
            );
        }

        private bool PositionInRect(Rect rect, Vector2 position) {
            return
                rect.xMin >= position.x && position.x <= rect.xMax
                && rect.yMin >= position.y && position.y <= rect.yMax
                ;
        }

        private string DescribeLocal() {
            Vector3[] corners = new Vector3[4];
            _TouchRect.GetLocalCorners(corners);
            string description = string.Format(
                "Rect Local Points: BL:{0},{1} TL:{2},{3} TR:{4},{5} BR:{6},{7}",
                corners[0].x, corners[0].y,
                corners[1].x, corners[1].y,
                corners[2].x, corners[2].y,
                corners[3].x, corners[3].y
            );
            return description;
        }

        private string DescribeWorld() {
            Vector3[] corners = new Vector3[4];
            _TouchRect.GetWorldCorners(corners);
            string description = string.Format(
                "Rect Local Points: BL:{0},{1} TL:{2},{3} TR:{4},{5} BR:{6},{7}",
                corners[0].x, corners[0].y,
                corners[1].x, corners[1].y,
                corners[2].x, corners[2].y,
                corners[3].x, corners[3].y
            );
            return description;
        }

        private float DomainValueFor(Touch aTouch) {
            Vector2 rectanglePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_TouchRect, aTouch.position, TouchCamera, out rectanglePos);
            float axisValue = ComputeAxisValue(rectanglePos);
            UiDebug.Log("Touch at local {0} -> {1}", aTouch.position, (axisValue / AxisRange) );
            return axisValue / AxisRange;
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

        private IEnumerable<Touch> FindRelevantTouches() {
            return Input.touches.Where(tch =>
               tch.phase != TouchPhase.Ended
               && tch.phase != TouchPhase.Canceled
               && IsInRegion(tch)
            );
        }

        private bool IsInRegion(Touch touch) {
            bool result = _ScreenRect.Contains(touch.position);
            UiDebug.Log("touch in Region? {0}", result);
            return result;
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
            Debug.LogWarningFormat("Region Info:\n"
                + "- Local {0} \n"
                + "- World {1} \n",
                DescribeLocal(),
                _ScreenRect
                //DescribeWorld()
            );
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
            //float x, y;
            //x = _TouchRect.position.x + _TouchRect.rect.x;
            //y = _TouchRect.position.y + _TouchRect.rect.y;

            //Vector3 worldBottomLeft = new Vector3(x, y);
            //Vector3 worldTopRight = new Vector3(_TouchRect.rect.width + x, _TouchRect.rect.height + y);
            //Vector3 screenBottomLeft = TouchCamera.WorldToScreenPoint(worldBottomLeft);
            //Vector3 screenTopRight = TouchCamera.WorldToScreenPoint(worldTopRight);
            //float width, height;
            //width = screenTopRight.x - screenBottomLeft.x;
            //height = screenTopRight.y - screenBottomLeft.y;
            //return new Rect(screenBottomLeft.x, screenBottomLeft.y, width, height);
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

namespace Actor.Inputs {

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class TouchRegion : MonoBehaviour {
        public enum Direction {
            Vertical,
            Horizontal
        }

        [SerializeField] private Direction TouchDirection = Direction.Horizontal;
        [SerializeField] private TouchListener[] TouchEventListeners = new TouchListener[0];
        [SerializeField] private Camera TouchCamera;

        private Image _TouchRegion;
        private RectTransform _TouchRect;

        void Start() {
            SetupRegion();
        }

        void Update() {
            if (IsReady) {
                foreach(Touch aTouch in FindRelevantTouches()) {
                    foreach(var listener in TouchEventListeners) {
                        listener.TouchEvent(DomainValueFor(aTouch));
                    }
                }
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

        private float DomainValueFor(Touch aTouch) {
            Vector2 rectanglePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_TouchRect, aTouch.position, TouchCamera, out rectanglePos);
            float axisValue = TouchDirection == Direction.Horizontal ? rectanglePos.x : rectanglePos.y;
            float axisRange = TouchDirection == Direction.Horizontal ? _TouchRect.rect.width : _TouchRect.rect.height;
            return axisValue / axisRange;
        }

        private IEnumerable<Touch> FindRelevantTouches() {
            return Input.touches.Where(tch =>
               tch.phase != TouchPhase.Ended
               && tch.phase != TouchPhase.Canceled
               && IsInRegion(tch)
            );
        }

        private bool IsInRegion(Touch touch) {
            return RectTransformUtility.RectangleContainsScreenPoint(_TouchRect, touch.position, TouchCamera);
        }

        private void SetupRegion() {
            _TouchRegion = GetComponent<Image>();
            _TouchRect = GetComponent<RectTransform>();
            if (! IsReady ) {
                Debug.LogWarningFormat("GameObject {0} needs UnityEngine.UI Image, RectTransform and values for TouchListeners and the Camera.");
            }
        }
    }

}
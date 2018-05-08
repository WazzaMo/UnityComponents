/*
 * DraggableIcon Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Tools.Common;

namespace Actor.UI {

    [AddComponentMenu("UI/DraggableIcon")]
    [RequireComponent(typeof(RectTransform))]
    public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public Texture _Image;

        private RectTransform _RectTransform;
        private RawImage _RawImage;
        private DragOrigin _Origin;
        private RectTransform _ReferenceRectangle;
        private bool _IsReady;
        private bool _IsDragging;

        public void SetOrigin(DragOrigin origin) {
            _Origin = origin;
            SetupReferenceRectangle();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _IsDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData) {
            _IsDragging = false;
        }

        public void OnDrag(PointerEventData eventData) {
            if (_IsReady) {
                MoveToPointer(eventData);
            }
        }

        void Start() {
            Setup();
            CheckEditorParams();
        }

        void Update() {
            if (_IsReady && IsOriginAndReferenceSet()) {
                if (! _IsDragging) {
                    _RectTransform.position = _Origin.OriginTransform.position;
                }
            }
            if (_IsReady && _Origin != null) {
                Logging.Warning("{0}: {1} did not call SetOrigin()!", gameObject.name, TypeUtil.NameOf<DragOrigin>());
            }
        }

        private void MoveToPointer(PointerEventData pointerData) {
            Vector3 dragPosition;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_ReferenceRectangle, pointerData.position, Camera.main, out dragPosition) ) {
                _RectTransform.position = dragPosition;
            }
        }

        private void Setup() {
            _RectTransform = this.GetComponent<RectTransform>();
            _RawImage = gameObject.GetOrAddComponent<RawImage>();
            _RawImage.texture = _Image;
            _Origin = null;
            _IsReady = false;
            _IsDragging = false;
        }

        private bool IsOriginAndReferenceSet() {
            bool isSet = _Origin != null && _ReferenceRectangle != null;
            return isSet;
        }

        private void SetupReferenceRectangle() {
            Canvas canvas = _Origin.GetFirstMatchingComponentInParentsOrWarn<Canvas>();
            _RectTransform.SetParent(canvas.transform);
            if (canvas != null) {
                _ReferenceRectangle = canvas.GetComponent<RectTransform>();
            }
        }

        private void CheckEditorParams() {
            _IsReady = this.AreAllEditorValuesConfigured(_Image);
        }
    }

}
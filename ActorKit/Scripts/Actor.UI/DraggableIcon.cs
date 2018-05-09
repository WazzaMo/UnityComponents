/*
 * DraggableIcon Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Tools.Common;

namespace Actor.UI {

    [AddComponentMenu("UI/DraggableIcon")]
    [RequireComponent(typeof(RectTransform))]
    public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public Texture _Image;
        public Vector2 _PivotPoint;

        private RectTransform _RectTransform;
        private RawImage _RawImage;
        private DragOrigin _Origin = null;
        private RectTransform _ReferenceRectangle = null;
        private Canvas _ReferenceCanvas = null;
        private bool _IsReady = false;
        private bool _IsDragging = false;
        private List<DragToTarget> _Targets = null;
        private Vector2 _OriginHomePosition;
        private Vector2 _IconHomePosition;

        private Func<Vector2> _HomeSelector;


        public void OnBeginDrag(PointerEventData eventData) {
            _IsDragging = true;
            Logging.Log<DraggableIcon>("drag start {0}", _IsDragging);
        }

        public void OnEndDrag(PointerEventData eventData) {
            DragToTarget target;

            _IsDragging = false;
            if (IsInTarget(eventData.position, out target)) {
                _IconHomePosition = eventData.position;
                _HomeSelector = () => _IconHomePosition;
            } else {
                _HomeSelector = () => _OriginHomePosition;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            Logging.Log<DraggableIcon>("dragging {0} - is read: {1}", _IsDragging, _IsReady);

            if (_IsReady) {
                MoveToPointer(eventData);
            }
        }

        public void SetOrigin(DragOrigin origin) {
            _Origin = origin;
            if (_Origin == null) {
                Logging.Warning<DraggableIcon>("Given NULL origin!!");
            } else {
                SetupReferenceRectangle();
                _OriginHomePosition = GetCanvasPosForOrigin();
                _HomeSelector = () => _OriginHomePosition;
            }
        }

        void Start() {
            Setup();
            MakeChildOfCanvas();
            CheckEditorParams();
            SetupListOfAllSceneTargets();
        }

        void Update() {
            if (_IsReady && IsOriginAndReferenceSet()) {
                if (! _IsDragging ) {
                    MoveToOrigin();
                }
            } else {
                Logging.Log<DraggableIcon>("IsReady = {0}  IsOriginAndRefSet() = {1}", _IsReady, IsOriginAndReferenceSet());
            }
        }

        private void MoveToPointer(PointerEventData pointerData) {
            _RectTransform.position = pointerData.position;
        }

        private void MoveToOrigin() {
            _RectTransform.position = _HomeSelector();
        }

        private Vector2 GetCanvasPosForOrigin() {
            Vector2 pos =  _Origin.OriginTransform.TransformPoint(Vector3.zero);
            Vector2 adjust = new Vector2(0, - _Origin.OriginTransform.rect.height / 2);
            return pos + adjust;
        }

        private bool IsOriginAndReferenceSet() {
            bool isSet = _Origin != null && _ReferenceRectangle != null;
            return isSet;
        }

        private void Setup() {
            EnsureRectTransformResolved();
            _RawImage = gameObject.GetOrAddComponent<RawImage>();
            _RawImage.texture = _Image;
            _RectTransform.pivot = _PivotPoint;
        }

        private void SetupListOfAllSceneTargets() {
            DragToTarget[] array = GameObject.FindObjectsOfType<DragToTarget>();

            if (array == null) {
                Logging.Warning<DraggableIcon>("Cannot find any {0} instances so dragging won't work properly!  Add {0} to a Panel in your UI.", TypeUtil.NameOf<DragToTarget>());
                _Targets = null;
            } else {
                _Targets = array.ToList();
            }
        }

        private bool HaveTargets() {
            return _Targets != null;
        }

        private bool IsInTarget(Vector2 pos, out DragToTarget detectedTarget) {
            DragToTarget theTarget = null;
            if (HaveTargets()) {
                _Targets.ForEach(target => {
                    if (target.IsPointInRectangle(pos)) {
                        theTarget = target;
                    }
                });
            }
            detectedTarget = theTarget;
            return theTarget != null;
        }

        private void EnsureRectTransformResolved() {
            if (_RectTransform == null) {
                _RectTransform = gameObject.GetOrAddComponent<RectTransform>();
            }
        }

        private void MakeChildOfCanvas() {
            EnsureRectTransformResolved();
            if ( IsOriginAndReferenceSet()) {
                _RectTransform.SetParent(_ReferenceRectangle);
            } else {
                Logging.Warning<DraggableIcon>("Unable to set parent of draggable to the Canvas.");
            }
        }

        private bool IsParentSet() {
            return _RectTransform != null && _RectTransform.parent != null;
        }

        private void SetupReferenceRectangle() {
            _ReferenceCanvas = _Origin.GetFirstMatchingComponentInParentsOrWarn<Canvas>();
            if (_ReferenceCanvas != null) {
                _ReferenceRectangle = _ReferenceCanvas.GetComponent<RectTransform>();
                if (!IsParentSet()) {
                    MakeChildOfCanvas();
                }
            } else {
                Logging.Warning<DraggableIcon>("Could not get Canvas parent from Origin");
            }
        }

        private void CheckEditorParams() {
            _IsReady = this.AreAllEditorValuesConfigured(_Image);
        }
    }

}
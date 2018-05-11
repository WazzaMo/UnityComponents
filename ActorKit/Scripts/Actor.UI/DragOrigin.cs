/*
 * DragOrigin Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Tools.Common;


namespace Actor.UI {

    [AddComponentMenu("UI/DragOrigin")]
    [RequireComponent(typeof(RectTransform))]
    public class DragOrigin : MonoBehaviour {
        public DraggableIcon _PrefabIcon = null;

        private Image _Image;
        private RectTransform _RectTransform;
        private bool _IsReady;
        private DraggableIcon _Icon;

        internal RectTransform OriginTransform { get { return _RectTransform; } }

	    void Start () {
            Setup();
            CheckEditorParameters();
            ResetDraggableIcon();
	    }

        public void ResetDraggableIcon() {
            if (_IsReady) {
                SpawnIcon();
            }
        }

        private void Setup() {
            _Image = gameObject.GetOrAddComponent<Image>();
            _Image.color = Color.black;
            _RectTransform = GetComponent<RectTransform>();
        }

        private void CheckEditorParameters() {
            _IsReady = this.AreAllEditorValuesConfigured(_PrefabIcon);
        }

        private void SpawnIcon() {
            _Icon = GameObject.Instantiate<DraggableIcon>(_PrefabIcon);
            _Icon.SetOrigin(this);
        }
    }

}
/*
 * ReplaceIconWithCharacter - Example Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Tools.Common;

using Actor.UI;
using Actor.Events;

namespace ActorKitExample.Drag_n_DropUi {

    public class ReplaceIconWithCharacter : MonoBehaviour {
        public Texture _CharacterImage = null;

        private bool _IsReady = false;
        private DraggableIcon _IconOnThisPrefab;

        public void OnDragToTarget(DragData data) {
            if (_IsReady) {
                if (IsDraggedIconTheSameAsThisIcon(data)) {
                    CreateFullCharacterAndMoveIconBackToOrigin(data);
                }
            } else {
                Logging.Log<ReplaceIconWithCharacter>("Not configured so could not handle end of drag");
            }
        }

	    void Start () {
            _IconOnThisPrefab = this.GetComponentOrWarn<DraggableIcon>();
            CheckEditorParams();
            RegisterDragHandler();
	    }

        private void CheckEditorParams() {
            _IsReady = this.AreAllEditorValuesConfigured(_CharacterImage);
        }

        private void RegisterDragHandler() {
            DragToTarget target = GameObject.FindObjectOfType<DragToTarget>();
            if (target != null) {
                target._IconDraggedToTarget.AddListener(OnDragToTarget);
            }
        }

        private bool IsDraggedIconTheSameAsThisIcon(DragData data) {
            return data.Icon == _IconOnThisPrefab;
        }

        private void CreateFullCharacterAndMoveIconBackToOrigin(DragData data) {
            RectTransform targetTransform = data.Target.GetComponent<RectTransform>();
            UiObjectUtil.CreateUiObject<RawImage>("Character", targetTransform, data.DragPosition, image => {
                image.texture = _CharacterImage;
            });
            data.Icon.MoveBackToOrigin();
        }
    }

}
/*
 * EventPipeRelativeInputToPosition Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actor.Relative;
using Actor.Events;

namespace Actor.Pipe {

    public class EventPipeRelativeInputToPosition : MonoBehaviour {
        [SerializeField] private MovePositionEvent OnPositionChange;
        [SerializeField] private RelativeInputToPosition _RelativeInputToPosition;

        public bool IsReady { get {
                return _RelativeInputToPosition != null
                    && OnPositionChange != null;
            }
        }


        public void RelativeInputIn(float RelativeInput) {
            if (IsReady) {
                Vector3 position = _RelativeInputToPosition.GetPointFromRelativeInput(RelativeInput);
                OnPositionChange.Invoke(position);
            } else {
                Debug.Log("Getting relative input but not ready to move!");
            }
        }
    }

}

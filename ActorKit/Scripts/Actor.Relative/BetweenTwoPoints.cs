/*
 * BetweenTwoPoints Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actor.Events;

namespace Actor.Relative {

    public class BetweenTwoPoints : BaseTrack {
        [Header("Points: Left to Right")]
        [SerializeField] private WayPoint FirstPoint;
        [SerializeField] private WayPoint SecondPoint;

        [Header("Relative Range: -1 or 0 based")]
        [Tooltip("When checked, -1 is left, 0 centre, +1 right")]
        [SerializeField] private bool IsNegativeToPositive = false;

        public bool IsReady { get { return FirstPoint != null && SecondPoint != null; } }

        public WayPoint TheFirstPoint {
            get { return FirstPoint; }
            set { FirstPoint = value; }
        }

        public WayPoint TheSecondPoint {
            get { return SecondPoint; }
            set { SecondPoint = value; }
        }

        public override Vector3 GetPointFromRelativeInput(float progressPosition) {
            progressPosition = MapToRange(progressPosition);
            if (IsReady) {
                Vector3 value = Vector3.Lerp(FirstPoint.Position, SecondPoint.Position, progressPosition);
                return value;
            } else {
                Debug.Log("Not ready!");
                return Vector3.zero;
            }
        }

        private float MapToRange(float value) {
            if (IsNegativeToPositive) {
                return 0.5f + (value / 2f);
            } else {
                return RelativeInputEvent.WithinPositiveRange(value);
            }
        }
    }

}

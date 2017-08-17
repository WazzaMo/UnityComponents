/*
 * BetweenTwoPoints Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Relative {

    public class BetweenTwoPoints : MonoBehaviour, IRelativeInputToPosition {
        [SerializeField] private WayPoint FirstPoint;
        [SerializeField] private WayPoint SecondPoint;

        public bool IsReady { get { return FirstPoint != null && SecondPoint != null; } }

        public Vector3 GetPoint(float PortionFromFirstToSecond) {
            if (IsReady) {
                Vector3 value = Vector3.Lerp(FirstPoint.Position, SecondPoint.Position, PortionFromFirstToSecond);
                return value;
            } else {
                Debug.Log("Not ready!");
                return Vector3.zero;
            }
        }
    }

}

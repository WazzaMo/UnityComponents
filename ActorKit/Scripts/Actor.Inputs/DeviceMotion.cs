/*
 * DeviceMotion Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

/*
* Definition for Pitch, Yaw and Roll from Wikipedia https://en.wikipedia.org/wiki/Aircraft_principal_axes
* Where change in Pitch makes you look up or down, change in yaw makes you look in a different
* direction relative to the ground and Roll makes you lose balance and end up on your side.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Tools.Common;

namespace Actor.Inputs {

    public class DeviceMotion : MonoBehaviour {
        [SerializeField] private DeviceMotionEvent MotionListeners;
        [SerializeField] private Vector3 SensitivityFactor = Vector3.one;


        void Start() {
        }

        void Update() {
            if ( HasListeners ){
                Vector3 Direction = GetDirectionTowardGravity();
                NotifyListeners(Direction);
            }
        }

        private void NotifyListeners(Vector3 Direction) {
            MotionListeners.Invoke(Direction);
        }

        private bool HasListeners {
            get { return MotionListeners.GetPersistentEventCount() > 0; }
        }

        private Vector3 GetDirectionTowardGravity() {
            AccelerationEvent accelEvent;
            Vector3 sumRotations = Vector3.zero;
            Vector3 direction;

            for (int index = 0; index < Input.accelerationEventCount; index++) {
                accelEvent = Input.accelerationEvents[index];
                sumRotations += accelEvent.deltaTime * accelEvent.acceleration;
            }
            direction.y = sumRotations.y * SensitivityFactor.y;
            direction.x = sumRotations.x * SensitivityFactor.x;
            direction.z = sumRotations.z * SensitivityFactor.z;
            direction.Normalize();
            return direction;
        }

    }

}
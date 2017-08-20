/*
 * DeviceMotionToRelativeInput Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actor.Events;

namespace Actor.Inputs {

    internal class DeviceMotionToRelativeInput  {
        const float ANGLE_MAX = 180;

        private RelativeInputEvent _Roll;
        private RelativeInputEvent _Pitch;
        private RelativeInputEvent _Yaw;

        internal DeviceMotionToRelativeInput(
            RelativeInputEvent Roll,
            RelativeInputEvent Pitch,
            RelativeInputEvent Yaw
        ) {
            _Roll = Roll;
            _Pitch = Pitch;
            _Yaw = Yaw;
        }

        internal void NotifyDirection(Vector3 towardGravity) {
            if (HasRollHandler) {
                HandleRoll(towardGravity);
            }
            if (HasPitchHandler) {
                HandlePitch(towardGravity);
            }
            if (HasYawHandler) {
                HandleYaw(towardGravity);
            }
        }

        private void HandleRoll(Vector3 towardGravity) {
            float angle = Vector3.SignedAngle(Vector3.down, towardGravity, Vector3.forward);
            float relativeAmount = angle / ANGLE_MAX;
            _Roll.Invoke(RelativeInputEvent.WithinFullRange(relativeAmount));
        }

        private void HandlePitch(Vector3 towardGravity) {
            float angle = Vector3.SignedAngle(Vector3.forward, towardGravity, Vector3.right);
            float relativeAmount = angle / ANGLE_MAX;
            _Pitch.Invoke(RelativeInputEvent.WithinFullRange(relativeAmount));
        }

        private void HandleYaw(Vector3 towardGravity) {
            float angle = Vector3.SignedAngle(Vector3.down, towardGravity, Vector3.up);
            float relativeAmount = angle / ANGLE_MAX;
            _Roll.Invoke(RelativeInputEvent.WithinFullRange(relativeAmount));
        }

        private bool HasRollHandler { get { return _Roll != null && _Roll.GetPersistentEventCount() > 0; } }
        private bool HasPitchHandler { get { return _Pitch != null && _Pitch.GetPersistentEventCount() > 0; } }
        private bool HasYawHandler { get { return _Yaw != null && _Yaw.GetPersistentEventCount() > 0; } }
    }

}

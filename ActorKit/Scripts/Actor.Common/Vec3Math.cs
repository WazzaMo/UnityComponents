/*
 * Vec3Math Unity Tool Class
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;


using UnityEngine;

namespace Actor.Common {

    public static class Vec3Math {

        public static Vector3 DirectionToTargetVertical(Transform HunterTransform, Vector3 Target) {
            var direction = DirectionToTarget(HunterTransform, Target);
            direction.y = 0;
            return direction;
        }

        public static Vector3 DirectionToTarget(Transform HunterTransform, Vector3 Target) {
            Vector3 directionToTarget = (Target - HunterTransform.position).normalized;
            Vector3 currentDireciton = HunterTransform.forward;
            return directionToTarget - currentDireciton;
        }

        public static float YawRotationNeededToFaceTarget(Transform HunterTransform, Vector3 Target) {
            Quaternion orientation = Quaternion.identity;
            Vector3 directionVec = DirectionToTargetVertical(HunterTransform, Target);
            Vector3 forwardXZPlane = HunterTransform.forward;
            forwardXZPlane.y = 0;
            orientation.SetFromToRotation(forwardXZPlane, directionVec);
            return orientation.eulerAngles.y; //Vector3.Angle(forwardXZPlane, directionVec);
        }

    }

}

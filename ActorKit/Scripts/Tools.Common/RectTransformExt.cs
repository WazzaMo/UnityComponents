/*
 * RectTransformExt Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */



using System;

using UnityEngine;

namespace Tools.Common {

    public static class RectTransformExt {

        public static Vector2 GetWorldPos(this RectTransform origin) {
            Vector2 pos = origin.TransformPoint(Vector3.zero);
            return pos;
        }

        public static Vector2 GetWorldPosForRelativePos(this RectTransform origin, Vector2 relative) {
            Vector2 pos = origin.TransformPoint(Vector3.zero);
            return pos + relative;
        }

    }

}

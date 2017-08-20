using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

namespace Actor.Events {

    /// <summary>
    /// A RelativeInputEvent passes a value between 0 and 1 from a relative
    /// input source to an input consumer.  The consumer will determine what
    /// the relative input means - say a distance between two points or
    /// progress along a way point track.
    /// </summary>
    /// 
    [Serializable]
    public class RelativeInputEvent : UnityEvent<float> {
        public const float
            MAX_LIMIT = 1.0f,
            MIN_LIMIT = -1.0f;

        public static float WithinFullRange(float value) {
            if (value >= MIN_LIMIT && value <= MAX_LIMIT) {
                return value;
            } else if (value < MIN_LIMIT) {
                return MIN_LIMIT;
            } else {
                return MAX_LIMIT;
            }
        }

        public static float WithinPositiveRange(float value) {
            if (value >= 0 && value <= MAX_LIMIT) {
                return value;
            } else if (value < 0) {
                return 0;
            } else {
                return MAX_LIMIT;
            }
        }
    }

}

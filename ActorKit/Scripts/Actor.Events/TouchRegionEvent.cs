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
    public class RelativeInputEvent : UnityEvent<float> {}

}

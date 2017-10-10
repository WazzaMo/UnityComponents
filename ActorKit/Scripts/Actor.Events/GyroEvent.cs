using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace Actor.Events {

    [Serializable]
    public class GyroEvent : UnityEvent<Quaternion> {
    }

}
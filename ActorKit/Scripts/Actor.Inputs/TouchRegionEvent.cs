﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

namespace Actor.Inputs {

    [Serializable]
    public class TouchRegionEvent : UnityEvent<float> {
    }

}

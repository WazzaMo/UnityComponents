/*
 * GazeEvent Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

using Actor.GazeInput;

namespace Actor.Events {

    public class GazeEvent : UnityEvent<GazeData> { }

}

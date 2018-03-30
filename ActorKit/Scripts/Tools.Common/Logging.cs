/*
 * Logging Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Common {

    using UnityEngine;

    public static class Logging {
        const string PREFIX = "UNITY >: ";

        public static void Log(string format, params object[] objects) {
            Debug.LogFormat(PREFIX + format, objects);
        }

        public static void Warning(string format, params object[] objects) {
            Debug.LogWarningFormat(PREFIX + format, objects);
        }
    }
}

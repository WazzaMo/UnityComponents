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

        public static void Log<Tsource>(string format, params object[] objects) {
            Log(Prefix<Tsource>() + format, objects);
        }

        public static void Warning<Tsource>(string format, params object[] objects) {
            Warning(Prefix<Tsource>() + format, objects);
        }

        public static void Log(Type Tsource, string format, params object[] objects) {
            Log(Prefix(Tsource) + format, objects);
        }

        public static void Warning(Type TSource, string format, params object[] objects) {
            Warning(Prefix(TSource) + format, objects);
        }


        private static string Prefix<Tsource>() {
            return Prefix(typeof(Tsource));
        }

        private static string Prefix(Type t) {
            string prefix = string.Format("{0}: ", t.Name);
            return prefix;
        }
    }
}

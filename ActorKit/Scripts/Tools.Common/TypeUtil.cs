/*
 * Type Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;


namespace Tools.Common {

    public static class TypeUtil {
        public static string NameOf<T>() {
            return typeof(T).Name;
        }

        public static string TypeName(this object thing) {
            return thing.GetType().Name;
        }
    }

}

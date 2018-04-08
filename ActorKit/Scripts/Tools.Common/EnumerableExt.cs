/*
 * EnumerableExt Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Common {

    public static class EnumerableExt {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> Do) {
            var enumerator = collection.GetEnumerator();
            if (Do != null) {
                while(enumerator != null && enumerator.MoveNext() && enumerator.Current != null) {
                    Do(enumerator.Current);
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> Do) {
            int index = 0;
            var enumerator = collection.GetEnumerator();
            while (enumerator != null && enumerator.MoveNext()) {
                Do(enumerator.Current, index);
                index++;
            }
        }

        public static void ForEachWhile<T>(this IEnumerable<T> collection, Func<T, bool> WhileCondition, Action<T> Do) {
            var enumerator = collection.GetEnumerator();

            while (enumerator != null && enumerator.MoveNext() && WhileCondition(enumerator.Current)) {
                Do(enumerator.Current);
            }
        }
    }

}

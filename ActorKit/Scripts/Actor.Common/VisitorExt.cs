/*
 * VisitorExt Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Actor.Common {

    public static class VisitorExt {

        public static void OnEach<T>(this IEnumerable<T> collection, Action<T> Do) {
            var item = collection.GetEnumerator();
            while(item.MoveNext()) {
                Do(item.Current);
            }
        }

    }

}

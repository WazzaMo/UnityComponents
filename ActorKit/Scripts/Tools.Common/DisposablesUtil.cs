/*
 * DisposablesUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Linq;

namespace Tools.Common {

    public static class DisposablesUtil {

        public static void DisposeAll(params IDisposable[] disposables) {
            disposables.ForEach(disposableSingle => {
                if (disposableSingle != null) {
                    disposableSingle.Dispose();
                }
            });
        }

    }

}

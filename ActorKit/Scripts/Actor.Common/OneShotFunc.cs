/*
 * OneShotFunc Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Actor.Common {

    public class OneShotFunc<Tretval> {
        private Func<Tretval> _Func;
        private Func<Tretval> _Later;

        public OneShotFunc(Func<Tretval> firstCall, Func<Tretval> laterCall = null) {
            _Func = firstCall;
            _Later = laterCall;
        }

        public Tretval Call() {
            Tretval value;
            if (_Func != null) {
                value = _Func();
                _Func = _Later;
            } else {
                value = default(Tretval);
            }
            return value;
        }
    }

}

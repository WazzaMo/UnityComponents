/*
 * OneShotAction Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Actor.Common {

    public class OneShotAction {
        private Action _Action;
        private Action _Later;

        public OneShotAction(Action firstCall, Action laterCall = null) {
            _Action = firstCall;
            _Later = laterCall;
        }

        public void Call() {
            if (_Action != null) {
                _Action();
                _Action = _Later;
            }
        }
    }

}

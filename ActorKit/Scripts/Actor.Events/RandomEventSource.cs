/*
 * RandomEventSource Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub account WazzaMo
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Events {

    public class RandomEventSource : MonoBehaviour {
        [SerializeField]
        private UnityEvent Trigger;
        [SerializeField]
        private float MaxTimeToTrigger;

        private float _NextTriggerTime = 0f;

        void Start() { // Called by Unity
            SetRandomTrigger();
        }

        void Update() { // Called by Unity every screen update
            UpdateTimerValue();
        }

        private void UpdateTimerValue() {
            if (_NextTriggerTime <= 0f) {
                TriggerAndReset();
            } else {
                _NextTriggerTime -= Time.deltaTime;
            }
        }

        private void TriggerAndReset() {
            SetRandomTrigger();
            Trigger.Invoke();
        }

        private void SetRandomTrigger() {
            _NextTriggerTime = Random.value* MaxTimeToTrigger;
        }
    }

}
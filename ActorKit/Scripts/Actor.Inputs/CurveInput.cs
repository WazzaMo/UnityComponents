/*
 * CurveInput Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Actor.Events;

namespace Actor.Inputs {

    public class CurveInput : MonoBehaviour {
        [SerializeField] AnimationCurve Curve;
        [SerializeField] float TimeFactor = 1.0f;
        [SerializeField] RelativeInputEvent InputConsumers;
        [SerializeField] bool AutoLoop = true;

        public bool IsReady { get { return Curve != null && Curve.keys.Length > 0; } }

        private float _TimeSeconds;

        void Start() {
            _TimeSeconds = 0f;
        }

        void Update() {
            if (IsReady) {
                AssessTimeAndProduceInput();
            } else {
                Debug.LogFormat("{0} not ready to follow Curve.", name);
            }
        }

        private void AssessTimeAndProduceInput() {
            bool isDone = IsDone();
            UpdateTime(isDone);
            if (!isDone) {
                NotifyNewInput();
            }
        }

        private bool IsDone() {
            return _TimeSeconds > MaxTime();
        }

        private void UpdateTime(bool isDone) {
            if (isDone && AutoLoop) {
                _TimeSeconds = 0;
            } else {
                _TimeSeconds += TimeFactor * Time.deltaTime;
            }
        }

        private void NotifyNewInput() {
            float value = RelativeInputEvent.WithinFullRange( Curve.Evaluate(_TimeSeconds) );
            InputConsumers.Invoke(value);
        }

        private float MaxTime() {
            Keyframe lastKey = Curve.keys.Last();
            return lastKey.time;
        }
    }

}

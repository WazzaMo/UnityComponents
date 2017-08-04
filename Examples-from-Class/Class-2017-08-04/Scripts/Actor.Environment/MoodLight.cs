/*
 * Spawner Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub account WazzaMo
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Environment {


    public class MoodLight : MonoBehaviour {
        const int OUT_OF_TRANSITION = -1;

        [SerializeField]
        private Color[] ColorsToChoose = new Color[] {
            Color.red,
            Color.blue,
            Color.green
        };

        [SerializeField]
        private float BlendSpeedInSeconds = 0.5f;

        private Light _LightComponent;
        private float _BlendInterval;
        private int _CurrentColorIndex;
        private int _NextColorIndex;
        private float _StartTransitionTime;
        private System.Random _RandomNumberGenerator;

        public bool IsReady { get { return _LightComponent != null; } }
        public bool IsInTransition { get { return _NextColorIndex != OUT_OF_TRANSITION; } }

        void Start() {
            SetupRandomNumberGenerator();
            Setup();
            InitialiseCurrentColor();
            ClearTransitionState();
        }

        private void Update() {
            if (IsInTransition) {
                _LightComponent.color = GetColorForThisFrame();
                ResetTransitionStateIfTransitionCompleted();
            }
        }

        public void BlendToNextRandomColor() {
            if (!IsInTransition) {
                SetNextColor();
            }
        }

        private void SetNextColor() {
            _NextColorIndex = GetNextRandomIndex();
            if (_NextColorIndex >= ColorsToChoose.Length || _NextColorIndex < 0) {
                Debug.LogWarningFormat("RandomNumber was {0} and max colors = {1}", _NextColorIndex, ColorsToChoose.Length);
            }
            _StartTransitionTime = BlendSpeedInSeconds;
        }

        private Color GetColorForThisFrame() {
            float progress = GetTransitionProgress();
            Color current, next;
            current = ColorsToChoose[_CurrentColorIndex];
            next = ColorsToChoose[_NextColorIndex];
            return Color.Lerp(current, next, progress);
        }

        private float GetTransitionProgress() {
            _StartTransitionTime -= Time.deltaTime;
            return (1 - (_StartTransitionTime / BlendSpeedInSeconds));
        }

        private void ResetTransitionStateIfTransitionCompleted() {
            if (_StartTransitionTime < 0.001f) {
                ClearTransitionState();
            }
        }

        private void Setup() {
            _LightComponent = GetComponent<Light>();
            if (!IsReady) {
                Debug.LogWarningFormat("{0} needs to be attached to a GameObject with a Light component.  None was found for {1}.",
                    GetType().Name,
                    gameObject.name
                );
            }
        }

        private void InitialiseCurrentColor() {
            _CurrentColorIndex = GetNextRandomIndex();
            if (IsReady) {
                _LightComponent.color = ColorsToChoose[_CurrentColorIndex];
            }
        }

        private void SetupRandomNumberGenerator() {
            _RandomNumberGenerator = new System.Random();
        }

        private int GetNextRandomIndex() {
            return _RandomNumberGenerator.Next(0, ColorsToChoose.Length);
        }

        private void ClearTransitionState() {
            _NextColorIndex = OUT_OF_TRANSITION;
        }
    }

}
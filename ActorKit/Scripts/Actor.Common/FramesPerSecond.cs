/*
 * FramesPerSecond Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Tools.Common;

namespace Actor.Common {

    [AddComponentMenu("UnityComponents/FramesPerSecond")]
    public class FramesPerSecond : MonoBehaviour {
        public Text _FpsTextBox = null;
        public int _NumFrames = 10;

        private bool _IsReady;
        private double _FramesPerSecond, _TotalFramesInPeriod;
        private int _UpdateCount;

        private void Start() {
            Setup();
            _UpdateCount = 0;
        }

        private void Update() {
            if (_IsReady) {
                double fps = 1 / Time.deltaTime;
                _TotalFramesInPeriod += fps;
                _UpdateCount++;
                _FramesPerSecond = _TotalFramesInPeriod / _UpdateCount;

                if (_UpdateCount == _NumFrames) {
                    _UpdateCount = 0;
                    _TotalFramesInPeriod = 0;
                }
                if(_UpdateCount == 0) {
                    RefreshText();
                }
            }
        }

        private void Setup() {
            _IsReady = true;
            this.AreAllEditorValuesConfigured(ref _IsReady, _FpsTextBox);
        }

        private void RefreshText() {
            _FpsTextBox.text = _FramesPerSecond.ToString("N1");
        }
    }

}

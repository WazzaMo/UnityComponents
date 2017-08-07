using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Actor.Inputs {

    public class TouchListener : MonoBehaviour {
        private Text _DebugTextBox;

        void Start() {
            _DebugTextBox = GetComponent<Text>();
        }

        public bool IsReady { get { return _DebugTextBox != null; } }

        public void TouchEvent(float domainValue) {
            var message = string.Format("TouchEvent [{0}]", domainValue);
            ShowLine(message);
        }

        private void ShowLine(string message) {
            if (IsReady) {
                _DebugTextBox.text = message + "\n" + _DebugTextBox.text;
            }
        }
    }

}
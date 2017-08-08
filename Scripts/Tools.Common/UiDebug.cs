/*
 * UiDebug Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Tools.Common {

    [RequireComponent(typeof(Text))]
    public class UiDebug : MonoBehaviour {
        private static UiDebug _SingleUiDebug = null;

        public static bool IsOkToLog { get { return _SingleUiDebug != null && _SingleUiDebug.IsReady; } }

        public static void Log(string formatMessage, params System.Object[] values) {
            if (IsOkToLog) {
                string message = string.Format(formatMessage, values);
                _SingleUiDebug.ShowLine(message);
            } else {
                Debug.LogFormat("UiDebug - unable to log to display: {0}", string.Format(formatMessage, values));
            }
        }

        private Text _DebugTextBox;

        void Awake() {
            _DebugTextBox = GetComponent<Text>();
            _SingleUiDebug = this;
        }

        public bool IsReady { get { return _DebugTextBox != null; } }

        private void ShowLine(string message) {
            if (IsReady) {
                _DebugTextBox.text = message + "\n" + _DebugTextBox.text;
            }
        }
    }

}
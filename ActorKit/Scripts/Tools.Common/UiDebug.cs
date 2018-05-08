/*
 * UiDebug Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Tools.Common {

    [AddComponentMenu("UnityComponents/Tools.Common/UiDebug")]
    [RequireComponent(typeof(Text))]
    public class UiDebug : MonoBehaviour {
        private static UiDebug _SingleUiDebug = null;

        public static bool IsOkToLog { get { return _SingleUiDebug != null && _SingleUiDebug.IsReady; } }

        public static void Log(string formatMessage, params System.Object[] values) {
            string message = string.Format(formatMessage, values);
            if (IsOkToLog) {
                _SingleUiDebug.ShowLine(message);
            }// else {
                Debug.Log("LOG: " + message);
            //}
        }

        private Text _DebugTextBox;
        private string[] _TextLines = new string[10];

        void Awake() {
            _DebugTextBox = GetComponent<Text>();
            _SingleUiDebug = this;
        }

        public bool IsReady { get { return _DebugTextBox != null; } }

        private void ShowLine(string message) {
            if (IsReady) {
                ShiftLinesDown();
                SetNewRowZero(message);
                _DebugTextBox.text = GetConcatenatedRows();
            }
        }

        private void ShiftLinesDown() {
            for(int row = 1; row < _TextLines.Length; row++) {
                _TextLines[row] = _TextLines[row - 1];
            }
        }

        private void SetNewRowZero(string message) {
            _TextLines[0] = message.TrimEnd();
        }

        private string GetConcatenatedRows() {
            StringBuilder builder = new StringBuilder();
            for(int row  = 0; row < _TextLines.Length; row++) {
                builder.AppendLine(_TextLines[row]);
            }
            return builder.ToString();
        }
    }

}
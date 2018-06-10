/*
 * EdditorWidgits Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using Tools.Common;

namespace Actor.EditorUI {

    public static class EditorWidgits {
        internal static readonly GUILayoutOption[] NO_OPTIONS = new GUILayoutOption[0];

        public static void Text(GUIStyle style, string value) {
            GUILayout.Label(value, style);
        }

        public static void ValidatedField(string Label, string value, Action<string> onChange, Action onClick, Func<string, bool> isValid) {
            const string BUTTONTEXT = "Submit";

            Horizontal(NO_OPTIONS, null, () => {
                GUILayout.Label(Label);
                var newValue = GUILayout.TextField(value, GUI.skin.textField);
                if (!newValue.Equals(value)) {
                    onChange(newValue);
                }
                if (isValid(newValue)) {
                    if (GUILayout.Button(BUTTONTEXT))
                        onClick();
                } else {
                    Background(Color.red, () => Text(GUI.skin.button, BUTTONTEXT));
                }
            });
        }

        public static void TextField(string Label, string value, Action<string> onChange) {
            var newValue = EditorGUILayout.TextField(Label, value);
            if (!newValue.Equals(value)) {
                onChange(newValue);
            }
        }

        public static void Button(GUIStyle style, string text, Action onClick, float width = 0f) {
            bool clicked = width == 0 ? GUILayout.Button(text, style) : GUILayout.Button(text, style, GUILayout.Width(width));
            if (clicked) {
                onClick();
            }
        }

        public static void Popup(string Label, int selectionIndex, string[] options, Action<int> onIndexChange) {
            int newValue = EditorGUILayout.Popup(Label, selectionIndex, options);
            if (newValue != selectionIndex) {
                onIndexChange(newValue);
            }
        }

        public static void Row(Color background, GUILayoutOption option, GUIStyle style, params Action[] widgets) {
            Background(background, () => ShowRow(new GUILayoutOption[1] { option }, style, widgets));
        }

        public static void Row(Color background, GUIStyle style, params Action[] widgets) {
            Background(background, () => ShowRow(NO_OPTIONS, style, widgets));
        }

        internal static void ShowRow(GUILayoutOption[] options, GUIStyle style, Action[] widgets) {
            Horizontal(options, style, () => widgets.ForEach(wgt => wgt()));
        }

        internal static void Background(Color color, Action Do) {
            var oldBkg = GUI.backgroundColor;
            GUI.backgroundColor = color;
            Do();
            GUI.backgroundColor = oldBkg;
        }

        internal static void Horizontal(GUILayoutOption[] options, GUIStyle style, Action Do) {
            if (style == null) {
                GUILayout.BeginHorizontal(options);
            } else {
                GUILayout.BeginHorizontal(style, options);
            }
            Do();
            GUILayout.EndHorizontal();
        }
    }

}

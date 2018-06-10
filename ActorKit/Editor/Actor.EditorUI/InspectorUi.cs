/*
 * InspectorUi Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Tools.Common;

namespace Actor.EditorUI {

    public class InspectorUi : Editor {
        private static readonly GUILayoutOption[] NO_OPTIONS = new GUILayoutOption[0];

        public static Action Button(string text, Action onClick, float width = 0f) {
            return () => EditorWidgits.Button(GUI.skin.button, text, onClick, width);
        }

        public static Action Button(string text, Action onClick, Func<bool> isValid, float width = 0f) {
            return () => {
                if (isValid()) {
                    EditorWidgits.Button(GUI.skin.button, text, onClick, width);
                    Logging.Log("valid!");
                } else {
                    EditorWidgits.Background(Color.red, () => EditorWidgits.Text(GUI.skin.button, text));
                    Logging.Log("Not valid");
                }
            };
        }

        public static Action FloatField(string Label, float value, Action<float> onChange) {
            return () => EditorWidgits.TextField(Label, value.ToString(), textVal => onChange(ParseText(value, textVal)));
        }

        public static Action IntField(string Label, int value, Action<int> onChange) {
            return () => EditorWidgits.TextField(Label, value.ToString(), textVal => onChange(ParseText(value, textVal)));
        }

        public static Action EnumField<Tenum>(string Label, Tenum value, Action<Tenum> onChange) {
            return () => EditorWidgits.TextField(Label, value.ToString(), textVal => onChange(ParseTextEnum<Tenum>(value, textVal)));
        }

        public static Action Popup(string Label, int selectionIndex, string[] options, Action<int> onIndexChange) {
            return () => EditorWidgits.Popup(Label, selectionIndex, options, onIndexChange);
        }

        public static Action Text(string value) {
            return () => GUILayout.Label(value);
        }

        public static Action Text(Color background, GUIStyle style, string value) {
            return () => EditorWidgits.Background(background, () => EditorWidgits.Text(style, value));
        }

        public static Action TextField(string Label, string value, Action<string> onChange) {
            return () => EditorWidgits.TextField(Label, value, onChange);
        }

        public static Action ValidatedTextField(string Label, string value, Action<string> onChange, Action onClick, Func<string, bool> isValid) {
            return () => EditorWidgits.ValidatedField(Label, value, onChange, onClick, isValid);
        }

        //--- Layout ---
        public static void Row(params Action[] widgets) {
            EditorWidgits.ShowRow(NO_OPTIONS, style: null, widgets: widgets);
        }

        public static void Row(Color background, params Action[] widgets) {
            EditorWidgits.Background(background, () => EditorWidgits.ShowRow(NO_OPTIONS, style: null, widgets: widgets));
        }

        public static void Row(GUIStyle style, params Action[] widgets) {
            if (style != null) {
                EditorWidgits.ShowRow(NO_OPTIONS, style, widgets);
            } else {
                Logging.Warning("Given style parameter but was NULL!");
            }
        }

        private static void WithColor(Color color, Action goDo) {
            Color before = GUI.backgroundColor;
            GUI.backgroundColor = color;
            goDo();
            GUI.backgroundColor = before;
        }

        private static int ParseText(int oldValue, string textValue) {
            int val;
            if (Int32.TryParse(textValue, out val)) {
                return val;
            } else {
                return oldValue;
            }
        }

        private static float ParseText(float oldValue, string textValue) {
            float val;
            if (Single.TryParse(textValue, out val)) {
                return val;
            } else {
                return oldValue;
            }
        }

        private static T ParseTextEnum<T>(T oldValue, string textValue) {
            T val;
            try {
                val = (T)Enum.Parse(typeof(T), textValue);
            } catch(Exception) {
                val = oldValue;
            }
            return val;
        }

    }

}

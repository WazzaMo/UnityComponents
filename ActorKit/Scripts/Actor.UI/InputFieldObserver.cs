/*
 * InputFieldObserver Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;

using UnityEngine;
using UnityEngine.UI;


namespace Actor.UI {

    using ValidatorFunc = InputField.OnValidateInput;

    public class InputFieldObserver {
        public static char NUMERIC_VALIDATOR(string text, int index, char addedChar) { return Char.IsDigit(addedChar) || addedChar == '.' ? addedChar : '\0'; }

        private ValidatorFunc _Validator;
        private float _Value;
        private Action<InputFieldObserver> _OnChange;
        private int _Length;

        public InputFieldObserver(InputField inputField, ValidatorFunc validator = null) {
            IsValid = false;
            _Value = 0f;
            _Length = 0;
            inputField.onValueChanged.AddListener(OnChange);
            _Validator = validator;
            if (_Validator != null) {
                inputField.onValidateInput += _Validator;
            }
            _OnChange = null;
        }

        public float Value { get { return _Value; } }
        public bool IsValid { get; private set; }
        public bool IsEmpty { get { return _Length == 0; } }

        public void AddOnChangeListener(Action<InputFieldObserver> observer) {
            if (_OnChange == null) {
                _OnChange = observer;
            } else {
                _OnChange += observer;
            }
        }

        private void OnChange(string newText) {
            IsValid = Single.TryParse(newText, out _Value);
            _Length = newText.Length;
            _OnChange(this);
        }
    }
}


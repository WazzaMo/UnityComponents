/*
 * WayPoint Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Relative {

    public class WayPointChangeEvent : UnityEvent<WayPoint> { }

    public class WayPoint : MonoBehaviour {
        private Action<WayPoint> OnChange;

        private Transform _Transform;
        private int _NodeIndex;

        void Awake() {
            _Transform = GetComponent<Transform>();
            _NodeIndex = 0;
            OnChange = x => { };
        }

        void Update() {
            if (IsReady) {
                NotifyIfChanged();
            }
        }

        public Vector3 Position { get { return _Transform.position; } }
        public int NodeIndex { get { return _NodeIndex; }  set { _NodeIndex = value; } }

        public void AddListener(Action<WayPoint> listener) {
            OnChange += listener;
        }

        private void OnDrawGizmosSelected() {
            Vector3 size = Vector3.one / 5;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, size);
        }

        private void OnDrawGizmos() {
            Vector3 size = Vector3.one / 5;
            Gizmos.color = Color.white;
            Gizmos.DrawCube(transform.position, size);
        }

        public bool IsReady { get { return _Transform != null; } }

        private void NotifyIfChanged() {
            if(HasMovedSince()) {
                OnChange(this);
            }
        }

        private bool HasMovedSince() {
            bool val = _Transform.hasChanged;
            _Transform.hasChanged = false;
            return val;
        }
    }

}
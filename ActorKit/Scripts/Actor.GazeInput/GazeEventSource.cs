/*
 * GazeEventSource Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;

using Tools.Common;

using Actor.Events;

namespace Actor.GazeInput {

    public class GazeEventSource : MonoBehaviour {
        private Camera _Camera;
        private GazeData _Current;
        private Vector3 _CentreScreenPos;
        private List<IGazeEventPolicy> _EventPolicies;
        private IGazeEventBroadcaster _EventBroadcaster;

        public int _NumConcurrentHits = 2;
        public float _MaxDepth = 50f;

        public bool IsReady { get { return _Camera != null && _NumConcurrentHits > 0; } }

        private void Start() {
            Setup();
            RegisterEventPolicies();
        }

        private void FixedUpdate() {
            if (IsReady) {
                RaycastHit hit;
                if (CheckRaycast(out hit)) {
                    if (IsGazeDataNeedingUpdate(hit)) {
                        UpdateGazeDataViaPolicies();
                    }
                }
            }
        }

        private bool CheckRaycast(out RaycastHit hitInfo) {
            RaycastHit[] hits = new RaycastHit[_NumConcurrentHits];
            Ray ray = _Camera.ScreenPointToRay(_CentreScreenPos);
            int numHits = Physics.RaycastNonAlloc(ray, hits, _MaxDepth);
            bool isHitFound = (numHits > 0);
            hitInfo = isHitFound ? hits[numHits - 1] : default(RaycastHit);
            return isHitFound;
        }

        private bool IsGazeDataNeedingUpdate(RaycastHit hit) {
            IGazeEventHandler handler = hit.collider.GetComponent<IGazeEventHandler>();
            if (handler != null) {
                _Current.GazeTarget = hit.collider.gameObject;
                _Current.GazeHandler = handler;
                _Current.EventKind = GazeEventKind.NoEvent;
                return true;
            }
            return false;
        }

        private void UpdateGazeDataViaPolicies() {
            List<GazeData> eventList = new List<GazeData>();
            _EventPolicies.ForEach(policy => {
                policy.ApplyGesturePolicy(_Current, eventList);
            });
            _EventBroadcaster.BroadcastGazeEvents(eventList);
        }

        private void Setup() {
            _EventBroadcaster = new SimpleGazeEventBroadcaster();
            SetupCamera();
            SetupGazeData();
            _CentreScreenPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }

        private void SetupCamera() {
            _Camera = this.GetComponentOrWarn<Camera>();
        }

        private void SetupGazeData() {
            _Current = new GazeData() {
                GazeTarget = null,
                TimeGazing = 0f,
                GazeHandler = null
            };
        }

        private void RegisterEventPolicies() {
            Component[] components = GameObject.FindObjectsOfType<Component>();
            _EventPolicies = components
                .Where(comp => comp is IGazeEventPolicy)
                .Select(policy => policy as IGazeEventPolicy)
                .ToList();
            if (_EventPolicies.Count == 0) {
                Logging.Log<GazeEventSource>("No Gaze Event policies found");
            }
        }
    }

}

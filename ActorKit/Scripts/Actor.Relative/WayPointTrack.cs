/*
 * WayPointTrack Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Actor.Relative {

    public class WayPointTrack : MonoBehaviour {
        public class PositionOnTrackEvent : UnityEvent<Vector3> { };

        [SerializeField] private WayPoint[] WayPointNodes = new WayPoint[0];
        [SerializeField] private bool IsTrackVisible = false;

        private Vector3[] _NodePositions;
        private float _TotalLength;
        private LineRenderer _LineRenderer;

        public WayPoint[] Points { get { return WayPointNodes; } }

        void Start() {
            Setup();
            if (IsTrackVisible) {
                SetupLineRenderer();
            }
        }

        public void WayPointNodeUpdate(WayPoint point) {
            if (IsReady) {
                UpdatePosition(point);
                MeasureTrackLength();
                UpdatePointsToLineRenderer();
            }
        }

        public bool IsReady { get { return WayPointNodes != null && WayPointNodes.Length > 1; } }

        public Vector3 GetPointOnTrack(float ProgressLevel) {
            if (ProgressLevel < 0 || ProgressLevel > 1.0f) {
                Debug.LogErrorFormat("GetPointOnTrack(ProgressLevel) - ProgressLevel must be between 0 and 1 but was {0}", ProgressLevel);
                ProgressLevel = 0f;
            }
            float pointLength = ProgressLevel * _TotalLength;
            float portionOfLastSegment;
            int indexOfStartPos = FindIndexForLength(pointLength, out portionOfLastSegment);
            return GetPositionBetweenPoints(indexOfStartPos, portionOfLastSegment);
        }

        private Vector3 GetPositionBetweenPoints(int indexFirstPoint, float portionToNext) {
            int indexSecondPoint = indexFirstPoint < (_NodePositions.Length - 1) ? indexFirstPoint + 1 : 0;
            Vector3 first = _NodePositions[indexFirstPoint];
            Vector3 second = _NodePositions[indexSecondPoint];
            return Vector3.Lerp(first, second, portionToNext);
        }

        private int FindIndexForLength(float pointLength, out float lengthPortion) {
            int index = 0;
            float remainingLength = pointLength;
            float length = LengthFromNode(0);

            while(remainingLength > length && index < _NodePositions.Length) {
                remainingLength -= length;
                index++;
                length = LengthFromNode(index);
            }
            lengthPortion = remainingLength / length;
            return index;
        }

        private float LengthFromNode(int index) {
            Vector3 first = _NodePositions[index];
            Vector3 second = _NodePositions[index + 1];
            return Vector3.Magnitude(second - first);
        }

        private void Setup() {
            if (IsReady) {
                RegisterWithAllNodes();
                MeasureTrackLength();
            }
        }

        private void RegisterWithAllNodes() {
            WayPoint node;

            _NodePositions = new Vector3[WayPointNodes.Length + 1];
            for(int index = 0; index < WayPointNodes.Length; index++ ) {
                node =  WayPointNodes[index];
                if (node != null) {
                    node.AddListener(WayPointNodeUpdate);
                    node.NodeIndex = index;
                    UpdatePosition(node);
                } else {
                    Debug.LogFormat("Node #{0} not initialised!", index);
                }
            }
        }

        private void UpdateAllPositions() {
            WayPoint node;

            for (int index = 0; index < WayPointNodes.Length; index++) {
                node = WayPointNodes[index];
                if (node != null) {
                    UpdatePosition(node);
                }
            }
            if (IsTrackVisible) {
                UpdatePointsToLineRenderer();
            }
        }

        private void UpdatePosition(WayPoint node) {
            _NodePositions[node.NodeIndex] = node.Position;
            if (node.NodeIndex == 0) {
                _NodePositions[_NodePositions.Length - 1] = node.Position;
            }
        }

        private void MeasureTrackLength() {
            Vector3 first = _NodePositions[0];
            Vector3 second;

            _TotalLength = 0f;
            for(int index = 1; index < _NodePositions.Length; index++) {
                second = _NodePositions[index];
                _TotalLength += Vector3.Magnitude(second - first);
                first = second;
            }
        }

        private void SetupLineRenderer() {
            _LineRenderer = gameObject.AddComponent<LineRenderer>();
            _LineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            _LineRenderer.widthMultiplier = 0.2f;
            UpdatePointsToLineRenderer();
        }

        private void UpdatePointsToLineRenderer() {
            if (_NodePositions != null) {
                _LineRenderer.positionCount = _NodePositions.Length;
                _LineRenderer.SetPositions(_NodePositions);
            }
        }
    }

}
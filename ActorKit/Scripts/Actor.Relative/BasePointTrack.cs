/*
 * BasePointTrack Unity Component
 *         ~ A Base Class ~
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Actor.Relative {

    public class BasePointTrack : BaseTrack {

        [Tooltip("Makes Track visible in-game")]
        [SerializeField] private bool IsTrackVisible = false;

        [Tooltip("Material to use to visualize track.")]
        [SerializeField] Material MaterialForVisibleTrack;

        private Vector3[] _NodePositions;
        private float _TotalLength;
        private LineRenderer _LineRenderer;
        private WayPoint[] _WayPointNodes = new WayPoint[0];

        public bool IsReady { get { return _WayPointNodes != null && _WayPointNodes.Length > 1; } }
        
        public bool TrackVisibility {
            get { return IsTrackVisible && MaterialForVisibleTrack != null; }
            set { IsTrackVisible = value; }
        }

        public Material TrackPathMaterial {
            get { return MaterialForVisibleTrack; }
            set { MaterialForVisibleTrack = value; }
        }

        public void WayPointNodeUpdate(WayPoint point) {
            if (IsReady) {
                UpdatePosition(point);
                MeasureTrackLength();
                UpdatePointsToLineRenderer();
            }
        }

        public WayPoint[] PointsInEditor { get { return gameObject.GetComponentsInChildren<WayPoint>(); } }

        public override Vector3 GetPointFromRelativeInput(float relativeInput) {
            if (relativeInput < 0 || relativeInput > 1.0f) {
                Debug.LogErrorFormat("GetPointOnTrack(ProgressLevel) - ProgressLevel must be between 0 and 1 but was {0}", relativeInput);
                relativeInput = 0f;
            }
            float pointLength = relativeInput * _TotalLength;
            float portionOfLastSegment;
            int indexOfStartPos = FindIndexForLength(pointLength, out portionOfLastSegment);
            return GetPositionBetweenPoints(indexOfStartPos, portionOfLastSegment);
        }

        protected void ConfigureWayPointNodes() {
            RegisterWithAllNodes();
            MeasureTrackLength();
        }

        protected void SetWayPointNodes(WayPoint[] points) {
            _WayPointNodes = points;
        }

        private void RegisterWithAllNodes() {
            WayPoint node;

            _NodePositions = new Vector3[_WayPointNodes.Length + 1];
            for (int index = 0; index < _WayPointNodes.Length; index++) {
                node = _WayPointNodes[index];
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

            for (int index = 0; index < _WayPointNodes.Length; index++) {
                node = _WayPointNodes[index];
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
            for (int index = 1; index < _NodePositions.Length; index++) {
                second = _NodePositions[index];
                _TotalLength += Vector3.Magnitude(second - first);
                first = second;
            }
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

            while (remainingLength > length && index < _NodePositions.Length) {
                remainingLength -= length;
                index++;
                length = LengthFromNode(index);
            }
            lengthPortion = remainingLength / length;
            return index;
        }

        private float LengthFromNode(int index) {
            if (_NodePositions == null) {
                Debug.LogWarning("Nodes are not initialized.  Cannot determine position on track.");
                return 0f;
            } else {
                Vector3 first = _NodePositions[index];
                Vector3 second = _NodePositions[index + 1];
                return Vector3.Magnitude(second - first);
            }
        }

        protected void SetupLineRenderer() {
            EnsureLineRendererIsAttached();
            if (_LineRenderer != null) {
                _LineRenderer.material = MaterialForVisibleTrack;
                _LineRenderer.widthMultiplier = 0.2f;
                UpdatePointsToLineRenderer();
            }
        }

        private void EnsureLineRendererIsAttached() {
            _LineRenderer = gameObject.GetComponent<LineRenderer>();
            if (_LineRenderer == null) {
                _LineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }

        protected void UpdatePointsToLineRenderer() {
            if (_NodePositions != null && _LineRenderer != null) {
                _LineRenderer.positionCount = _NodePositions.Length;
                _LineRenderer.SetPositions(_NodePositions);
            }
        }

    }
}

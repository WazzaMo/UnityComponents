/*
 * MeshUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Common {

    using UnityEngine;

    public class MeshUtil {
        private Mesh _Mesh;
        private Vector3 _Smallest;
        private Vector3 _Largest;
        private Vector3 _Scale;

        public MeshDimensions WorldScaleDimensions { get; private set; }
        public MeshDimensions UnscaledDimensions { get; private set; }

        public MeshUtil(Mesh mesh, Transform transform) {
            SetupWithMeshAndTransform(mesh, transform);
        }

        public MeshUtil(GameObject gameObject, ref bool isReady) {
            SetupWithGameObject(gameObject, ref isReady);
        }

        public MeshUtil(GameObject gameObject) {
            bool isReady = true;
            SetupWithGameObject(gameObject, ref isReady);
        }

        public bool IsInVolume(Transform transform, Vector3 point, out Vector3 volumetricPos) {
            Vector3 localPoint = GetPoint(transform, point);
            float x, y, z;

            x = (localPoint.x - _Smallest.x) / (_Largest.x - _Smallest.x);
            y = (localPoint.y - _Smallest.y) / (_Largest.y - _Smallest.y);
            z = (localPoint.z - _Smallest.z) / (_Largest.z - _Smallest.z);

            volumetricPos = new Vector3(x, y, z);
            return (x >= 0f && x <= 1.0f)
                && (y >= 0f && y <= 1.0f)
                && (z >= 0f && z <= 1.0f);
        }

        public bool IsInXZPlane(Transform transform, Vector3 point, out Vector2 planarPos) {
            Vector3 localPoint = GetPoint(transform, point);

            float x, z;
            x = (localPoint.x - _Smallest.x) / (_Largest.x - _Smallest.x);
            z = (localPoint.z - _Smallest.z) / (_Largest.z - _Smallest.z);
            planarPos = new Vector2(x, z);
            return (x >= 0f && x <= 1.0f)
                && (z >= 0f && z <= 1.0f);
        }

        private void SetupWithGameObject(GameObject gameObject, ref bool isReady) {
            if (isReady) {
                var mesh = gameObject.GetMeshOrWarn(ref isReady);
                var _transform = gameObject.transform;
                SetupWithMeshAndTransform(mesh, _transform);
            } else {
                MakeEmpty();
            }
        }

        private void SetupWithMeshAndTransform(Mesh mesh, Transform _Transform) {
            _Mesh = mesh;
            var OnePoint = new Vector3(1, 1, 1);
            var scaledOne = _Transform.TransformPoint(OnePoint);
            Logging.Log("{0}: ScaleOne = {1}", typeof(MeshUtil).Name, scaledOne);
            //_Scale = _Transform.localScale;
            _Scale = scaledOne;
            AnalyseMeshToGetExtentsInAllAxes();
            CalculateMinAndMax();
            Logging.Log("{0}: Scale of {1} = {2}",
                typeof(MeshUtil).Name,
                _Transform.gameObject.name,
                _Scale
            );
        }

        private void MakeEmpty() {
            _Smallest = Vector3.zero;
            _Largest = Vector3.zero;
            _Scale = Vector3.one;
        }

        private Vector3 GetPoint(Transform transform, Vector3 point) {
            return transform.worldToLocalMatrix.MultiplyPoint(point);
        }

        private void AnalyseMeshToGetExtentsInAllAxes() {
            IEnumerable<Vector3> vertices = _Mesh.vertices;

            _Smallest.x = vertices.Select(v => v.x).Aggregate((a, b) => a < b ? a : b);
            _Smallest.y = vertices.Select(v => v.y).Aggregate((a, b) => a < b ? a : b);
            _Smallest.z = vertices.Select(v => v.z).Aggregate((a, b) => a < b ? a : b);

            _Largest.x = vertices.Select(v => v.x).Aggregate((a, b) => a > b ? a : b);
            _Largest.y = vertices.Select(v => v.y).Aggregate((a, b) => a > b ? a : b);
            _Largest.z = vertices.Select(v => v.z).Aggregate((a, b) => a > b ? a : b);

            Logging.Log(
                "X: {0} to {1}, "+
                "Y: {2} to {3}, "+
                "Z: {4} to {5} [Scale {6}]",
                _Smallest.x, _Largest.x,
                _Smallest.y, _Largest.y,
                _Smallest.z, _Largest.z,
                _Scale
            );
        }

        private void CalculateMinAndMax(){
            WorldScaleDimensions = CalculateDimensions(_Scale);
            UnscaledDimensions = CalculateDimensions(new Vector3(1, 1, 1));
        }

        private MeshDimensions CalculateDimensions(Vector3 scaling) {
            MeshDimensions dimensions = new MeshDimensions(
                minX: _Smallest.x * scaling.x,
                maxX: _Largest.x * scaling.x,
                minY: _Smallest.y * scaling.y,
                maxY: _Largest.y * scaling.y,
                minZ: _Smallest.z * scaling.z,
                maxZ: _Largest.z * scaling.z
            );
            return dimensions;
        }
    }
}

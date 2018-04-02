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

        public float MinX { get { return _Smallest.x * _Scale.x; } }
        public float MaxX { get { return _Largest.x * _Scale.x; } }
        public float MinY { get { return _Smallest.y * _Scale.y; } }
        public float MaxY { get { return _Largest.y * _Scale.y; } }
        public float MinZ { get { return _Smallest.z * _Scale.z; } }
        public float MaxZ { get { return _Largest.z * _Scale.z; } }

        public float Width { get { return MaxX - MinX; } }
        public float Height { get { return MaxY - MinY; } }
        public float Depth { get { return MaxZ - MinZ; } }

        public MeshUtil(Mesh mesh, Transform transform) {
            _Mesh = mesh;
            _Scale = transform.localScale;
            AnalyseMesh();
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
                _Mesh = gameObject.GetMeshOrWarn(ref isReady);
                _Scale = gameObject.transform.localScale;
                AnalyseMesh();
            } else {
                MakeEmpty();
            }
        }

        private void MakeEmpty() {
            _Smallest = Vector3.zero;
            _Largest = Vector3.zero;
            _Scale = Vector3.one;
        }

        private Vector3 GetPoint(Transform transform, Vector3 point) {
            return transform.worldToLocalMatrix.MultiplyPoint(point);
        }

        private void AnalyseMesh() {
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
                "Z: {4} to {5}",
                _Smallest.x, _Largest.x,
                _Smallest.y, _Largest.y,
                _Smallest.z, _Largest.z
            );
        }
    }
}

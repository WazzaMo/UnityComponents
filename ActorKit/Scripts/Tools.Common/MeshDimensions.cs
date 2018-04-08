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

    public struct MeshDimensions {
        private float _MinX;
        private float _MaxX;
        private float _MinY;
        private float _MaxY;
        private float _MinZ;
        private float _MaxZ;
        private float _Width;
        private float _Height;
        private float _Depth;
        private float _ExtentX;
        private float _ExtentY;
        private float _ExtentZ;

        public MeshDimensions(float minX, float maxX, float minY, float maxY, float minZ, float maxZ) {
            _MinX = minX;
            _MaxX = maxX;
            _MinY = minY;
            _MaxY = maxY;
            _MinZ = minZ;
            _MaxZ = maxZ;
            _Width = maxX - minX;
            _Height = maxY - minY;
            _Depth = maxZ - minZ;
            _ExtentX = _Width / 2;
            _ExtentY = _Height / 2;
            _ExtentZ = _Depth / 2;
        }

        public float MinX {
            get {
                return _MinX;
            }
        }
        public float MaxX {
            get {
                return _MaxX;
            }
        }
        public float MinY {
            get {
                return _MinY;
            }
        }
        public float MaxY {
            get {
                return _MaxY;
            }
        }
        public float MinZ {
            get {
                return _MinZ;
            }
        }
        public float MaxZ {
            get {
                return _MaxZ;
            }
        }

        public float Width {
            get {
                return _Width;
            }
        }
        public float Height {
            get {
                return _Height;
            }
        }
        public float Depth {
            get {
                return _Depth;
            }
        }

        public float ExtentX {
            get {
                return _ExtentX;
            }
        }
        public float ExtentY {
            get {
                return _ExtentY;
            }
        }
        public float ExtentZ {
            get {
                return _ExtentZ;
            }
        }
    }


}

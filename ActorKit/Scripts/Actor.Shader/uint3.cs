/*
 * uint3 Shader Util Type Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;

namespace Actor.Shader {

    public struct uint3 {
        public readonly static uint3 ZERO = new uint3(0, 0, 0);

        public uint x, y, z;

        public uint2 xy { get { return new uint2(x, y); } }
        public uint2 yz { get { return new uint2(y, z); } }
        public uint2 xz { get { return new uint2(x, z); } }

        public uint3(uint _x, uint _y, uint _z) {
            x = _x;
            y = _y;
            z = _z;
        }

        public override string ToString() {
            return string.Format("uint3({0}, {1}, {2})", x, y, z);
        }
    }

}

/*
 * uint2 Shader Util Type Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;

namespace Actor.Shader {

    public struct uint2 {
        public readonly static uint2 ZERO = new uint2(0, 0);

        public uint x, y;

        public uint2(uint _x, uint _y) {
            x = _x;
            y = _y;
        }

        public override string ToString() {
            return string.Format("uint2({0}, {1})", x, y);
        }
    }

}

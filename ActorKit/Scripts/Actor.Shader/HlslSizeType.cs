/*
 * HlslSizeType Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


namespace Actor.Shader {

    public struct HlslSizeType {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }

        public HlslSizeType(int width, int height, int depth) {
            Width = width > 0 ? width : 0;
            Height = height > 0 ? height : 0;
            Depth = depth > 0 ? depth : 0;
        }

        public HlslSizeType(uint width, uint height, uint depth) {
            Width = (int) width;
            Height = (int) height;
            Depth = (int) depth;
        }
    }

}

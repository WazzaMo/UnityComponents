/*
 * (c) Copyright 2018 Lokel Digital Pty Ltd
 * CNN for Unity
 */



using System;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;

namespace Tools.Common {

    public static class TextureUtil {
        static readonly Color ZERO = new Color(0, 0, 0, 0);

        public static void SetPixel(Texture3D texture, Color[] pixels, int x, int y, int z, Color pixel) {
            int index;
            index = x
                + texture.width * y
                + texture.width * texture.height * z;
            pixels[index] = pixel;
        }

        public static void SetPixel(Texture2D texture, Color[] pixels, int x, int y, Color pixel) {
            int index;
            index = IndexFor(texture, x, y);
            pixels[index] = pixel;
        }

        public static Color GetPixel(Texture2D texture, Color[] pixels, int x, int y) {
            int index = IndexFor(texture, x, y);
            return pixels[index];
        }

        public static Color[] MakeAndZeroPixels(Texture3D texture) {
            if (texture == null) {
                return null;
            }
            return CreatePixelValues( texture.width * texture.height * texture.depth);
        }

        public static Color[] MakeAndZeroPixels(Texture2D texture) {
            if (texture == null) {
                return null;
            }
            return CreatePixelValues(texture.width * texture.height);
        }

        private static int IndexFor(Texture2D texture, int x, int y) {
            return x + texture.width * y; ;
        }

        private static Color[] CreatePixelValues(int totalPixels) {
            var data = new Color[totalPixels];
            for (int i = 0; i < data.Length; i++) {
                data[i] = ZERO;
            }
            return data;
        }
    }

}

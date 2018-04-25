/*
 * RenderTextureUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Tools.Common {

    public static class RenderTextureUtil {
        const int COLOR_DEPTH = 24;

        public static RenderTexture GetQuickTemp(int _width, int _height, RenderTextureFormat format = RenderTextureFormat.ARGB32) {
            //RenderTextureDescriptor desc = new RenderTextureDescriptor() {
            //    enableRandomWrite = true,
            //    width = _width,
            //    height = _height,
            //    colorFormat = format,
            //    volumeDepth = 1
            //};
            //return RenderTexture.GetTemporary(desc);
            var texture = RenderTexture.GetTemporary(_width, _height);
            texture.enableRandomWrite = true;
            return texture;
        }

        public static RenderTexture CreateEmpty(int _width, int _height, RenderTextureFormat format = RenderTextureFormat.ARGB32) {
            var texture = new RenderTexture(_width, _height, COLOR_DEPTH, format);
            texture.enableRandomWrite = true;
            texture.Create();
            Graphics.Blit(BlankImage(_width, _height, Color.black), texture );
            return texture;
        }

        private static Texture2D BlankImage(int width, int height, Color color) {
            var image = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for(int index = 0; index < pixels.Length; index++) {
                pixels[index] = color;
            }
            image.SetPixels(pixels);
            image.Apply();
            return image;
        }
    }

}

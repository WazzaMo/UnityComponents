/*
 * TextureUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */



using System;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;

using Actor.Shader;

namespace Tools.Common {

    public static class TextureUtil {
        const int UNINITIALISED_KERNEL = -1;

        static readonly Color ZERO = new Color(0, 0, 0, 0);

        private static ComputeShader __GetTextureData = null;
        private static int __F1Kernel = UNINITIALISED_KERNEL;
        private static int __F1SetKernel = UNINITIALISED_KERNEL;

        public static void SetPixel(int width, int height, Color[] pixels, int x, int y, int z, Color pixel) {
            int index = IndexFor(width, height, x, y, z);
            pixels[index] = pixel;
        }

        public static void SetPixel(Texture3D texture, Color[] pixels, int x, int y, int z, Color pixel) {
            SetPixel(texture.width, texture.height, pixels, x, y, z, pixel);
        }

        public static void SetPixel(Texture2D texture, Color[] pixels, int x, int y, Color pixel) {
            SetPixel(texture.width, texture.height, pixels, x, y, 0, pixel);
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

        public static bool TryRead1Float3DTexture(RenderTexture text3d, Color[] volumeColors) {
            if (text3d.dimension == UnityEngine.Rendering.TextureDimension.Tex3D) {
                HlslSizeType size = new HlslSizeType(text3d.width, text3d.height, text3d.volumeDepth);

                bool isReady = IsShaderLoaded( __GetTextureData != null );
                isReady = isReady && Get1FKernelIfNotInitialised();
                if (isReady) {
                    ComputeBuffer floatData = ComputeBufferUtil.CreateBufferForSimpleArray<Color>(volumeColors);
                    ComputeBuffer sizeData = ComputeBufferUtil.CreateBufferForStruct<HlslSizeType>(size);
                    __GetTextureData.SetBuffer(__F1Kernel, "Size", sizeData);
                    __GetTextureData.SetBuffer(__F1Kernel, "OutColors", floatData);
                    __GetTextureData.SetTexture(__F1Kernel, "InTextureF1", text3d);
                    __GetTextureData.Dispatch(__F1Kernel, size.Width, size.Height, size.Depth);
                    floatData.GetData(volumeColors);
                    DisposablesUtil.DisposeAll(floatData, sizeData);
                    return true;
                }
            }
            return false;
        }

        public static bool TryWrite1Float3DTexture(RenderTexture text3d, Color[] colors) {
            if (text3d.dimension == UnityEngine.Rendering.TextureDimension.Tex3D) {
                HlslSizeType size = new HlslSizeType(text3d.width, text3d.height, text3d.volumeDepth);

                bool isReady = IsShaderLoaded(__GetTextureData != null);
                __F1SetKernel = ComputeShaderExt.FindKernelOrWarn(__GetTextureData, "SetTextureF1Data", ref isReady);
                isReady = isReady && Get1FKernelIfNotInitialised();
                if (isReady) {
                    ComputeBuffer colorData = ComputeBufferUtil.CreateBufferForSimpleArray<Color>(colors);
                    ComputeBuffer sizeData = ComputeBufferUtil.CreateBufferForStruct<HlslSizeType>(size);
                    __GetTextureData.SetBuffer(__F1SetKernel, "Size", sizeData);
                    __GetTextureData.SetBuffer(__F1SetKernel, "InColors", colorData);
                    __GetTextureData.SetTexture(__F1SetKernel, "OutTexture1F", text3d);
                    __GetTextureData.Dispatch(__F1SetKernel, size.Width, size.Height, size.Depth);
                    DisposablesUtil.DisposeAll(colorData, sizeData);
                    return true;
                }
            }
            return false;
        }

        private static bool IsShaderLoaded(bool isAlreadyLoaded) {
            bool isReady = isAlreadyLoaded;
            if (! isAlreadyLoaded) {
                __GetTextureData = ComputeShaderUtil.FindComputeOrWarn("Shaders/3DTextureSupport", out isReady);
            }
            return isReady;
        }

        private static bool Get1FKernelIfNotInitialised() {
            bool isReady;
            if (__GetTextureData != null && __F1Kernel == UNINITIALISED_KERNEL) {
                isReady = true;
                __F1Kernel = ComputeShaderExt.FindKernelOrWarn(__GetTextureData, "GetTextureF1Data", ref isReady);
            } else {
                isReady = __GetTextureData != null && __F1Kernel != UNINITIALISED_KERNEL;
            }
            return isReady;
        }

        private static int IndexFor(Texture2D texture, int x, int y) {
            return IndexFor(texture.width, texture.height, x, y, 0);
        }

        private static int IndexFor(int width, int height, int x, int y, int z = 0) {
            return x + width * (y + z * height);
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

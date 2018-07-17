/*
 * Texture3DCompute Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Tools.Common;

namespace Actor.Shader {

    public class Texture3DCompute : IDisposable {
        const string
            SHADERPROP_OUT_RFLOAT       = "OutData",
            SHADERPROP_IN_RFLOAT        = "InData",
            SHADERPROP_OUT_COLOR        = "OutColors",
            SHADERPROP_IN_COLOR         = "InColors",
            SHADERPROP_TEXT_IN_RGBA     = "InTextureF4",
            SHADERPROP_TEXT_OUT_RGBA    = "",
            SHADERPROP_TEXT_IN_RFLOAT   = "InTextureF1",
            SHADERPROP_TEXT_OUT_RFLOAT  = "OutTextureF1",
            SHADERPROP_SIZE             = "Size",
            SHADERPROP_TIME             = "Time",
            SHADERKERNEL_RGBA           = "GetTextureF4Data",
            KERNELGET_RFLOAT            = "GetTextureF1Data",
            KERNELSET_RFLOAT            = "SetTextureF1Data",
            KERNELGET_RFLOATCOLOR       = "GetTextureF1Color",
            KERNELSET_RFLOATCOLOR       = "SetTextureF1Color",
            SHADERPATH                  = "Shaders/3DTextureSupport";

        public enum TextureFormat {
            FloatR,
            FloatRGBA
        }

        private RenderTexture _3dTexture;
        private ComputeBuffer _SizeBuffer;
        private ComputeBuffer _DataBuffer;
        private ComputeBuffer _ColorsBuffer;
        private ComputeShader _ReadWriteShader;
        private TextureFormat _Format;
        private HlslSizeType  __Size;
        private int _KernelRGBA, _KernelSetRGBA;
        private int _KernelGetRFloat, _KernelSetRFloat;
        private int _KernelGetRFloatColor, _KernelSetRFloatColor;
        private int _TotalDataSize;
        private bool _IsReady;

        public int Width { get { return __Size.Width; } }
        public int Height { get { return __Size.Height; } }
        public int Depth { get { return __Size.Depth; } }
        public bool IsTextureReady { get { return _IsReady && _3dTexture != null; } }

        public Texture3DCompute(int width, int height, int depth, TextureFormat format) {
            _IsReady = false;
            _Format = format;
            __Size = new HlslSizeType(width, height, depth);
            _TotalDataSize = __Size.CalcTotalSize();
            SetupShaderTextureAndKernels();
            SetupSizeBuffer(width, height, depth);
            SetupDataBuffer();
        }

        public void Dispose() {
            DisposablesUtil.DisposeAll(_SizeBuffer, _DataBuffer, _ColorsBuffer);
            _SizeBuffer = null;
            _DataBuffer = null;
            _ColorsBuffer = null;
            if (_3dTexture != null) {
                _3dTexture.Release();
                _3dTexture = null;
            }
        }

        public void SetTextureInto(ComputeShader shader, int kernel, string nameID) {
            if (shader != null && IsTextureReady) {
                shader.SetTexture(kernel, nameID, _3dTexture);
            }
        }

        public void SetTextureInfo(Material mat, string nameId) {
            mat.SetTexture(nameId, _3dTexture);
        }

        public int Pos(int x, int y, int z) {
            var index = Width * (y + Height * z) + x;
            if (index > _TotalDataSize && _IsReady) {
                Logging.Warning<Texture3DCompute>(
                    "Coordinates ({0}, {1}, {2}) resulted in an index too large for [Width, Height, Depth] ({3}, {4}, {5})",
                    x,y,z,
                    Width, Height, Depth
                );
                index = 0;
            }
            return index;
        }

        public void SetPixelsRFloat(float[] data) {
            if (_IsReady && _Format == TextureFormat.FloatR) {
                if (data.Length == _TotalDataSize) {
                    _DataBuffer.SetData(data);
                    PerformWriteRFloat();
                }
            } else if (_Format != TextureFormat.FloatR) {
                Logging.Warning<Texture3DCompute>("Cannot use SetPixelsRFloat() for format {0}", _Format.ToString());
            } else {
                Logging.Warning<Texture3DCompute>("Not initialized properly");
            }
        }

        public bool TryGetPixelsRFloat(float[] arrayToPopulate) {
            bool result = false;
            if (_IsReady && _Format == TextureFormat.FloatR) {
                PerformReadRFloat();
                _DataBuffer.GetData(arrayToPopulate);
                result = true;
            } else if (_Format != TextureFormat.FloatR) {
                Logging.Warning<Texture3DCompute>("Cannot use GetPixelsRFloat() for format {0}", _Format.ToString());
            } else {
                Logging.Warning<Texture3DCompute>("Not initialized");
            }
            return result;
        }

        public void SetPixels(Color[] colors) {
            if (_IsReady) {
                if (colors.Length == _TotalDataSize) {
                    _ColorsBuffer.SetData(colors);
                    PerformWriteRFloatColor();
                } else {
                    Logging.Warning<Texture3DCompute>("Cannot use SetPixels() - expected {0} colors but was given {1} colors", colors.Length, _TotalDataSize);
                }
            } else {
                Logging.Warning<Texture3DCompute>("Not initialized");
            }
        }

        public bool TryGetPixels(Color[] colors) {
            bool result = false;
            if(_IsReady) {
                PerformReadRFloatColor();
                _ColorsBuffer.GetData(colors);
                result = true;
            }
            return result;
        }

        ~Texture3DCompute() {
            if (_SizeBuffer != null || _DataBuffer != null || _ColorsBuffer != null) {
                Logging.Warning<Texture3DCompute>("Use the IDisposable interface to clean up resources!");
                Dispose();
            }
        }

        private void PerformWriteRFloat() {
            _ReadWriteShader.SetTexture(_KernelSetRFloat, SHADERPROP_TEXT_OUT_RFLOAT, _3dTexture);
            _ReadWriteShader.SetBuffer(_KernelSetRFloat, SHADERPROP_SIZE, _SizeBuffer);
            _ReadWriteShader.SetBuffer(_KernelSetRFloat, SHADERPROP_IN_RFLOAT, _DataBuffer);
            _ReadWriteShader.Dispatch(_KernelSetRFloat, __Size.Width, __Size.Height, __Size.Depth);
        }

        private void PerformReadRFloat() {
            _ReadWriteShader.SetTexture(_KernelGetRFloat, SHADERPROP_TEXT_IN_RFLOAT, _3dTexture);
            _ReadWriteShader.SetBuffer(_KernelGetRFloat, SHADERPROP_SIZE, _SizeBuffer);
            _ReadWriteShader.SetBuffer(_KernelGetRFloat, SHADERPROP_OUT_RFLOAT, _DataBuffer);
            _ReadWriteShader.Dispatch(_KernelGetRFloat, __Size.Width, __Size.Height, __Size.Depth);
        }

        private void PerformWriteRFloatColor() {
            _ReadWriteShader.SetTexture(_KernelSetRFloatColor, SHADERPROP_TEXT_OUT_RFLOAT, _3dTexture);
            _ReadWriteShader.SetBuffer(_KernelSetRFloatColor, SHADERPROP_SIZE, _SizeBuffer);
            _ReadWriteShader.SetBuffer(_KernelSetRFloatColor, SHADERPROP_IN_COLOR, _DataBuffer);
            _ReadWriteShader.Dispatch(_KernelSetRFloatColor, __Size.Width, __Size.Height, __Size.Depth);
        }

        private void PerformReadRFloatColor() {
            _ReadWriteShader.SetTexture(_KernelGetRFloatColor, SHADERPROP_TEXT_IN_RFLOAT, _3dTexture);
            _ReadWriteShader.SetBuffer(_KernelGetRFloatColor, SHADERPROP_SIZE, _SizeBuffer);
            _ReadWriteShader.SetBuffer(_KernelGetRFloatColor, SHADERPROP_OUT_COLOR, _DataBuffer);
            _ReadWriteShader.Dispatch(_KernelGetRFloatColor, __Size.Width, __Size.Height, __Size.Depth);
        }

        private void SetupShaderTextureAndKernels() {
            SetupShader();
            SetupTextureAndKernelByFormat();
        }

        private void SetupTextureAndKernelByFormat() {
            switch(_Format) {
                case TextureFormat.FloatR: SetupTextureAndKernelFloatR(); break;
                case TextureFormat.FloatRGBA: SetupTextureAndKernelRGBA(); break;
                default:
                    Logging.Warning<Texture3DCompute>("Unsupported format: {0}", _Format.ToString());
                    break;
            }
        }

        private void SetupSizeBuffer(int width, int height, int depth) {
            _SizeBuffer = ComputeBufferUtil.CreateBufferForStruct<HlslSizeType>(__Size);
        }

        private void SetupDataBuffer() {
            if (_IsReady) {
                _DataBuffer = ComputeBufferUtil.CreateBufferForSimpleArray<float>(_TotalDataSize);
            } else {
                Logging.Warning<Texture3DCompute>("Not ready; failed to setup data buffer!");
            }
        }

        private void SetupColorsBuffer() {
            if (_IsReady) {
                _ColorsBuffer = ComputeBufferUtil.CreateBufferForSimpleArray<Color>(_TotalDataSize);
            } else {
                Logging.Warning<Texture3DCompute>("Not ready; cannot create color buffer!");
            }
        }

        private void SetupTextureAndKernelFloatR() {
            _3dTexture = RenderTextureUtil.Create3D(__Size.Width, __Size.Height, __Size.Depth, RenderTextureFormat.RFloat, true);
            _KernelGetRFloat = _ReadWriteShader.FindKernelOrWarn(KERNELGET_RFLOAT, ref _IsReady);
            _KernelSetRFloat = _ReadWriteShader.FindKernelOrWarn(KERNELSET_RFLOAT, ref _IsReady);
            _KernelGetRFloatColor = _ReadWriteShader.FindKernelOrWarn(KERNELGET_RFLOATCOLOR, ref _IsReady);
            _KernelSetRFloatColor = _ReadWriteShader.FindKernelOrWarn(KERNELSET_RFLOATCOLOR, ref _IsReady);
        }

        private void SetupTextureAndKernelRGBA() {
            _3dTexture = RenderTextureUtil.Create3D(__Size.Width, __Size.Height, __Size.Depth, RenderTextureFormat.ARGBFloat, true);
            _KernelRGBA = _ReadWriteShader.FindKernelOrWarn(SHADERKERNEL_RGBA, ref _IsReady);
        }

        private void SetupShader() {
            _ReadWriteShader = ComputeShaderUtil.FindComputeOrWarn(SHADERPATH, out _IsReady);
        }
    }

}

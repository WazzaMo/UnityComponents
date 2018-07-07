/*
 * ThreeDTextureText Unity Component
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
using Actor.Shader;

namespace ActorKitExample.ComputeShaders {

    public class ThreeDTextureTest : MonoBehaviour {
        const string
            SHADERPROP_SPACE = "Output",
            SHADERPROP_InValues = "InValues",
            SHADERPROP_SIZE = "SpaceSize",
            SHADERPROP_TIME = "Time",
            SHADERKERNEL = "Life3D",
            SHADERPATH = "Texture3DLife";

        const int
            SHADERTHREAD_X = 8,
            SHADERTHREAD_Y = 8,
            SHADERTHREAD_Z = 1;

        public int
            _Width = 3, 
            _Height = 3, 
            _Depth = 3;

        private uint3 __SpaceSize;
        private ComputeBuffer _SpaceSizeBuffer;
        private RenderTexture _3DData;
        private ComputeBuffer _InputData;

        private ComputeShader _Shader;
        private int _Kernel;

        private VolViewCosmos _VolView;
        private System.Random _Seed;
        private bool _IsReady;

        private static Color[] __Volume = null;

        private void Start() {
            Setup();
        }

        private void Update() {
            if (_IsReady) {
                RunShader();
                Color[] data = GetTextureData(_3DData);
                if (data != null) {
                    _VolView.RenderVolumeView(data);
                }
            }
        }

        private static Color[] GetTextureData(RenderTexture text3d) {
            Color[] result = null;
            if (text3d.dimension == UnityEngine.Rendering.TextureDimension.Tex3D) {
                HlslSizeType size = new HlslSizeType(text3d.width, text3d.height, text3d.volumeDepth);
                
                if (__Volume == null) {
                    __Volume = new Color[size.Width * size.Height * size.Depth];
                }

                if (TextureUtil.TryRead1Float3DTexture(text3d, __Volume)) {
                    result = __Volume;
                }
            }
            return result;
        }

        private void OnDestroy() {
            DisposablesUtil.DisposeAll(_SpaceSizeBuffer, _InputData);
            _3DData.Release();
        }

        private void RunShader() {
            if (_IsReady) {
                _Shader.SetTexture(_Kernel, SHADERPROP_SPACE, _3DData);
                _Shader.SetBuffer(_Kernel, SHADERPROP_InValues, _InputData);
                _Shader.SetFloat(SHADERPROP_TIME, Time.realtimeSinceStartup);
                _Shader.SetBuffer(_Kernel, SHADERPROP_SIZE, _SpaceSizeBuffer);

                int x, y, z;
                CalcDispatchSize(out x, out y, out z);
                _Shader.Dispatch(_Kernel, x, y, z);
            }
        }

        private void CalcDispatchSize(out int x, out int y, out int z) {
            Func<int, int, int> calc = CalcThread;
            x = calc(_Width, SHADERTHREAD_X);
            y = calc(_Height, SHADERTHREAD_Y);
            z = calc(_Depth, SHADERTHREAD_Z);
        }

        private static int CalcThread(int total, int group) {
            int remainder = total % group > 0 ? 1 : 0;
            return remainder + (total / group);
        }

        private void Setup() {
            _Seed = new System.Random();
            _VolView = new VolViewCosmos(_Width, _Height, _Depth);
            Setup3DSpace();
            SetupShader();
        }

        private void SetupShader() {
            _Shader = ComputeShaderUtil.FindComputeOrWarn(SHADERPATH, out _IsReady);
            if (_IsReady) {
                _Kernel = _Shader.FindKernelOrWarn(SHADERKERNEL, ref _IsReady);
            }
        }

        private void Setup3DSpace() {
            CreateSpaceSize();
            Create3DSpace();
            CreateRandom3D();
        }

        private void CreateSpaceSize() {
            __SpaceSize = new uint3((uint)_Width, (uint)_Height, (uint)_Depth);
            _SpaceSizeBuffer = ComputeBufferUtil.CreateBufferForStruct<uint3>(__SpaceSize);
        }

        private void Create3DSpace() {
            _3DData = RenderTextureUtil.Create3D(_Width, _Height, _Depth, RenderTextureFormat.ARGB32);
        }

        private void CreateRandom3D() {
            Color[] pixels = new Color[_Width * _Height * _Depth];
            Randomize(pixels);
            _InputData = ComputeBufferUtil.CreateBufferForSimpleArray<Color>(pixels);
        }

        private void Randomize(Color[] values) {
            Color color;

            for(int x = 0; x < _Width; x++) {
                for(int y = 0; y < _Height; y++) {
                    for(int z = 0; z < _Depth; z++) {
                        color = RandomColor();
                        TextureUtil.SetPixel(_Width, _Height, values, x, y, z, color);
                    }
                }
            }
        }

        private Color RandomColor() {
            var scale = (float) _Seed.NextDouble();
            Color redScale = new Color(scale, 0, 0);
            return redScale;
        }
    }

}

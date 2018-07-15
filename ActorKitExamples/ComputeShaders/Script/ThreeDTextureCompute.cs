/*
 * ThreeDTextureCompute Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Tools.Common;
using Actor.Shader;

namespace ActorKitExample.ComputeShaders {

    public class ThreeDTextureCompute : MonoBehaviour {
        const string
            SHADERPROP_VOLUME = "Volume",
            SHADERPROP_SIZE = "Size",
            SHADERPROP_TIME = "Time",
            SHADERKERNEL = "SwirlKernel",
            SHADERPATH = "Swirl";

        const int
            SHADERTHREAD_X = 1,
            SHADERTHREAD_Y = 1,
            SHADERTHREAD_Z = 1;

        public int
            _Width = 3, 
            _Height = 3, 
            _Depth = 3;

        public Material _Material = null;

        private Texture3DCompute _3D;

        private VolViewCosmos _VolView;
        private System.Random _Seed;
        private bool _IsReady;

        private HlslSizeType __Size;
        private ComputeBuffer _SizeBuffer;
        private ComputeShader _Shader;
        private float[] _Values;
        private Color[] _Colors;
        private int _Kernel;

        private void Start() {
            Setup();
            this.AreAllEditorValuesConfigured(_Material);
        }

        private void Update() {
            if (_IsReady) {
                Compute();
                RenderVolumeData();
            }
        }

        private void RenderVolumeData() {
            var ok = _3D.TryGetPixelsRFloat(_Values);
            if (ok) {
                SyncColors();
                _VolView.RenderVolumeView(_Colors);
            }
        }

        private void OnDestroy() {
            DisposablesUtil.DisposeAll(_3D, _SizeBuffer);
        }

        private void Compute() {
            int xThread, yThread, zThread;
            CalcDispatchSize(out xThread, out yThread, out zThread);
            _Shader.SetFloat(SHADERPROP_TIME, Time.realtimeSinceStartup);
            _Shader.SetBuffer(_Kernel, SHADERPROP_SIZE, _SizeBuffer);
            _Shader.SetTexture3D(_Kernel, SHADERPROP_VOLUME, _3D);
            _Shader.Dispatch(_Kernel, xThread, yThread, zThread);
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
            _VolView = new VolViewCosmos(_Width, _Height, _Depth, _Material);
            SetupShaderAndKernel();
            SetupSize();
            Setup3DSpace();
        }

        private void SetupShaderAndKernel() {
            _Shader = ComputeShaderUtil.FindComputeOrWarn(SHADERPATH, out _IsReady);
            _Kernel = _Shader.FindKernelOrWarn(SHADERKERNEL, ref _IsReady);
        }

        private void SetupSize() {
            __Size = new HlslSizeType(_Width, _Height, _Depth);
            _SizeBuffer = ComputeBufferUtil.CreateBufferForStruct<HlslSizeType>(__Size);
        }

        private void Setup3DSpace() {
            _3D = new Texture3DCompute(_Width, _Height, _Depth, Texture3DCompute.TextureFormat.FloatR);
            CreateStorageForVolume();
            CreateRandom3D();
        }

        private void CreateStorageForVolume() {
            _Values = new float[__Size.CalcTotalSize()];
            _Colors = new Color[__Size.CalcTotalSize()];
        }

        private void SyncColors() {
            float value;
            for(int index = 0; index < _Values.Length; index++) {
                value = _Values[index];
                _Colors[index] = new Color(value, value, value, value);
            }
        }

        private void CreateRandom3D() {
            Randomize();
            _3D.SetPixelsRFloat(_Values);
        }

        private void Randomize() {
            for(int x = 0; x < _Width; x++) {
                for(int y = 0; y < _Height; y++) {
                    for(int z = 0; z < _Depth; z++) {
                        _Values[_3D.Pos(x, y, z)] = RandomFloat();
                    }
                }
            }
        }

        private float RandomFloat() {
            var value = (float)_Seed.NextDouble();
            return value;
        }
    }

}

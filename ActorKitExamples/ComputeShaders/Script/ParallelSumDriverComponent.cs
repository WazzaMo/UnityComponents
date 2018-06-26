/*
 * Example Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Tools.Common;
using Actor.Shader;

namespace ActorKitExamples.ComputeShaders.Script {

    public class ParallelSumDriverComponent : MonoBehaviour {
        const string
            SHADER_PATH = "ParallelSum",
            KERNEL_NAME = "ParallelSumCS",
            BUFFER_VALUESIN = "valuesIn",
            BUFFER_RESULTOUT = "sumOut";

        public float[] __ValuesIn = null;
        public Text _Output = null;

        private ComputeShader _Shader;
        private int _SumKernel;
        private float __SumOut;
        private ComputeBuffer _ValuesInBuffer;
        private ComputeBuffer _SumOutBuffer;
        private bool _IsReady;

        private void Start() {
            SetupShader();
            CheckOutput();
            SetupValuesBuffer();
            SetupSumOut();
        }

        public void Run() {
            if (_IsReady) {
                _Shader.Dispatch(_SumKernel, 1, 1, 1);
                float[] sumVal = new float[1];
                _SumOutBuffer.GetData(sumVal);
                _Output.text = sumVal[0].ToString();
            }
        }

        private void OnDestroy() {
            DisposablesUtil.DisposeAll(_ValuesInBuffer, _SumOutBuffer);
        }

        private void SetupShader() {
            _Shader = ComputeShaderUtil.FindComputeOrWarn(SHADER_PATH, out _IsReady);
            _SumKernel = _Shader.FindKernelOrWarn(KERNEL_NAME, ref _IsReady);
        }

        private void SetupValuesBuffer() {
            _IsReady = _IsReady && __ValuesIn != null;
            if (_IsReady) {
                _ValuesInBuffer = ComputeBufferUtil.CreateBufferForSimpleArray<float>(__ValuesIn);
                _Shader.SetBuffer(_SumKernel, BUFFER_VALUESIN, _ValuesInBuffer);
            }
        }

        private void SetupSumOut() {
            __SumOut = 0f;
            if (_IsReady) {
                _SumOutBuffer = ComputeBufferUtil.CreateBufferForStruct<float>(__SumOut);
                _Shader.SetBuffer(_SumKernel, BUFFER_RESULTOUT, _SumOutBuffer);
            }
        }

        private void CheckOutput() {
            this.AreAllEditorValuesConfigured(ref _IsReady, _Output);
        }
    }

}

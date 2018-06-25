/*
 * Example Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Tools.Common;

using Actor.Shader;

namespace ActorKitExample.ComputeShaders {

    public class ThreadOrderTest : MonoBehaviour {
        const uint
            THREAD_GROUP_SIZE = 8,
            TOTAL_OPERATIONS = 16;

        const string
            ORDER_TEST_SHADER_PATH = "TestInvokationOrder",
            ORDER_TEST_KERNEL = "CSMain",
            BUFFER_THREAD_SEQUENCE = "ThreadSequence",
            BUFFER_GROUP_SEQUENCE = "GroupSequence";

        public Text _OutputText = null;
        public Button _StartButton = null;

        private ComputeShader _OrderTestShader;
        private ComputeBuffer _ThreadSequenceBuffer;
        private ComputeBuffer _GroupSequenceBuffer;
        private int _OrderTestKernel;

        private uint[] __ThreadSequence;
        private uint[] __GroupSequence;

        private bool _IsReady = false;

        void Start() {
            SetupRawValues();
            SetupBuffersFromRaw();
            SetupShader();
            SetupUiElements();
        }

        void Update() {

        }

        private void OnDestroy() {
            DisposablesUtil.DisposeAll(_ThreadSequenceBuffer, _GroupSequenceBuffer);
        }

        private void SetupRawValues() {
            __ThreadSequence = new uint[TOTAL_OPERATIONS];
            __GroupSequence = new uint[TOTAL_OPERATIONS];

            ZeroOutData(__ThreadSequence);
            ZeroOutData(__GroupSequence);
        }

        private void SetupBuffersFromRaw() {
            _ThreadSequenceBuffer = ComputeBufferUtil.CreateBufferForSimpleArray<uint>(__ThreadSequence);
            _GroupSequenceBuffer = ComputeBufferUtil.CreateBufferForSimpleArray<uint>(__GroupSequence);
        }

        private void SetupShader() {
            _OrderTestShader = ShaderExt.GetComputeShader(ORDER_TEST_SHADER_PATH);
            _OrderTestKernel = _OrderTestShader.FindKernel(ORDER_TEST_KERNEL);
            _OrderTestShader.SetBuffer(_OrderTestKernel, BUFFER_THREAD_SEQUENCE, _ThreadSequenceBuffer);
            _OrderTestShader.SetBuffer(_OrderTestKernel, BUFFER_GROUP_SEQUENCE, _GroupSequenceBuffer);
        }

        private void SetupUiElements() {
            _IsReady = this.AreAllEditorValuesConfigured(_OutputText, _StartButton);
            if (_IsReady) {
                _StartButton.onClick.AddListener(OnButton);
            }
        }

        private void OnButton() {
            if (_IsReady) {
                RunTheShader();
                ShowResult();
            }
        }

        private void RunTheShader() {
            uint numGroups =  TOTAL_OPERATIONS / THREAD_GROUP_SIZE;

            _OrderTestShader.Dispatch(_OrderTestKernel, (int) numGroups, 1, 1);
        }

        private void ShowResult() {
            uint[] data;
            string message = "";

            data = ComputeBufferUtil.GetBufferDataArray<uint>(_ThreadSequenceBuffer);
            message = "Thread Sequence : " + GetArrayAsText(data);
            data = ComputeBufferUtil.GetBufferDataArray<uint>(_GroupSequenceBuffer);
            message += "\nGroup Sequence  : " + GetArrayAsText(data);

            _OutputText.text = message;
        }


        private string GetArrayAsText<T>(T[] data) {
            if (data != null) {
                if (data.Length == 0) {
                    return "[]";
                } else {
                    StringBuilder builder = new StringBuilder("[");
                    for(int index = 0; index < data.Length; index++) {
                        if (index < (data.Length - 1)) {
                            builder.AppendFormat("{0}, ", data[index]);
                        } else {
                            builder.Append(data[index]);
                        }
                    }
                    builder.Append(']');
                    return builder.ToString();
                }
            }
            return "NULL";
        }

        private void ZeroOutData(uint3[] data) {
            for(uint index = 0; index < data.Length; index++) {
                data[index] = uint3.ZERO;
            }
        }

        private void ZeroOutData(uint[] data) {
            for (uint index = 0; index < data.Length; index++) {
                data[index] = 0;
            }
        }
    }

}
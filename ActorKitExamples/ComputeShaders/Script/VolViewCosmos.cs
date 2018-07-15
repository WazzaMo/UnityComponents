/*
 * ThreeDTextureText Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Tools.Common;

namespace ActorKitExample.ComputeShaders {

    public class VolViewCosmos {
        const string INSTANCE_SHADER = "UnityComponents/InstancedColorShader";

        private Material _Material;
        private bool _IsReady;
        private GameObject[] _Objects;
        private MeshRenderer[] _Renderers;
        private int _Width, _Height, _Depth;

        public VolViewCosmos(int width, int height, int depth, Material material = null) {
            _Width = width;
            _Height = height;
            _Depth = depth;
            _IsReady = true;
            if (material == null) {
                SetupMaterial();
            } else {
                _Material = material;
            }
            if (_IsReady) {
                Spawn();
                GetRenderersAndConfigureMaterial();
            }
        }

        public void RenderVolumeView(Color[] colors) {
            if (_IsReady) {
                Render(colors, _Width, _Height, _Depth);
            }
        }

        private void Render(Color[] colors, int width, int height, int depth) {
            int index = 0;
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            for(int z = 0; z < depth; z++) {
                for (int x = 0; x < width; x++) {
                    for(int y = 0; y < height; y++) {
                        index = PosIndex(x, y, z);
                        AssignRenderProperties(index, props, colors[index]);
                    }
                }
            }
        }

        private int PosIndex(int x, int y, int z) {
            return _Width * (y + z * _Height) + x;
        }

        private void AssignRenderProperties(int index, MaterialPropertyBlock propBlock, Color color) {
            propBlock.SetColor("_Color", color);
            _Renderers[index].SetPropertyBlock(propBlock);
        }

        private void SetPosition(int index, Vector3 pos) {
            GameObject gameObject = _Objects[index];
            gameObject.transform.position = pos;
        }

        private void SetupMaterial() {
            Shader shader = ShaderExt.FindOrWarn(INSTANCE_SHADER, ref _IsReady);
            _Material = _IsReady ? new Material(shader) : null;
        }

        private void Spawn() {
            _Objects = new GameObject[_Width * _Height * _Depth];
            for(int index = 0; index < _Objects.Length; index++) {
                _Objects[index] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }
            SetPositionForAllObjects();
        }

        private void SetPositionForAllObjects() {
            Vector3 pos;
            int index = 0;

            for (int z = 0; z < _Depth; z++) {
                for (int x = 0; x < _Width; x++) {
                    for (int y = 0; y < _Height; y++) {
                        pos = new Vector3(x, y, z);
                        index = PosIndex(x, y, z);
                        SetPosition(index, pos);
                    }
                }
            }
        }

        private void GetRenderersAndConfigureMaterial() {
            _Renderers = _Objects.Select(obj => obj.GetComponent<MeshRenderer>()).ToArray();
            _Renderers.ForEach(renderer => renderer.sharedMaterial = _Material);
        }
    }

}

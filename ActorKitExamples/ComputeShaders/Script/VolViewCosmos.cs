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

        public VolViewCosmos(int width, int height, int depth) {
            _Width = width;
            _Height = height;
            _Depth = depth;
            _IsReady = true;
            SetupMaterial();
            if (_IsReady) {
                Spawn(width, height, depth);
                GetRenderersAndConfigureMaterial();
            }
        }

        public void RenderVolumeView(Color[] colors) {
            if (_IsReady) {
                Render(colors, _Width, _Height, _Depth);
            }
        }

        private void Render(Color[] colors, int width, int height, int depth) {
            Vector3 pos;
            int index = 0;

            for(int z = 0; z < depth; z++) {
                for (int x = 0; x < width; x++) {
                    for(int y = 0; y < height; y++) {
                        pos = new Vector3(x, y, z);
                        RenderObject(index, pos, colors[PosIndex(x, y, z)]);
                        index++;
                    }
                }
            }
        }

        private int PosIndex(int x, int y, int z) {
            return _Width * (y + z * _Height) + x;
        }

        private void RenderObject(int index, Vector3 pos, Color color) {
            MaterialPropertyBlock prop = new MaterialPropertyBlock();
            prop.SetColor("_Color", color);
            GameObject gameObject = _Objects[ index ];
            MeshRenderer renderer = _Renderers[index];
            renderer.SetPropertyBlock(prop);
            gameObject.transform.position = pos;
        }

        private void SetupMaterial() {
            Shader shader = ShaderExt.FindOrWarn(INSTANCE_SHADER, ref _IsReady);
            _Material = _IsReady ? new Material(shader) : null;
        }

        private void Spawn(int width, int height, int depth) {
            _Objects = new GameObject[width * height * depth];
            for(int index = 0; index < _Objects.Length; index++) {
                _Objects[index] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }
        }

        private void GetRenderersAndConfigureMaterial() {
            _Renderers = _Objects.Select(obj => obj.GetComponent<MeshRenderer>()).ToArray();
            _Renderers.ForEach(renderer => renderer.sharedMaterial = _Material);
        }
    }

}

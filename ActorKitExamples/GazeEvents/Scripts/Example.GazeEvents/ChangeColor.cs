/*
 * Example Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;

using UnityEngine;


using Tools.Common;

namespace Example.GazeEvents {

    public class ChangeColor : MonoBehaviour {
        private System.Random _Random;
        private MeshRenderer _Renderer;
        private bool _IsReady = true;

	    void Start () {
            _Random = new System.Random();
            _Renderer = this.GetComponentOrWarn<MeshRenderer>(ref _IsReady);
            CheckRenderer();
	    }
	
	    public void ChangeToRandomColor() {
            if (_IsReady) {
                SetColor(RandomColor());
            } else {
                Logging.Log<ChangeColor>("Not ready");
            }
        }

        private void CheckRenderer() {
            if (_Renderer != null) {
                if (_Renderer.sharedMaterial == null) {
                    var mat = new Material(Shader.Find("Standard"));
                    _Renderer.sharedMaterial = mat;
                }
            }
        }

        private void SetColor(Color color) {
            //Logging.Log<ChangeColor>("setting color {0}", color);
            _Renderer.sharedMaterial.color = color;
        }

        private Color RandomColor() {
            return new Color(RandomFloat(), RandomFloat(), RandomFloat(), 1);
        }

        private float RandomFloat() {
            return (float) _Random.NextDouble();
        }
    }

}

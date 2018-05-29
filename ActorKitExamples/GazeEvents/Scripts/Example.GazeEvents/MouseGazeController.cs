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

    public class MouseGazeController : MonoBehaviour {
        public float horizontalSpeed = 10f;
        public float verticalSpeed = 8f;

        private Transform _Transform;

	    void Start () {
            Setup();
	    }
	
	    void Update () {
            RotateCamOnMouseAction();
	    }

        private void Setup() {
            _Transform = GetComponent<Transform>();
        }

        private void RotateCamOnMouseAction() {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            _Transform.Rotate(v, h, 0);
        }
    }

}

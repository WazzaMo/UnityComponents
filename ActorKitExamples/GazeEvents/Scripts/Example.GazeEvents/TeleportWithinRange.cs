/*
 * Example Code
 * --------------
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example.GazeEvents {

    public class TeleportWithinRange : MonoBehaviour {
        public float _MinRadius = 5f;
        public float _MaxRadius = 10f;

        private System.Random _Random;
        private Transform _Transform;

	    void Start () {
            Setup();
	    }

        public void PerformTeleport() {
            _Transform.position = RandomPos();
        }
	
        private void Setup() {
            _Random = new System.Random();
            _Transform = GetComponent<Transform>();
        }

        private Vector3 RandomPos() {
            var pos = RandomOrientation() * new Vector3(RandomRadius(), 0, 0);
            return pos;
        }

        private float RandomRadius() {
            float factor = _MaxRadius - _MinRadius;
            return _MinRadius + ((float)_Random.NextDouble() * _MaxRadius);
        }

        private Quaternion RandomOrientation() {
            var value = Quaternion.Euler(RandomDegrees(), RandomDegrees(), RandomDegrees());
            return value;
        }

        private float RandomDegrees() {
            return (float)_Random.NextDouble() * 360f;
        }
    }

}

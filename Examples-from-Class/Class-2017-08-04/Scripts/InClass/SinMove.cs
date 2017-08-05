/*
 * Written in class on 2017-08-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * According to a Sin Wave move a gameobject from a lower boundary to an upper boundary.
 * NOTE: Sin takes us from -1 to +1 over a 360 degree range
 * 
 * Over time, the component will change:
 * - the height of the game object via the transform's position
 * - the height will be calculated from this formula
 *   heigh = 5 * sin(angle)
 * 
 */

namespace InClass {

    public class SinMove : MonoBehaviour {
        [SerializeField]
        [Range(0f, 2f)]
        private float AngularSpeed = 0.2f;

        [SerializeField]
        private Direction MovementDirection = Direction.Vertical;

        private float _angle;
        private Transform _transform;


        // Use this for initialization
        void Start() {
            _angle = 0;
            _transform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update() {
            _angle = _angle + AngularSpeed;
            Vector3 currentPosition = _transform.position;
            if (MovementDirection == Direction.Vertical) {
                currentPosition.y = GetHeight();
            } else if (MovementDirection == Direction.Horizontal) {
                currentPosition.x = GetHeight();
            }
            _transform.position = currentPosition;
        }

        private float GetHeight() {
            return 5 * Mathf.Sin(_angle);
        }

    }

}
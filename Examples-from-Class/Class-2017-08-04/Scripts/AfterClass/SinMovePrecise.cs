/*
 * Updated After class on 2017-08-04
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
 *   height = 5 * sin(angle)
 * 
 * Angular Speed is in radians per second
 * so as a basis, let's use 2xPI as a single loop
 * 
 * Time is measured between update cycles using Time.delta time
 * Let's say I want to be able to set the movement speed in loops per second.
 * Now, let's figure out how to calculate the amount of extra angle to add
 * to use with the call to Sin() to get the next position...
 * 
 * (1)  Start with the angle to add for 1 second
 * 
 *      Change In Angle to make a full loop = 2 x PI x Number_loops_per_second
 *          [why? because 2 PI in radians = 360 in degrees ]
 * 
 *      Change in time = 1 second
 * 
 * (2)  Now extend the idea to figure out part angle for the part time
 *      between Update() calls that Unity will make.
 * 
 *      angle_difference = time_difference x ( 2 x Pi ) x Number_loops_per_second
 *      
 * (3)  The other change I'm making here is to rename the GetHeight()
 *      function because we are now using it for a position.
 */

using InClass;

namespace AfterClass {

    public class SinMovePrecise : MonoBehaviour {
        [SerializeField]
        [Range(0f, 20f)]
        private float LoopSpeed = 1f;  // Loops per second

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
            _angle = _angle + CalculateAngularIncrement();
            Vector3 currentPosition = _transform.position;
            if (MovementDirection == Direction.Vertical) {
                currentPosition.y = GetMovement();
            } else if (MovementDirection == Direction.Horizontal) {
                currentPosition.x = GetMovement();
            }
            _transform.position = currentPosition;
        }

        private float CalculateAngularIncrement() {
            return ONE_FULL_REVOLUTION * LoopSpeed * Time.deltaTime;
        }

        const float ONE_FULL_REVOLUTION = Mathf.PI * 2;

        private float GetMovement() {
            return 5 * Mathf.Sin(_angle);
        }

    }

}
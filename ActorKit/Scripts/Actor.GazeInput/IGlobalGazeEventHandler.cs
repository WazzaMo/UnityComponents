/*
 * IGlobalGazeEventHandler Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using UnityEngine;

namespace Actor.GazeInput {

    public interface IGlobalGazeEventHandler {
        void OnGazeUpdate(GazeData data);
    }

}

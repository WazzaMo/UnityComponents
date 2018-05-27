/*
 * IGazeEventHandler Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using Actor.GazeInput;

namespace Actor.GazeInput {

    public interface IGazeEventHandler {
        void OnGazeEnter(GazeData data);
        void OnGazeLeave(GazeData data);
        void OnGazeContinue(GazeData data);
        void OnGazeClick(GazeData data);
    }

}

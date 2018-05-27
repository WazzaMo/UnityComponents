/*
 * GazeEventSource Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;


namespace Actor.GazeInput {

    public interface IGazeEventBroadcaster {
        void RegisterGlobalHandlers(List<IGlobalGazeEventHandler> handlers);
        void BroadcastGazeEvents(List<GazeData> events);
    }

}

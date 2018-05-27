/*
 * SimpleGazeEventBroadcaster Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tools.Common;

namespace Actor.GazeInput.Implementation {

    public class SimpleGazeEventBroadcaster : IGazeEventBroadcaster {
        public void RegisterGlobalHandlers(List<IGlobalGazeEventHandler> handlers) {
        }

        public void BroadcastGazeEvents(List<GazeData> eventList) {
            eventList
                .Where(data => data.GazeHandler != null)
                .ForEach(dataEvent => BroadCast(dataEvent.GazeHandler, dataEvent));
        }

        private void BroadCast(IGazeEventHandler handler, GazeData data) {
            switch(data.EventKind) {
                case GazeEventKind.NoEvent: break;
                case GazeEventKind.OnGazeClick: handler.OnGazeClick(data); break;
                case GazeEventKind.OnGazeEnter: handler.OnGazeEnter(data); break;
                case GazeEventKind.OnGazeExit: handler.OnGazeLeave(data); break;
                case GazeEventKind.OnGazeStay: handler.OnGazeContinue(data); break;
                default:
                    Logging.Log<SimpleGazeEventBroadcaster>("EventKind {0} was not handled", data.EventKind);
                    break;
            }
        }
    }
}

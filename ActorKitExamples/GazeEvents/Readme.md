# GazeInput and Events Example

## Overview
Gaze Events are an independent event system
that allow gaze based gestures and control
for VR, first-person characters or possibly AR.

## Dependency Injection
Dependencies are injected by adding components
to GameObjects to generate events and processing
of those events. Generally, I suggest adding them
to the main camera.

## Handling Events in Your App
In the Example.GazeEvents namespace there's a class
`TeleportWithinRange` that has a regular function
that will teleport the object it is attached to within
a spherical range. The example associates the gaze event
with the teleport function using the `GazeEventHandler_Proxy`
that proxies events to function calls.

## Extensible
The interpretation of raw gaze data into
a usable gaze event is done through a gaze policy.
The library supplies components to 
See the IGazeEventPolicy interface for what's
required of a gaze policy.

### GazePolicy_EnterExit
`GazePolicy_EnterExit` component can be attached to the
camera to convert raw gaze events into enter and leave events.  That is, it detects whent he user is gazing on a new
object of if the focus has wandered away.

### GazePolicy_StayAndClick
`GazePolicy_StayAndClick` component should be attached to the
camera to convert raw gazes into gaze-stay events and after
the defined time elapses, a gaze click event is raised and
 the timer resets. If gazing at the same object for a long
 time, multiple clicks will register over the period.
 

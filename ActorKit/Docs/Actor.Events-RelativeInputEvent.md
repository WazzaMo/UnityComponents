# RelativeInput Event

## Purpose
A RelativeInputEvent passes a value between 0 and 1 for positive-range or -1 to 0 to +1 for full range. The event is sent from a relative input source to a relative input consumer.
The consumer will determine what the relative input means - say a distance between two points
or progress along a way point track.

To work, like other UnityEvent types, this will require at least one GameObject with an EventSystem component in the Scene.  EventSystems can be created from the Unity menu
for GameObjects | UI | EventSystem.

## Inspector Values
| Setting | Purpose                          |
|---------|----------------------------------|
| Event | The event entry in the Inspector will have a `+` button that can be clicked to create a slot for a GameObject & Component|Function registration. |

## Methods
No specialized instance methods are provided.

## Static Methods
| Method | Description                          |
|---------|----------------------------------|
| `static float WithinFullRange(float value)` | Normalizes the value to be within -1 to +1. |
| `static float WithinPositiveRange(float value)` | Normalizes the value to be within 0 to 1 |

The normalizing approach guarantees the values are within range by returning the original
value if within the range and otherwise returning the closes extreme of the range - that is for
a value of -2, -1 will be returned (also for anything less than -1) and an argument of 3 will return 1 (again any argument > 1 will return 1).

### Copyright
(c) Copyright 2017 Warwick Molloy, may be used under MIT License terms.

# MoodLight Component

## Purpose
When attached to a GameObject that has one of the Unity Light components, will
allow switching to a new color, chosen randomly, from the array of colors configured
in the Inspector.

The `BlendToNextRandomColor()` method can be triggered using a Unity UI button
or can be called from another script component.

## Inspector Values
| Setting | Purpose                          |
|---------|----------------------------------|
|Colors To Choose | An array of colors to randomly select. |
|Blend Speed In Seconds | Time (seconds) blending from one color to the next. |

## Methods
| Method | Description                          |
|---------|----------------------------------|
| `void BlendToNextRandomColor()` | Causes the component to choose another color. |

### Copyright
(c) Copyright 2017 Warwick Molloy, may be used under MIT License terms.

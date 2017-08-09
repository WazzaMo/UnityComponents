# Class 2017-08-04

## Overview
In this class we created a Unity project called 
*MoodLighting* and then we looked at some simple
scripting.

See the `Scripts` directory for the examples
written in class.  The directories are:
- Actor.Environment for a component we discussed (but didn't use)
- InClass for the components we wrote in class
- AfterClass for some enhancements done outside of
class.

## First Example - FlyingCat
We created a script called FlyingCat in Unity
using the *Create | C# Script* menu.

The FlyingCat got a reference to the `MeshRenderer` component and used this to manipulate the attached
material's color.

This demonstrated the `GetComponent<T>()` method that
is provided by the `MonoBehaviour` class.

It also demonstrated making variables available
in the Unity Editor so it's easier to change
things later on in the Scene.  See below for how
we declared the "longer term memory variable" `ColorToUse` that holds the color we choose in Unity.

```cs
using UnityEngine;

public class CanEditInUnity : MonoBehaviour {
    [SerializeField]
    private Color ColorToUse;

    // .... rest of the component goes here ....
}
```

## Second Example - SinMove
We made a component that will move an object using
the Math Sin() operation to calculate a nicely
smoothed motion.

Our in-class version used a radius of 5 and had no
offset so the object will move from -5 to +5 units.

We initially made the object move vertically but
later we added an `enum` called `Direction` so
in Unity Editor we could choose either horizontal
or vertical movement.

## My Homework - After class
The 'AfterClass' directory and namespace
has some extended versions of the SinMove:
- SinMovePrecise
- SinMoveRanged

They use `Time.deltaTime` to make smoother transitions between each Update() call by Unity.
Unity's performance depends on the power of the
computer or device the game is running on.  As a result the time between screen updates changes from moment-to-moment and by device.

Using `Time.deltaTime` allows your components to give
a consistent response in time.

# Package Provided
See the unity package `2017-08-05--MoodLighting.unitypackage` file in
the same directory.

1. Create a new project in Unity
1. Use the menu Assets | Import Package | Custom Package
1. A dialog appears, choose the unitypackage file

This will then import the scripts and scene.

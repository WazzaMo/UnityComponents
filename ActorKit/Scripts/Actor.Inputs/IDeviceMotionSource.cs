/*
 * DeviceMotion Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 * 
 * An internal contract for how motion sources should work
 * within the DeviceMotion Unity component.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Actor.Inputs {

    public interface IDeviceMotionSource {
        bool IsHardwareAvailable { get; }
        bool IsConfiguredCorrectly { get; }

        string GetConfigurationMessage();
        void SetupSource();
        Vector3 GetDeviceDirectionTowardGravity();
    }

}

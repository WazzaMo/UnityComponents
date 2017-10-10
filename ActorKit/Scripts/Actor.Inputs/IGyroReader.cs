using System;

using UnityEngine;



namespace Actor.Inputs {

    public interface IGyroReader {
        Quaternion TakeOrientationReading();
    }

}

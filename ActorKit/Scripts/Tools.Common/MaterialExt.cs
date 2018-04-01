/*
 * ShaderExt Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Common {

    using UnityEngine;

    public static class MaterialExt {
        public static Material MakeMaterialFromShaderOrWarn(Shader shader, ref bool isReady) {
            if (shader != null && isReady) {
                return new Material(shader);
            } else {
                Logging.Warning("{0}: Unable to create Material - given NULL shader or component not ready.", typeof(MaterialExt).Name);
                return null;
            }
        }
    }

}

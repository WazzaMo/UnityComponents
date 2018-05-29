/*
 * ComputeShaderUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


namespace Tools.Common {

    public static class ComputeShaderUtil {
        public static ComputeShader FindComputeOrWarn(string resourcePath, out bool IsReady) {
            var shader = Resources.Load<ComputeShader>(resourcePath);
            if (shader == null) {
                Logging.Warning(typeof(ShaderExt), "Could not load shader: {0}", resourcePath);
            } else {
                Logging.Log(typeof(ShaderExt), "Found compute shader: {0}", resourcePath);
            }
            IsReady = shader != null;
            return shader;
        }
    }

}

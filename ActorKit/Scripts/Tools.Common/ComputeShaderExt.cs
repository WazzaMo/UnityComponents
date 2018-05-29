/*
 * ComputeShaderExt Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tools.Common {

    public static class ComputeShaderExt {
        const string EXTNAME = "ComputeShaderExt";

        public static int FindKernelOrWarn(this ComputeShader compute, string kernalName, ref bool IsReady) {
            if (IsReady) {
                if (compute.HasKernel(kernalName)) {
                    int kernel = compute.FindKernel(kernalName);
                    IsReady = true;
                    return kernel;
                } else {
                    IsReady = false;
                    Logging.Warning("{0}: Can't find Compute Shader Kernel of name {1}", EXTNAME, kernalName);
                    return -1;
                }
            } else {
                Logging.Log("{0}: Not attempting to find Kernel - caller was not ready state!", EXTNAME);
                return -1;
            }
        }
    }

}

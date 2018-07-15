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

using Actor.Shader;

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

        /// <summary>
        /// Convenience extension method that makes Texture3DCompute work just like other textures in terms
        /// of how you set them into shader properties.
        /// </summary>
        /// <param name="compute">ComputeShader reference</param>
        /// <param name="kernel">Kernel ID for the shader</param>
        /// <param name="nameID">Name of the Texture3D property</param>
        /// <param name="texture">Reference to Texture3DCompute object</param>
        public static void SetTexture3D(this ComputeShader compute, int kernel, string nameID, Texture3DCompute texture) {
            if (texture.IsTextureReady && compute != null && nameID != null) {
                texture.SetTextureInto(compute, kernel, nameID);
            } else {
                Logging.Warning(
                    "{0}: texture not in usable state! State: [ready: {1}], Name = {2}",
                    EXTNAME, texture.IsTextureReady, nameID == null ? "NULL" : nameID
                );
            }
        }
    }

}

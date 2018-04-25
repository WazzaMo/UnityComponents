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

    public static class ShaderExt {

        public static Shader FindOrWarn(string shaderName) {
            var shader = Shader.Find(shaderName);
            if (shader == null) {
                Logging.Warning("Unable to find Shader {0}", shaderName);
            }
            return shader;
        }

        public static Shader FindOrWarn(string shaderName, ref bool isReady) {
            if (isReady) {
                var shader = FindOrWarn(shaderName);
                if (shader == null) {
                    isReady = false;
                }
                return shader;
            }
            return null;
        }

        public static Material MakeMaterial(this Shader shader, bool isReady = true, string DebugInfo = "ShaderExt") {
            if (isReady && shader != null) {
                return new Material(shader);
            } else {
                Logging.Warning("{0}: Material could not be created.", DebugInfo);
                return null;
            }
        }

        public static ComputeShader GetComputeShader(string resPath) {
            var shader = Resources.Load<ComputeShader>(resPath);
            if (shader == null) {
                Logging.Warning(typeof(ShaderExt),"Could not load shader: {0}", resPath);
            } else {
                Logging.Log(typeof(ShaderExt), "Found compute shader: {0}", resPath);
            }
            return shader;
        }


        public static void ListAllComputeShaders() {
            ComputeShader[] shaders = Resources.FindObjectsOfTypeAll<ComputeShader>();
            if (shaders != null && shaders.Length > 0) {
                StringBuilder message = new StringBuilder();
                message.AppendFormat("Listing ComputeShaders in Resources folder(s): {0} found", shaders.Length);
                shaders.ForEach(sh => message.AppendFormat("ComputeShader: {0}\n", sh.name));
                Logging.Log(message.ToString());
            } else {
                Logging.Log("No ComputeShaders found in any Resources/ folders in the Unity project");
            }
        }
    }
}

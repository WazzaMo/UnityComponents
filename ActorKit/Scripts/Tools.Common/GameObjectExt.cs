/*
 * GameObjectExt Unity Component
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

    public static class GameObjectExt {
        public static T GetComponentOrWarn<T>(this GameObject behaviour) where T : class {
            var component = behaviour.GetComponent<T>();
            if (component == null) {
                Logging.Warning("{0}: Could not find component of type {1}", behaviour.name, typeof(T).Name);
            }
            return component;
        }

        public static T GetComponentOrWarn<T>(this GameObject behaviour, ref bool isReady) where T : class {
            if (isReady) {
                var component = behaviour.GetComponentOrWarn<T>();
                if (component == null) {
                    isReady = false;
                }
                return component;
            }
            return null;
        }

        public static string NameOrNullString(this GameObject gameObject) {
            if (gameObject == null) {
                return "NULL";
            } else {
                return gameObject.name;
            }
        }

        public static Mesh GetMeshOrWarn(this GameObject gameObject, ref bool isReady) {
            if (isReady) {
                var meshRenderer = gameObject.GetComponentOrWarn<MeshFilter>();
                if (meshRenderer != null) {
                    return meshRenderer.sharedMesh;
                } else {
                    Logging.Warning("{0}: No mesh filter and (thus) no mesh attached.", gameObject.name);
                }
            }
            isReady = false;
            return null;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T: Component {
            T component = gameObject.GetComponent<T>();
            if (component == null) {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T FindComponentInParents<T>(this GameObject gameObject) where T: Component {
            Transform currentParent = gameObject.transform.parent;
            T result = null;
            while(result == null && currentParent != null) {
                result = currentParent.GetComponent<T>();
                currentParent = currentParent.parent;
            }
            if (result == null) {
                Logging.Warning("{0}: Component of type {1} could not be found in any parent game object.", gameObject.name, TypeUtil.NameOf<T>());
            }
            return result;
        }
    }

}

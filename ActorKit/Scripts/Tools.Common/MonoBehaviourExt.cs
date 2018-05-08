/*
 * MonoBehaviourExt Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tools.Common {

    using UnityEngine;

    public static class MonoBehaviourExt {
        public static T GetComponentOrWarn<T>(this MonoBehaviour behaviour) where T : class {
            var component = behaviour.GetComponent<T>();
            if (component == null) {
                Logging.Warning("{0}: Could not find component of type {1}", behaviour.name, typeof(T).Name);
            }
            return component;
        }

        public static T GetOrAddComponent<T>(this MonoBehaviour behaviour) where T : Component {
            var component = behaviour.GetComponent<T>();
            if (component == null) {
                component = behaviour.gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T GetComponentNullSafe<T>(this MonoBehaviour behaviour) where T : class {
            if (behaviour == null) {
                Logging.Warning("GetComponentNullSafe<{0}>() Called with NULL component reference", typeof(T).Name);
                return null;
            } else {
                var component = behaviour.GetComponent<T>();
                if (component == null) {
                    Logging.Warning("{0}: could not find component of type {1}", behaviour.name, typeof(T).Name);
                }
                return component;
            }
        }

        public static T GetChildComponentNullSafe<T>(this MonoBehaviour behaviour) where T : class {
            if (behaviour == null) {
                Logging.Warning("GetChildComponentNullSafe<{0}>() Called with NULL component reference", typeof(T).Name);
                return null;
            } else {
                var component = behaviour.GetComponentInChildren<T>();
                if (component == null) {
                    Logging.Warning("{0}: could not find child component of type {1}", behaviour.name, typeof(T).Name);
                }
                return component;
            }
        }

        public static T GetComponentOrWarn<T>(this MonoBehaviour behaviour, ref bool isReady) where T:class {
            if (isReady) {
                var component = behaviour.GetComponentOrWarn<T>();
                if (component == null) {
                    isReady = false;
                }
                return component;
            }
            return null;
        }

        public static bool AreAllEditorValuesConfigured(this MonoBehaviour component, ref bool isReady, params UnityEngine.Object[] values) {
            bool allConfigured = isReady;
            var publicMembers = component.GetType().GetFields().Where(field => field.IsPublic).ToArray();

            values.ForEach((editorValue, index) => allConfigured = allConfigured && CheckEditorValueForNull(component, publicMembers, editorValue, index));
            if (!allConfigured) {
                Logging.Warning("{0}: Editor parameters missing!", component.name);
            }
            isReady = allConfigured;
            return allConfigured;
        }

        public static bool AreAllEditorValuesConfigured(this MonoBehaviour component, params UnityEngine.Object[] values) {
            bool allConfigured = true;
            AreAllEditorValuesConfigured(component, ref allConfigured, values);
            return allConfigured;
        }

        public static T GetComponentInScene<T>(this MonoBehaviour component) where T : MonoBehaviour {
            T other = MonoBehaviour.FindObjectOfType<T>();
            if (other == null) {
                Logging.Warning("{0}: Unable to find component in Scene of type {1}", component.name, typeof(T).Name);
            }
            return other;
        }

        public static T GetComponentInScene<T>(this MonoBehaviour component, ref bool isReady) where T : MonoBehaviour {
            T other = GetComponentInScene<T>(component);
            if (other == null) {
                isReady = false;
            }
            return other;
        }

        public static T[] GetAllComponentInScene<T>(this MonoBehaviour behaviour) where T : MonoBehaviour {
            T[] allOthers = MonoBehaviour.FindObjectsOfType<T>();
            if (allOthers == null || allOthers.Length == 0) {
                Logging.Warning("{0}: Unable to find any components in Scene of type {1}, expecting more than 1", behaviour.name, typeof(T).Name);
            }
            return allOthers;
        }

        public static T[] GetAllComponentInScene<T>(this MonoBehaviour behaviour, ref bool isReady) where T : MonoBehaviour {
            T[] allOthers = GetAllComponentInScene<T>(behaviour);
            if (allOthers == null || allOthers.Length == 0) {
                isReady = false;
            }
            return allOthers;
        }

        public static T GetFirstMatchingComponentInParentsOrWarn<T>(this Component behaviour) where T : Component {
            Transform currentParent = behaviour.transform.parent;
            T result = null;
            while (result == null && currentParent != null) {
                result = currentParent.GetComponent<T>();
                currentParent = currentParent.parent;
            }
            if (result == null) {
                Logging.Warning("{0}: Component of type {1} could not be found in any parent game object.", behaviour.name, TypeUtil.NameOf<T>());
            }
            return result;
        }

        public static Mesh GetMeshOrWarn(this MonoBehaviour behaviour, ref bool isReady) {
            if (isReady) {
                var meshRenderer = behaviour.GetComponentOrWarn<MeshFilter>();
                if (meshRenderer != null) {
                    return meshRenderer.sharedMesh;
                } else {
                    Logging.Warning("{0}: No mesh filter and (thus) no mesh attached.", behaviour.name);
                }
            }
            isReady = false;
            return null;
        }

        private static bool CheckEditorValueForNull(MonoBehaviour component, FieldInfo[] publicFields, UnityEngine.Object editorValue, int index) {
            if (editorValue == null) {
                if (publicFields != null && publicFields.Length > index) {
                    Logging.Warning("{0}: Editor value missing for {1}", component.name, publicFields[index].Name);
                } else {
                    Logging.Warning("{0}: Editor value number {1} missing a value", component.name, index);
                }
                return false;
            }
            return true;
        }
    }
}

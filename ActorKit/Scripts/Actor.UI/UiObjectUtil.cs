/*
 * UiObjectUtil Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Tools.Common;

namespace Actor.UI {

    public static class UiObjectUtil {
        public static GameObject CreateUiObject<T>(string name, RectTransform parent, Vector2 position, Action<T> setup, bool PositionIsFixed = true) where T: Component {
            GameObject uiThing = CreatePlaceAndParent(name, parent, position, PositionIsFixed);
            T comp = uiThing.AddComponent<T>();
            if (setup != null) {
                setup(comp);
            }
            return uiThing;
        }

        public static GameObject CreateUiObject<T1, T2>(
            string name,
            RectTransform parent,
            Vector2 position,
            Action<T1,T2> setup,
            bool PositionIsFixed = true
        )
            where T1 : Component
            where T2: Component
        {
            GameObject uiThing = CreatePlaceAndParent(name, parent, position, PositionIsFixed);
            T1 comp1 = uiThing.AddComponent<T1>();
            T2 comp2 = uiThing.AddComponent<T2>();
            if (setup != null) {
                setup(comp1, comp2);
            }
            return uiThing;
        }

        public static GameObject CreateUiObject<T1, T2, T3>(
            string name,
            RectTransform parent,
            Vector2 position,
            Action<T1, T2, T3> setup,
            bool PositionIsFixed = true
        )
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            GameObject uiThing = CreatePlaceAndParent(name, parent, position, PositionIsFixed);
            T1 comp1 = uiThing.AddComponent<T1>();
            T2 comp2 = uiThing.AddComponent<T2>();
            T3 comp3 = uiThing.AddComponent<T3>();
            if (setup != null) {
                setup(comp1, comp2, comp3);
            }
            return uiThing;
        }

        private static GameObject CreatePlaceAndParent(string name, RectTransform parent, Vector2 position, bool WorldPosition) {
            string uniqueName = GetUniqueName(parent, name);
            GameObject uiThing = new GameObject() { name = uniqueName };

            RectTransform rectTransform = uiThing.AddComponent<RectTransform>();
            rectTransform.position = position;
            rectTransform.SetParent(parent, worldPositionStays: WorldPosition);
            return uiThing;
        }

        private static string GetUniqueName(RectTransform parent, string name) {
            RectTransform[] children = parent.gameObject.GetComponentsInChildren<RectTransform>();
            var matching = children.Where(child => child.name.StartsWith(name));
            if (matching.Any()) {
                return string.Format("{0} ({1})", name, matching.Count());
            } else {
                return name;
            }
        }
    }

}

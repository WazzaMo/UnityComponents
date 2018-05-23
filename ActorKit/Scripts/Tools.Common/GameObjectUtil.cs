/*
 * GameObjectUtil Unity Component
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tools.Common {

    public static class GameObjectUtil {
        public static IEnumerable<T> GetObjectsInSceneWithImplementationOf<T>() where T : class {
            return GameObject.FindObjectsOfType<Component>()
                .Where(component => component is T)
                .Select(component => component as T);
        }

        public static IEnumerable<Tint> EnsureComponentIsPresentForObjectsInSceneWithInterface<Tint, Tcomp>(
            Action<GameObject> RepairStrategy
        )
            where Tint : class
            where Tcomp : Component
        {
            IEnumerable<Tint> components = GameObject.FindObjectsOfType<Component>()
                .Where(component => component is Tint)
                .Select(component => {
                    var desired = component.GetComponent<Tcomp>();
                    if(desired == null) {
                        RepairStrategy( desired.gameObject );
                        Logging.Warning("{0}: Scene object {1} should have a {2} component - it was missing so I repaired it!");
                    }
                    return component as Tint;
                });
            return components;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public enum ComponentGetType
    {
        SameObject,
        InChild,
        InParent,
        Add
    }

    public static class ComponentUtility
    {
        public static T GetComponentInParentWhere<T>(Transform target, Func<T, bool> criteria) where T : Component
        {
            while (target != null)
            {
                // Check for a component, cancel if one isn't found
                T component = target.GetComponentInParent<T>();
                if (component == null) break;
                // Return if it meets the criteria
                if (criteria.Invoke(component)) return component;
                // Updates the parent, to check even further up in the next loop
                target = component.transform.parent;
            }

            return null;
        }

        public static Child[] GetImmediateComponentsInChildren<Child, Parent>(Parent parent, ref Child[] cachedData) where Child : Component where Parent : Component
        {
            return GetImmediateComponentsInChildren(parent, ref cachedData, (c) => c.GetComponentInParent<Parent>());
        }
        /// <summary>
        /// Gets all components of type <typeparamref name="Child"/>, whose closest parent is <paramref name="parent"/> and not any other instance.
        /// </summary>
        /// <typeparam name="Child"></typeparam>
        /// <typeparam name="Parent"></typeparam>
        /// <param name="parent"></param>
        /// <param name="cachedData"></param>
        /// <returns></returns>
        public static Child[] GetImmediateComponentsInChildren<Child, Parent>(Parent parent, ref Child[] cachedData, System.Func<Child, Parent> getInParent) where Child : Component where Parent : Component
        {
            // Just return cached data if already present
            if (cachedData != null) return cachedData;

            // Do a standard children check
            List<Child> list = new List<Child>();
            list.AddRange(parent.GetComponentsInChildren<Child>());

            // Cull values that have a different immediate parent
            list.RemoveAll((i) => getInParent.Invoke(i) != parent);

            // Cache and return values
            cachedData = list.ToArray();
            return cachedData;
        }

        public static T AutoCache<T>(ref T cacheValue, GameObject target, ComponentGetType getType = ComponentGetType.SameObject) where T : Component
        {
            if (cacheValue == null)
            {
                cacheValue = getType switch
                {
                    ComponentGetType.InChild => target.GetComponentInChildren<T>(),
                    ComponentGetType.InParent => target.GetComponentInParent<T>(),
                    ComponentGetType.Add => target.GetComponent<T>(),
                    _ => target.GetComponent<T>(),
                };
            }
            return cacheValue;
        }
    }
}
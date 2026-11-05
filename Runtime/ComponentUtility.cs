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
        AddIfAbsent,
        Add
    }

    public static class ComponentUtility
    {
        public static T GetComponentInParentWhere<T>(this GameObject target, Func<T, bool> criteria) where T : Component
        {
            return target.transform.GetComponentInParentWhere(criteria);
        }
        public static T GetComponentInParentWhere<T>(this Transform target, Func<T, bool> criteria) where T : Component
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

        public static Child[] GetImmediateComponentsInChildren<Child, Parent>(this Parent parent, ref Child[] cachedData) where Child : Component where Parent : Component
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
        public static Child[] GetImmediateComponentsInChildren<Child, Parent>(this Parent parent, ref Child[] cachedData, System.Func<Child, Parent> getInParent) where Child : Component where Parent : Component
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
        
        /// <summary>
        /// Looks for an instance of <typeparamref name="T"/> on a <typeparamref name="GameObject"/>, and adds one if not already present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T AddIfAbsent<T>(this GameObject target) where T : Component
        {
            bool alreadyExists = target.TryGetComponent(out T existing);
            if (alreadyExists) return existing;

            return target.AddComponent<T>();
        }

        /// <summary>
        /// Automatically searches for an instance of <typeparamref name="T"/> on <paramref name="target"/>, and caches it to <paramref name="cacheValue"/> so it only needs to be obtained once.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="cacheValue"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public static T Cache<T>(this Component target, ref T cacheValue, ComponentGetType getType = ComponentGetType.SameObject) where T : Component
        {
            return Cache(target.gameObject, ref cacheValue, getType);
        }
        public static T Cache<T>(this GameObject target, ref T cacheValue, ComponentGetType getType = ComponentGetType.SameObject) where T : Component
        {
            if (cacheValue != null) return cacheValue;

            cacheValue = getType switch
            {
                ComponentGetType.InChild => target.GetComponentInChildren<T>(),
                ComponentGetType.InParent => target.GetComponentInParent<T>(),
                ComponentGetType.AddIfAbsent => target.AddIfAbsent<T>(),
                ComponentGetType.Add => target.AddComponent<T>(),
                _ => target.GetComponent<T>(),
            };

            return cacheValue;
        }
        [Obsolete]
        public static T AutoCache<T>(ref T cacheValue, GameObject target, ComponentGetType getType = ComponentGetType.SameObject) where T : Component
        {
            if (cacheValue == null)
            {
                cacheValue = getType switch
                {
                    ComponentGetType.InChild => target.GetComponentInChildren<T>(),
                    ComponentGetType.InParent => target.GetComponentInParent<T>(),
                    ComponentGetType.AddIfAbsent => target.AddIfAbsent<T>(),
                    ComponentGetType.Add => target.GetComponent<T>(),
                    _ => target.GetComponent<T>(),
                };
            }
            return cacheValue;
        }
    }
}
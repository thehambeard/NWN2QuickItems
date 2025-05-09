using Kingmaker.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NWN2QuickItems.Utility.UnityExtensions
{
    public static class UnityExtensions
    {
        public static T GetComponentOrThrow<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var component))
                throw new NullReferenceException($"Unable to get {nameof(T)}");

            return component;
        }
        
        public static T GetComponentInChildrenOrThrow<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponentInChildren<T>();

            if (component == null)
                throw new NullReferenceException($"Unable to get {nameof(T)}");

            return component;
        }

        public static bool TryGetComponentIncludeChildren<T>(this Transform transform, out T component) where T : Component =>
            transform.gameObject.TryGetComponentIncludeChildren<T>(out component);

        public static bool TryGetComponentIncludeChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out component))
                return true;

            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }
    }
}

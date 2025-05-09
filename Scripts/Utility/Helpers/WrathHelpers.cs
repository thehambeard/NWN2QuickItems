using Kingmaker;
using Kingmaker.UI;
using Kingmaker.UI.ServiceWindow;
using System;
using UnityEngine;

namespace NWN2QuickItems.Utility.Helpers
{
    public static class WrathHelpers
    {
        public static T GetUIOffStaticCanvas<T>(string path, Action<GameObject> action = null) where T : Component
        {
            var componet = GetUIOffStaticCanvas(path, action).GetComponent<T>()
                ?? throw new NullReferenceException($"Could not get component {nameof(T)}");

            return componet;
        }

        public static GameObject GetUIOffStaticCanvas(string path, Action<GameObject> action = null)
        {
            var transform = Game.Instance.UI.Canvas.transform.parent.Find(path);

            if (transform == null)
                throw new NullReferenceException($"Cannot find {path} on Static Canvas");

            action?.Invoke(transform.gameObject);

            return transform.gameObject;
        }

        public static StaticCanvas GetStaticCanvas() => Game.Instance.UI.Canvas;
        public static FadeCanvas GetFadeCanvas() => Game.Instance.UI.FadeCanvas;
        public static DollRoom GetDollRoom() => Game.Instance.UI.Common.DollRoom;
    }
}

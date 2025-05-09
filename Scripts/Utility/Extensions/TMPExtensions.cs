using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.UI._ConsoleUI.TurnBasedMode;
using NWN2QuickItems.Utility.Helpers;
using NWN2QuickItems.Utility.UnityExtensions;
using System;
using TMPro;
using UnityEngine;
using static LayoutRedirectElement;

namespace NWN2QuickItems.UI.Extensions
{
    public static class TMPExtensions
    {
        public static void FixTMPMaterialShader(this GameObject gameObject)
        {
            foreach (var tmp in gameObject.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                var defaultFont = BlueprintRoot.Instance.UIRoot.DefaultTMPFontAsset;
                tmp.font = defaultFont;

                if (tmp.fontSharedMaterial != null)
                    tmp.fontSharedMaterial = defaultFont.material;

                if (tmp.spriteAsset != null)
                    tmp.spriteAsset = BlueprintRoot.Instance.UIRoot.DefaultTMPSriteAsset;
            }
        }
    }
}

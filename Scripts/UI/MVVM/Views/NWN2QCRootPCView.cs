using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.Utility;
using NWN2QuickItems.UI.Extensions;
using NWN2QuickItems.UI.MVVM.VMs;
using NWN2QuickItems.Utility.Helpers;
using Owlcat.Runtime.UI.MVVM;
using TMPro;
using UnityEngine;

namespace NWN2QuickItems.UI.MVVM.Views
{
    public class NWN2QCRootPCView : ViewBase<NWN2QCRootVM>, IInitializable
    {
        [SerializeField]
        private NWNQCWindowPCView _windowPCView;

        public static NWN2QCRootPCView Root { get; private set; }

        public override void BindViewImplementation()
        {
            _windowPCView.Bind(ViewModel.NWNQCWindowVM);
            _windowPCView.gameObject.FixTMPMaterialShader();
            
        }

        public override void DestroyViewImplementation()
        {
            Main.Logger.Debug($"Destroyed {nameof(NWN2QCRootPCView)}");
            ViewModel.Dispose();
        }

        public void Initialize()
        {
            _windowPCView.Initialize();
        }

        [HarmonyPatch]
        internal static class BindPatch
        {
            static GameObject _rootPCViewPrefab;

            [HarmonyPatch(typeof(ActionBarBaseView), nameof(ActionBarBaseView.BindViewImplementation))]
            [HarmonyPostfix]
            private static void Bind()
            {
                _rootPCViewPrefab = ResourcesLibrary.TryGetResource<GameObject>("d9acedc58ec509b409c39fc7656497f7");

                var go = GameObject.Instantiate(_rootPCViewPrefab, WrathHelpers.GetStaticCanvas().transform, false);
                Root = go.GetComponent<NWN2QCRootPCView>();
                Root.Initialize();
                Root.Bind(new NWN2QCRootVM());
                Root.transform.SetAsFirstSibling();
            }

            [HarmonyPatch(typeof(ActionBarBaseView), nameof(ActionBarBaseView.DestroyViewImplementation))]
            [HarmonyPostfix]
            private static void Unbind()
            {
                Root.DestroyView();
            }
        }
    }
}

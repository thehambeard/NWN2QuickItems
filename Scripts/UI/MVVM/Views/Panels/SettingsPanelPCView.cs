using NWN2QuickItems.UI.MVVM.Views.Settings;
using NWN2QuickItems.UI.MVVM.VMs.Panels;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NWN2QuickItems.UI.MVVM.Views.Panels
{
    public class SettingsPanelPCView : ViewBase<SettingsPanelVM>
    {
        [SerializeField]
        private HotKeySettingPCView _hotKeySettingPCView;

        [SerializeField]
        private BackgroundSettingPCView _backgroundSettingPCView;

        public override void BindViewImplementation()
        {
            _hotKeySettingPCView.Bind(ViewModel.HotKeySettingVM);
            _backgroundSettingPCView.Bind(ViewModel.BackgroundSettingVM);
            gameObject.SetActive(false);
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

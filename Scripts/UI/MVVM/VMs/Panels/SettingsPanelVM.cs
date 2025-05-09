using NWN2QuickItems.UI.MVVM.VMs.Settings;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWN2QuickItems.UI.MVVM.VMs.Panels
{
    public class SettingsPanelVM : BaseDisposable, IViewModel
    {
        public HotKeySettingVM HotKeySettingVM;
        public BackgroundSettingVM BackgroundSettingVM;

        public SettingsPanelVM()
        {
            base.AddDisposable(HotKeySettingVM = new HotKeySettingVM());
            base.AddDisposable(BackgroundSettingVM = new BackgroundSettingVM());
        }

        public override void DisposeImplementation()
        {
        }
    }
}

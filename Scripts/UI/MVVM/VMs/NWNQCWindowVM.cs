using NWN2QuickItems.UI.MVVM.VMs.Panels;
using Owlcat.Runtime.UI.MVVM;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace NWN2QuickItems.UI.MVVM.VMs
{
    public class NWNQCWindowVM : BaseDisposable, IViewModel
    {
        public SpellPanelVM SpellPanelVM;
        public SettingsPanelVM SettingsPanelVM;

        public NWNQCWindowVM()
        {
            base.AddDisposable(SpellPanelVM = new SpellPanelVM());
            base.AddDisposable(SettingsPanelVM = new SettingsPanelVM());
        }

        public override void DisposeImplementation()
        {
        }
    }
}

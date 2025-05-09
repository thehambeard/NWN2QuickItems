using NWN2QuickItems.UI.MVVM.Views;
using Owlcat.Runtime.UI.MVVM;

namespace NWN2QuickItems.UI.MVVM.VMs
{
    public class NWN2QCRootVM : BaseDisposable, IViewModel
    {
        public NWNQCWindowVM NWNQCWindowVM;
        public NWN2QCRootVM()
        {
            base.AddDisposable(NWNQCWindowVM = new NWNQCWindowVM());
        }
        public override void DisposeImplementation()
        {
            Main.Logger.Debug($"Destroyed {nameof(NWN2QCRootVM)}");
        }
    }
}

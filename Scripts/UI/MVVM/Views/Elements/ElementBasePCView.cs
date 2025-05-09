using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using NWN2QuickItems.UI.MVVM.VMs.Elements;
using Owlcat.Runtime.UI.VirtualListSystem.ElementSettings;

namespace NWN2QuickItems.UI.MVVM.Views.Elements
{
    internal abstract class ElementBasePCView<T> : VirtualListElementViewBase<T> where T : ElementBaseVM
    {
        [SerializeField]
        private Button _expandButton;

        public override VirtualListLayoutElementSettings LayoutSettings => _layoutSettings;

        [SerializeField]
        protected VirtualListLayoutElementSettings _layoutSettings;

        public override void BindViewImplementation()
        {
            if (_expandButton != null)
                base.AddDisposable(_expandButton.OnClickAsObservable().Subscribe(_ => ViewModel.ToggleExpandCollapse()));
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

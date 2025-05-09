using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace NWN2QuickItems.UI.MVVM.VMs.Elements
{
    class ClassHeaderElementVM : ElementBaseVM
    {
        public readonly StringReactiveProperty ClassNameText = new StringReactiveProperty("");
        public readonly StringReactiveProperty ExpandedText = new StringReactiveProperty("");

        public ClassHeaderElementVM(string classNameText, bool isExpanded = true, List<ElementBaseVM> children = null, Action<bool> onExpanededChanged = null) : base(isExpanded, children, onExpanededChanged)
        {
            ClassNameText.Value = classNameText;
            base.AddDisposable(base.IsExpanded.Subscribe(isExpanded =>
            {
                ExpandedText.Value = isExpanded ? "-" : "+";
            }));
        }

        public override void DisposeImplementation()
        {
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using NWN2QuickItems.UI.MVVM.VMs.Elements;
using Owlcat.Runtime.UI.MVVM;
using TMPro;
using UnityEngine;
using UniRx;

namespace NWN2QuickItems.UI.MVVM.Views.Elements
{
    internal class ClassHeaderElementPCView : ElementBasePCView<ClassHeaderElementVM>
    {
        [SerializeField]
        private TextMeshProUGUI _expandedText;

        [SerializeField]
        private TextMeshProUGUI _classNameText;

        public override void BindViewImplementation()
        {
            base.BindViewImplementation();
            base.AddDisposable(ViewModel.ClassNameText.Subscribe(x => _classNameText.text = x));
            base.AddDisposable(ViewModel.ExpandedText.Subscribe(x => _expandedText.text = x));
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

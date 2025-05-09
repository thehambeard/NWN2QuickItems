using JetBrains.Annotations;
using Kingmaker.Utility;
using NWN2QuickItems.UI.MVVM.VMs.Elements;
using Owlcat.Runtime.UI.Controls.Button;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using NWN2QuickItems.UI.Extensions;
using Owlcat.Runtime.UI.Controls.Other;
using Kingmaker.UI.MVVM._VM.Tooltip.Utils;
using Owlcat.Runtime.UI.Tooltips;

namespace NWN2QuickItems.UI.MVVM.Views.Elements
{
    public abstract class SpellElementBasePCView<TViewModel> : VirtualListElementViewBase<TViewModel> where TViewModel : SpellElementBaseVM
    {
        [SerializeField]
        protected Image _iconImage;

        [SerializeField]
        protected OwlcatMultiButton _button;

        [SerializeField]
        protected TextMeshProUGUI _resourceText;

        [ConditionalShow("m_UseTooltipCustomPlace")]
        [SerializeField]
        [CanBeNull]
        protected RectTransform m_TooltipCustomPlace;

        [SerializeField]
        protected bool m_UseTooltipCustomPlace;

        protected IDisposable _toolTip;

        protected RectTransform TooltipPlace
        {
            get
            {
                if (!(this.m_TooltipCustomPlace != null))
                    return base.transform as RectTransform;
                return this.m_TooltipCustomPlace;
            }
        }


        public override void BindViewImplementation()
        {
            gameObject.FixTMPMaterialShader();
            base.AddDisposable(ViewModel.Icon.Subscribe(x => _iconImage.sprite = x));
            base.AddDisposable(_button.OnHoverAsObservable().Subscribe(x => ViewModel.OnHover(x)));
            base.AddDisposable(_toolTip = _button.SetTooltip(
                ViewModel.Tooltip,
                new TooltipConfig(
                    InfoCallPCMethod.ShiftRightMouseButton,
                    InfoCallConsoleMethod.LongShortRightStickButton,
                    false,
                    false,
                    this.TooltipPlace,
                    0,
                    0,
                    0,
                    null)));
            base.AddDisposable(ViewModel.ResourceValue.Subscribe(x => _resourceText.text = x.ToString()));
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

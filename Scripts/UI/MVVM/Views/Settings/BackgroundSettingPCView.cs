using NWN2QuickItems.UI.MVVM.VMs.Settings;
using Owlcat.Runtime.UI.MVVM;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace NWN2QuickItems.UI.MVVM.Views.Settings
{
    public class BackgroundSettingPCView : ViewBase<BackgroundSettingVM>
    {
        [SerializeField]
        private Slider _redSlider;

        [SerializeField]
        private Slider _blueSlider;

        [SerializeField]
        private Slider _greenSlider;

        [SerializeField]
        private Slider _alphaSlider;

        [SerializeField]
        private Image _background;

        public override void BindViewImplementation()
        {
            _redSlider.value = ViewModel.Red.Value;
            _greenSlider.value = ViewModel.Green.Value;
            _blueSlider.value = ViewModel.Blue.Value;
            _alphaSlider.value = ViewModel.Alpha.Value;

            base.AddDisposable(_redSlider.OnValueChangedAsObservable().Subscribe(x => ViewModel.Red.Value = x));
            base.AddDisposable(_greenSlider.OnValueChangedAsObservable().Subscribe(x => ViewModel.Green.Value = x));
            base.AddDisposable(_blueSlider.OnValueChangedAsObservable().Subscribe(x => ViewModel.Blue.Value = x));
            base.AddDisposable(_alphaSlider.OnValueChangedAsObservable().Subscribe(x => ViewModel.Alpha.Value = x));
            base.AddDisposable(ViewModel.UpdateColorCommand.Subscribe(x =>
            {
                _background.color = x;
            }));
            
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}

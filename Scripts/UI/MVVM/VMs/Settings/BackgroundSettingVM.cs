using NWN2QuickItems.Settings;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace NWN2QuickItems.UI.MVVM.VMs.Settings
{
    public class BackgroundSettingVM : BaseDisposable, IViewModel
    {
        public FloatReactiveProperty Red = new FloatReactiveProperty();
        public FloatReactiveProperty Blue = new FloatReactiveProperty();
        public FloatReactiveProperty Green = new FloatReactiveProperty();
        public FloatReactiveProperty Alpha = new FloatReactiveProperty();
        public ReactiveCommand<Color> UpdateColorCommand = new ReactiveCommand<Color>();

        public BackgroundSettingVM()
        {
            var setting = Main.Settings.GetSetting<ColorSetting>(SettingKeys.BackgroundColor);
            Red.Value = setting.Red;
            Green.Value = setting.Green;
            Blue.Value = setting.Blue;
            Alpha.Value = setting.Alpha;

            base.AddDisposable(Red.Subscribe(_ => UpdateColor()));
            base.AddDisposable(Green.Subscribe(_ => UpdateColor()));
            base.AddDisposable(Blue.Subscribe(_ => UpdateColor()));
            base.AddDisposable(Alpha.Subscribe(_ => UpdateColor()));
        }

        private void UpdateColor()
        {
            var color = new Color(Red.Value, Green.Value, Blue.Value, Alpha.Value);
            Main.Settings.SetSetting<ColorSetting>(SettingKeys.BackgroundColor, new ColorSetting(color));
            if (UpdateColorCommand.CanExecute.Value)
                UpdateColorCommand.Execute(color);
        }

        public override void DisposeImplementation()
        {
        }
    }
}

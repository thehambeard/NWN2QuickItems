using Kingmaker;
using Kingmaker.Dungeon.Blueprints.Boons;
using Kingmaker.Settings;
using Kingmaker.UI.MVVM._VM.Settings.KeyBindSetupDialog;
using Kingmaker.UI.SettingsUI;
using NWN2QuickItems.Settings;
using NWN2QuickItems.UI.MVVM.Views;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using static Kingmaker.UI.KeyboardAccess;

namespace NWN2QuickItems.UI.MVVM.VMs.Settings
{
    public class HotKeySettingVM : BaseDisposable, IViewModel
    {
        public ReactiveCommand StopBindingCommmand = new ReactiveCommand();

        public KeyBindingData CurrentKeyBinding;
        public bool CurrentBindingIsOccupied;

        public HotKeySettingVM()
        {
            CurrentKeyBinding = Main.Settings.GetSetting<HotKeySetting>(SettingKeys.HotKeyShowHide).ToValue();
        }

        public override void DisposeImplementation()
        {
        }

        public void OnBindingChosen(KeyBindingData keyBindingData)
        {
            CurrentKeyBinding = keyBindingData;
            CurrentBindingIsOccupied = !Game.Instance.Keyboard.CanBeRegistered(
                SettingKeys.HotKeyShowHide, 
                keyBindingData.Key, 
                GameModesGroup.World,
                keyBindingData.IsCtrlDown, 
                keyBindingData.IsAltDown, 
                keyBindingData.IsShiftDown);

            if (!CurrentBindingIsOccupied && StopBindingCommmand.CanExecute.Value)
            {
                StopBindingCommmand.Execute();
                var setting = new HotKeySetting(SettingKeys.HotKeyShowHide, keyBindingData);
                setting.RegisterHotkey();
                Main.Settings.SetSetting<HotKeySetting>(SettingKeys.HotKeyShowHide, setting);
            }
        }
    }
}

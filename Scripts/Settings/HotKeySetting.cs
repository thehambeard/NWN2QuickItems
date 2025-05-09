using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.Settings;
using NWN2QuickItems.UI.MVVM.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.UI.KeyboardAccess;

namespace NWN2QuickItems.Settings
{
    public class HotKeySetting : ISettingWrapper<KeyBindingData>
    {
        public string Name { get; set; }
        public KeyCode Key { get; set; }
        public bool IsShift { get; set; }
        public bool IsAlt { get; set; }
        public bool IsCtrl { get; set; }

        public HotKeySetting()
        {
            
        }

        public HotKeySetting(string name, KeyBindingData keyBindingData)
        {
            Name = name;
            Key = keyBindingData.Key;
            IsAlt = keyBindingData.IsAltDown;
            IsCtrl = keyBindingData.IsCtrlDown;
            IsShift = keyBindingData.IsShiftDown;
        }

        public HotKeySetting(string name, KeyCode key, bool isShift, bool isAlt, bool isCtrl)
        {
            Name = name;
            Key = key;
            IsShift = isShift;
            IsAlt = isAlt;
            IsCtrl = isCtrl;
        }

        public KeyBindingData ToValue() =>
            new KeyBindingData()
            {
                Key = this.Key,
                IsAltDown = IsAlt,
                IsCtrlDown = IsCtrl,
                IsShiftDown = IsShift
            };


        public void RegisterHotkey()
        {
            if (!Game.Instance.Keyboard.CanBeRegistered(Name, Key, GameModesGroup.World, IsCtrl, IsAlt, IsShift))
                return;

            Game.Instance.Keyboard.UnregisterBinding(Name);
            Game.Instance.Keyboard.RegisterBinding(Name, ToValue(), GameModesGroup.World, false);
        }
    }
}

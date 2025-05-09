using Kingmaker.Settings;
using Kingmaker.UI.MVVM._PCView.Settings.KeyBindSetupDialog;
using Kingmaker;
using NWN2QuickItems.UI.MVVM.VMs.Settings;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.UI.MVVM._VM.Settings.KeyBindSetupDialog;
using Kingmaker.UI;
using TMPro;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Rendering;

namespace NWN2QuickItems.UI.MVVM.Views.Settings
{
    public class HotKeySettingPCView : ViewBase<HotKeySettingVM>
    {
        [SerializeField]
        private TextMeshProUGUI _bindingText;

        [SerializeField]
        private Color _normalColor;

        [SerializeField]
        private Color _errorColor;

        [SerializeField]
        private Button _bindButton;

        [SerializeField]
        private Button _cancelBindButton;

        private Coroutine _bindingCoroutine;

        public override void BindViewImplementation()
        {
            base.AddDisposable(_bindButton.OnClickAsObservable().Subscribe(_ =>
            {
                _cancelBindButton.interactable = true;
                _bindButton.interactable = false;
                Game.Instance.UISettingsManager.IsNewKeyBindingSelectionHappening = true;
                _bindingCoroutine = StartCoroutine(BindingRoutine());
            }));
            base.AddDisposable(_cancelBindButton.OnClickAsObservable().Subscribe(_ =>
            {
                StopBinding();
            }));
            base.AddDisposable(ViewModel.StopBindingCommmand.Subscribe(_ => StopBinding()));
            _bindingText.text = ViewModel.CurrentKeyBinding.GetPrettyString();
        }

        public override void DestroyViewImplementation()
        {
            StopBinding();
        }

        private void OnDisable()
        {
            StopBinding();
        }

        private void StopBinding()
        {
            _cancelBindButton.interactable = false;
            _bindButton.interactable = true;
            Game.Instance.UISettingsManager.IsNewKeyBindingSelectionHappening = false;
            _bindingText.color = _normalColor;

            if (_bindingCoroutine != null)
                StopCoroutine(_bindingCoroutine);
        }

        private IEnumerator BindingRoutine()
        {
            while (true)
            {
                KeyBindingData keyBindingData;
                bool validBinding;
                do
                {
                    yield return (object)null;
                    if (!Game.Instance.UISettingsManager.IsNewKeyBindingSelectionHappening)
                        yield break;
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        StopBinding();
                        yield break;
                    }
                    else
                    {
                        validBinding = GetValidBinding(out keyBindingData);
                        DisplayPressedKeys();
                    }
                }
                while (!validBinding);

                ViewModel.OnBindingChosen(keyBindingData);
            }
        }

        private void DisplayPressedKeys()
        {
            KeyBindingData keyBindingData = this.GetTempBinding();
            if (keyBindingData.Key == KeyCode.None && !keyBindingData.IsCtrlDown && !keyBindingData.IsAltDown && !keyBindingData.IsShiftDown)
            {
                this._bindingText.text = ViewModel.CurrentKeyBinding.GetPrettyString();
                return;
            }
            this._bindingText.color = ViewModel.CurrentBindingIsOccupied ? _errorColor : _normalColor;
            this._bindingText.text = keyBindingData.GetPrettyString();
        }

        private KeyBindingData GetTempBinding()
        {
            KeyBindingData tempBinding = new KeyBindingData();
            if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                tempBinding.IsCtrlDown = KeyboardAccess.IsCtrlHold();
                tempBinding.IsAltDown = KeyboardAccess.IsAltHold();
                tempBinding.IsShiftDown = KeyboardAccess.IsShiftHold();
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(key) && key != KeyCode.LeftShift && key != KeyCode.RightShift && key != KeyCode.LeftAlt && key != KeyCode.RightAlt && key != KeyCode.LeftControl && key != KeyCode.RightControl)
                    {
                        tempBinding.Key = key;
                        break;
                    }
                }
            }
            return tempBinding;
        }

        private bool GetValidBinding(out KeyBindingData keyBindingData)
        {
            keyBindingData = new KeyBindingData()
            {
                Key = KeyCode.None
            };

            if (this.CommandKeyDown())
                return false;

            if (Input.anyKeyDown && !this.CommandKeyDown() && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        keyBindingData.Key = key;
                        keyBindingData.IsCtrlDown = KeyboardAccess.IsCtrlHold();
                        keyBindingData.IsAltDown = KeyboardAccess.IsAltHold();
                        keyBindingData.IsShiftDown = KeyboardAccess.IsShiftHold();
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CommandKeyDown()
        {
            return KeyboardAccess.IsAltDown() || KeyboardAccess.IsCtrlDown() || KeyboardAccess.IsShiftDown();
        }

        private bool CommandKeyUp()
        {
            return KeyboardAccess.IsAltUp() || KeyboardAccess.IsCtrlUp() || KeyboardAccess.IsShiftUp();
        }

        private bool CommandKeyHold()
        {
            return KeyboardAccess.IsAltHold() || KeyboardAccess.IsCtrlHold() || KeyboardAccess.IsShiftHold();
        }
    }
}

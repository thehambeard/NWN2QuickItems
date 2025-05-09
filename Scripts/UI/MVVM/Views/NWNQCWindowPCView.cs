using Kingmaker.UI.MVVM._VM.Utility;
using NWN2QuickItems.UI.MVVM.Views.Panels;
using NWN2QuickItems.UI.MVVM.VMs.Panels;
using NWN2QuickItems.UI.MVVM.VMs;
using Owlcat.Runtime.UI.MVVM;
using UnityEngine;
using UnityEngine.UI;
using NWN2QuickItems.Settings;
using UniRx;
using System;
using Kingmaker.PubSubSystem;
using NWN2QuickItems.UI.MVVM.Events;
using Kingmaker;
using Kingmaker.GameModes;

namespace NWN2QuickItems.UI.MVVM.Views
{
    class NWNQCWindowPCView : ViewBase<NWNQCWindowVM>, IInitializable
    {
        [SerializeField]
        private SpellPanelPCView _spellPanelPCView;

        [SerializeField]
        private SettingsPanelPCView _settingsPanelPCView;

        [SerializeField]
        private Button _settingsButton;

        [SerializeField]
        private Sprite _settingsButtonDefault;

        [SerializeField]
        private Sprite _settingsButtonPressed;

        [SerializeField]
        private Image _background;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        private IDisposable _hideShowBinding;
        private WindowSetting _windowSetting;

        public override void BindViewImplementation()
        {
            _spellPanelPCView.Bind(ViewModel.SpellPanelVM);
            _settingsPanelPCView.Bind(ViewModel.SettingsPanelVM);

            LoadRectProperties();
            LoadKeyBindings();
            LoadBackgroundColor();

            base.AddDisposable(_settingsButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsPanelPCView.gameObject.SetActive(!_settingsPanelPCView.gameObject.activeSelf);
                _spellPanelPCView.SetVisible(!_settingsPanelPCView.gameObject.activeSelf);
                _settingsButton.image.sprite = _settingsPanelPCView.gameObject.activeSelf ? _settingsButtonPressed : _settingsButtonDefault;
            }));
            base.AddDisposable(Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_windowSetting.WindowIsShown 
                    && (Game.Instance.CurrentMode == GameModeType.Default || Game.Instance.CurrentMode == GameModeType.Pause))
                {
                    _canvasGroup.alpha = 1f;
                    _canvasGroup.blocksRaycasts = true;
                }
                else
                {
                    _canvasGroup.alpha = 0f;
                    _canvasGroup.blocksRaycasts = false;
                }
            }));
            base.AddDisposable(_hideShowBinding);
            base.AddDisposable(EventBus.Subscribe(this));
        }

        private void LoadKeyBindings()
        {
            var setting = Main.Settings.GetSetting<HotKeySetting>(SettingKeys.HotKeyShowHide);
            _hideShowBinding = Game.Instance.Keyboard.Bind(SettingKeys.HotKeyShowHide, ToggleShowHide);
            setting.RegisterHotkey();
        }

        private void LoadBackgroundColor()
        {
            var setting = Main.Settings.GetSetting<ColorSetting>(SettingKeys.BackgroundColor);
            _background.color = setting.ToValue();
        }

        public override void DestroyViewImplementation()
        {
        }

        public void Initialize()
        {
            _spellPanelPCView.Initialize();
        }

        public void SaveRectProperties(Vector2 position, Vector2 size, Vector3 scale)
        {
            var setting = new WindowSetting(
                gameObject.activeSelf,
                position.x,
                position.y,
                size.x,
                size.y,
                scale.x,
                scale.y,
                scale.z);

            Main.Settings.SetSetting<WindowSetting>(SettingKeys.MainWindowSetting, setting);
        }

        public void LoadRectProperties()
        {
            var rect = (RectTransform)transform;
            _windowSetting = Main.Settings.GetSetting<WindowSetting>(SettingKeys.MainWindowSetting);

            if (_windowSetting.WindowIsShown)
                _canvasGroup.alpha = 1f;
            else
                _canvasGroup.alpha = 0f;

            rect.anchoredPosition = new Vector2(
                _windowSetting.WindowPosX,
                _windowSetting.WindowPosY);

            rect.sizeDelta = new Vector2(
                540f,
                _windowSetting.WindowSizeY);

            rect.localScale = new Vector3(
                _windowSetting.WindowScaleX,
                _windowSetting.WindowScaleY,
                _windowSetting.WindowScaleZ);
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _windowSetting.WindowIsShown = true;
            Main.Settings.SetSetting<WindowSetting>(SettingKeys.MainWindowSetting, _windowSetting);
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _windowSetting.WindowIsShown = false;
            Main.Settings.SetSetting<WindowSetting>(SettingKeys.MainWindowSetting, _windowSetting);
        }

        public void ToggleShowHide()
        {
            if (_windowSetting.WindowIsShown)
                Hide();
            else
                Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NWN2QuickItems.Settings
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class SettingsProvider
    {
        private readonly string _settingsFilePath;
        private Dictionary<string, string> _settingsCache;
        private readonly Timer _flushTimer;
        private readonly int _flushDelayMs;
        private readonly object _lock = new object();
        private bool _pendingFlush;

        public SettingsProvider(string settingsFilePath, int flushDelayMs = 1000)
        {
            _settingsFilePath = settingsFilePath;
            _flushDelayMs = flushDelayMs;
            _flushTimer = new Timer(_ => FlushIfPending(), null, Timeout.Infinite, Timeout.Infinite);
            if (!TryLoadSettingsFromFile(out _settingsCache))
                LoadDefaults();
        }

        public T GetSetting<T>(string key) where T : ISetting
        {
            if (TryGetSetting<T>(key, out var setting))
                return setting;

            return default;
        }

        public bool TryGetSetting<T>(string key, out T value) where T : ISetting
        {
            if (_settingsCache.TryGetValue(key, out var rawValue))
            {
                try
                {
                    value = JsonConvert.DeserializeObject<T>(rawValue);
                    return true;
                }
                catch { }
            }

            value = default;
            return false;
        }

        public void SetSetting<T>(string key, T value) where T : ISetting
        {
            lock (_lock)
            {
                _settingsCache[key] = JsonConvert.SerializeObject(value);
                ScheduleFlush();
            }
        }

        private bool TryLoadSettingsFromFile(out Dictionary<string, string> dict)
        {
            if (File.Exists(_settingsFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_settingsFilePath);
                    dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (dict != null)
                        return true;
                }
                catch
                {
                    
                }
            }

            dict = new Dictionary<string, string>();
            return false;
        }

        private void ScheduleFlush()
        {
            _pendingFlush = true;
            _flushTimer.Change(_flushDelayMs, Timeout.Infinite);
        }

        private void FlushIfPending()
        {
            lock (_lock)
            {
                if (_pendingFlush)
                {
                    _pendingFlush = false;
                    FlushInternal();
                }
            }
        }

        public void Flush()
        {
            lock (_lock)
            {
                _flushTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _pendingFlush = false;
                FlushInternal();
            }
        }

        private void FlushInternal()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_settingsCache, Formatting.Indented);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SettingsProvider] Failed to flush settings: {ex.Message}");
            }
        }

        private void LoadDefaults()
        {
            SetSetting<WindowSetting>(SettingKeys.MainWindowSetting,
                new WindowSetting(true, 100f, -100f, 500f, 700f, .69f, .69f, .69f));

            SetSetting<HotKeySetting>(SettingKeys.HotKeyShowHide,
                new HotKeySetting(SettingKeys.HotKeyShowHide, KeyCode.X, false, false, true));

            SetSetting<ColorSetting>(SettingKeys.BackgroundColor,
                new ColorSetting(0f, 0f, 0f, 0f));
        }
    }
}
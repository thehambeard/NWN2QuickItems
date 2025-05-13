using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWN2QuickItems.Settings
{
    public static class SettingKeys
    {
        public const string MainWindowSetting = "MainWindowSetting";
        public const string HotKeyShowHide = "HotKeyShowHideItems";
        public const string BackgroundColor = "BackgroundColor";
        public static string GetHeaderSettingKey(string header) => "HeaderSetting." + header;
    }
}

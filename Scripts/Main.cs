using NWN2QuickItems.Utility;
using Kingmaker.Modding;
using System.Reflection;
using UnityEngine;
using NWN2QuickItems.Settings;
using System.IO;

namespace NWN2QuickItems
{
    public class Main : MonoBehaviour
    {
        public static OwlcatModification ModDetails { get; private set; }
        public static bool IsEnabled { get; private set; } = true;
        public static Utility.Logger Logger { get; private set; }
        public static ModEventHandler ModEventHandler { get; private set; }
        public static SettingsProvider Settings { get; private set; }

        [OwlcatModificationEnterPoint]
        public static void ModEntryPoint(OwlcatModification modDetails)
        {
            ModDetails = modDetails;
            Logger = new Utility.Logger(modDetails.Logger);
            ModEventHandler = new ModEventHandler();
            Settings = new SettingsProvider(Path.Combine(ModDetails.Path, "settings.json"));

            modDetails.IsEnabled += () => IsEnabled;
            modDetails.OnSetEnabled += OnEnabled;
            OnEnabled(IsEnabled);

        }

        private static void OnEnabled(bool enabled)
        {
            IsEnabled = enabled;
            if (enabled)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                ModEventHandler.Enable(ModDetails.Manifest.UniqueName, assembly);
            }
            else
            {
                ModEventHandler.Disable(ModDetails.Manifest.UniqueName);
            }
        }
    }
}
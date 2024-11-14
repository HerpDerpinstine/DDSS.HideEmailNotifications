using DDSS_HideEmailNotifications.Utils;
using HarmonyLib;
using Il2CppUMUI;

namespace DDSS_HideEmailNotifications.Patches
{
    [HarmonyPatch]
    internal class Patch_UIManager
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIManager), nameof(UIManager.ShowNotification))]
        private static bool ShowNotification_Prefix(string __0)
        {
            // Check for Email Game Events
            if (ConfigHandler._prefs_Enabled.Value && (__0 == "New Email"))
                return false;

            // Run Original
            return true;
        }
    }
}
using MelonLoader;

namespace DDSS_HideEmailNotifications.Utils
{
    internal static class ConfigHandler
    {
        private static MelonPreferences_Category _prefs_Category;
        internal static MelonPreferences_Entry<bool> _prefs_Enabled;

        internal static void Setup()
        {
            // Create Preferences Category
            _prefs_Category = MelonPreferences.CreateCategory("HideEmailNotifications", "Hide Email Notifications");

            // Create Preferences Entries
            _prefs_Enabled = CreatePref("Enabled", "Enabled", "Toggles hiding of Email HUD Notifications", true);
        }

        private static MelonPreferences_Entry<T> CreatePref<T>(
            string id,
            string displayName,
            string description,
            T defaultValue,
            bool isHidden = false)
            => _prefs_Category.CreateEntry(id,
                defaultValue,
                displayName,
                description,
                isHidden,
                false,
                null);
    }
}

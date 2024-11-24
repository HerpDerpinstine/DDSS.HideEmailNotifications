using HarmonyLib;
using MelonLoader;
using System;
using System.Reflection;
using DDSS_HideEmailNotifications.Utils;

namespace DDSS_HideEmailNotifications
{
    internal class MelonMain : MelonMod
    {
        internal static MelonLogger.Instance _logger;

        public override void OnInitializeMelon()
        {
            // Static Cache Logger
            _logger = LoggerInstance;
			
            // Create Preferences
            ConfigHandler.Setup();

            // Apply Patches
            ApplyPatches();
            MakeModHelperAware();

            // Log Success
            _logger.Msg("Initialized!");
        }

        private void ApplyPatches()
        {
            Assembly melonAssembly = typeof(MelonMain).Assembly;
            foreach (Type type in melonAssembly.GetValidTypes())
            {
                // Check Type for any Harmony Attribute
                if (type.GetCustomAttribute<HarmonyPatch>() == null)
                    continue;

                // Apply
                try
                {
                    if (MelonDebug.IsEnabled())
                        LoggerInstance.Msg($"Applying {type.Name}");

                    HarmonyInstance.PatchAll(type);
                }
                catch (Exception e)
                {
                    LoggerInstance.Error($"Exception while attempting to apply {type.Name}: {e}");
                }
            }
        }
		
        private void MakeModHelperAware()
        {
            MelonMod modHelper = null;
            foreach (var mod in RegisteredMelons)
                if (mod.Info.Name == "ModHelper")
                {
                    modHelper = mod;
                    break;
                }
            if (modHelper == null)
                return;

            Type modFilterType = modHelper.MelonAssembly.Assembly.GetType("DDSS_ModHelper.Utils.RequirementFilterHandler");
            if (modFilterType == null) 
                return;

            MethodInfo method = modFilterType.GetMethod("AddOptionalMelon", BindingFlags.Public | BindingFlags.Static);
            if (method == null) 
                return;

            method.Invoke(null, [this]);
        }
    }   
}

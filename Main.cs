using HarmonyLib;
using MelonLoader;
using System;
using System.Reflection;
using DDSS_HideEmailNotifications.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DDSS_HideEmailNotifications
{
    internal class MelonMain : MelonMod
    {
        internal static MelonLogger.Instance _logger;
        internal static Dictionary<string, Color> _playerAddresses = new();

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

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if ((sceneName != "MainMenuScene")
                && (sceneName != "LobbyScene"))
                return;

            _playerAddresses.Clear();
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

            Type modFilterType = modHelper.MelonAssembly.Assembly.GetType("DDSS_ModHelper.Utils.RequirementFilter");
            if (modFilterType == null) 
                return;

            MethodInfo method = modFilterType.GetMethod("AddOptionalMelon", BindingFlags.Public | BindingFlags.Static);
            if (method == null) 
                return;

            method.Invoke(null, [this]);
        }
    }   
}

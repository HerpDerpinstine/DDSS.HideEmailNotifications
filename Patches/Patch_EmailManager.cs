using DDSS_HideEmailNotifications.Utils;
using HarmonyLib;
using Il2Cpp;
using Il2CppObjects.Scripts;
using Il2CppPlayer.Lobby;
using Il2CppPlayer.Tasks;
using Il2CppProps.ServerRack;
using Il2CppUMUI;
using UnityEngine;

namespace DDSS_HideEmailNotifications.Patches
{
    [HarmonyPatch]
    internal class Patch_EmailManager
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmailManager), nameof(EmailManager.RegisterEmail))]
        private static void RegisterEmail_Prefix(string __0, Color __1)
        {
            // Add Player Email to Cache
            __0 = __0.ToLower();
            MelonMain._playerAddresses[__0] = __1;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmailManager), nameof(EmailManager.AddEmailContact))]
        private static void AddEmailContact_Prefix(string __0, Color __1)
        {
            // Add Client Email to Cache
            __0 = __0.ToLower();
            MelonMain._clientAddresses[__0] = __1;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmailManager), nameof(EmailManager.RemoveEmailContact))]
        private static void RemoveEmailContact_Prefix(string __0)
        {
            // Remove Client Address from Cache
            __0 = __0.ToLower();
            if (MelonMain._clientAddresses.ContainsKey(__0))
                MelonMain._clientAddresses.Remove(__0);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmailManager), nameof(EmailManager.UserCode_RpcReceiveEmail__String__String__String__String__String))]
        private static bool UserCode_RpcReceiveEmail__String__String__String__String__String_Prefix(
            EmailManager __instance,
            string __0, // sender
            string __1, // recipient
            string __2, // subject
            string __3, // message
            string __4) // time
        {
            // Check if Servers are Working
            if (!ServerController.connectionsEnabled)
                return false;

            // Validate Recipient
            bool isPlayer = MelonMain._playerAddresses.ContainsKey(__1.ToLower());
            bool isClient = MelonMain._clientAddresses.ContainsKey(__1.ToLower());
            if (!isPlayer && !isClient)
                return false;

            // Validate Sender
            isPlayer = MelonMain._playerAddresses.ContainsKey(__0.ToLower());
            isClient = MelonMain._clientAddresses.ContainsKey(__0.ToLower());
            if (!isPlayer && !isClient)
                return false;

            // Validate LocalPlayer
            LobbyPlayer localPlayer = LobbyManager.instance.GetLocalPlayer();
            if ((localPlayer != null)
                && !localPlayer.WasCollected)
            {
                // Validate Workstation
                WorkStationController workstation = localPlayer.NetworkworkStationController;
                if ((workstation != null)
                    && !workstation.WasCollected)
                {
                    // Validate Recipient
                    string emailAddress = workstation.computerController.emailAddress.ToLower();
                    if ((__1.ToLower() == emailAddress.ToLower())
                        && ((isPlayer && !ConfigHandler._prefs_HidePlayerEmails.Value)
                            || (isClient && !ConfigHandler._prefs_HideGameEmails.Value)))
                        UIManager.instance.ShowNotification(
                            LocalizationManager.instance.GetLocalizedValue("New Email"),
                            LocalizationManager.instance.GetLocalizedValue("You have received a new email from [player]! View it on your computer", [__0]),
                            4f);
                }
            }

            // Add Email to Inbox
            if (!__instance.inbox.ContainsKey(__1))
                __instance.inbox.Add(__1, new Il2CppSystem.Collections.Generic.List<Il2CppSystem.ValueTuple<string, string, string, string>>());
            __instance.inbox[__1].Add(new Il2CppSystem.ValueTuple<string, string, string, string>(__0, __2, __3, __4));
            if (__instance.inbox[__1].Count > 16)
                __instance.inbox[__1].RemoveAt(0);

            // Trigger TaskHook
            TaskHook.TriggerTaskHookCommandStatic(new TaskHook(__0, null, null, "Received Email", null, null));

            // Trigger Local Callback
            __instance.OnEmailReceived.Invoke();

            // Prevent Original
            return false;
        }
    }
}
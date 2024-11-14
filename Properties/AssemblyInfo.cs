using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(DDSS_HideEmailNotifications.Properties.BuildInfo.Description)]
[assembly: AssemblyDescription(DDSS_HideEmailNotifications.Properties.BuildInfo.Description)]
[assembly: AssemblyCompany(DDSS_HideEmailNotifications.Properties.BuildInfo.Company)]
[assembly: AssemblyProduct(DDSS_HideEmailNotifications.Properties.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + DDSS_HideEmailNotifications.Properties.BuildInfo.Author)]
[assembly: AssemblyTrademark(DDSS_HideEmailNotifications.Properties.BuildInfo.Company)]
[assembly: AssemblyVersion(DDSS_HideEmailNotifications.Properties.BuildInfo.Version)]
[assembly: AssemblyFileVersion(DDSS_HideEmailNotifications.Properties.BuildInfo.Version)]
[assembly: MelonInfo(typeof(DDSS_HideEmailNotifications.MelonMain), 
    DDSS_HideEmailNotifications.Properties.BuildInfo.Name, 
    DDSS_HideEmailNotifications.Properties.BuildInfo.Version,
    DDSS_HideEmailNotifications.Properties.BuildInfo.Author,
    DDSS_HideEmailNotifications.Properties.BuildInfo.DownloadLink)]
[assembly: MelonGame("StripedPandaStudios", "DDSS")]
[assembly: VerifyLoaderVersion("0.6.5", true)]
[assembly: HarmonyDontPatchAll]
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(LethalPresence.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(LethalPresence.BuildInfo.Company)]
[assembly: AssemblyProduct(LethalPresence.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + LethalPresence.BuildInfo.Author)]
[assembly: AssemblyTrademark(LethalPresence.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(LethalPresence.BuildInfo.Version)]
[assembly: AssemblyFileVersion(LethalPresence.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(LethalPresence.LethalPresence), LethalPresence.BuildInfo.Name, LethalPresence.BuildInfo.Version, LethalPresence.BuildInfo.Author, LethalPresence.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]
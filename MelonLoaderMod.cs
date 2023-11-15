using MelonLoader;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine.Networking;
using Discord;


namespace LethalPresence
{
    using TMPro;

    public static class BuildInfo
    {
        public const string Name = "Lethal Presence"; 
        public const string Author = "squidypal"; 
        public const string Company = null; 
        public const string Version = "1.0.0"; 
        public const string DownloadLink = null; 
    }
    
    public class LethalPresence : MelonMod
    {
    // Discord im howling at the moon
            private static Discord.Discord _discord;
            private static ActivityManager _activityManager;
            // Client ID of the discord app
            private static long _clientId = 1174141549764935721;
            // In-game quota
            private int quota = 0;
            public List<String> Moons;
            private bool inGame;

// Make the activity 
            public static Activity defaultActivity = new Activity()
            {
                State = "Picking a launch mode",
                Details = "Ready to be a great great asset!",
                Assets =
                {
                    LargeImage = "lethalcompanylargeimage",
                    LargeText = "Lethal Company",
                    SmallImage = "faceicon",
                    SmallText = "Lethal Company"
                },
            };

            public override void OnApplicationStart()
            {
            // Load the discord_game_sdk.dll at runtime
                try
                {
                    LoadEmbeddedDll();
                    _discord = new Discord.Discord(_clientId, 0L);
                    _activityManager = _discord.GetActivityManager();
                    _activityManager.RegisterSteam(1966720);
        
                    MelonLogger.Msg("RichPresence created.");
                    SetActivity(defaultActivity);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Error in OnApplicationStart: " + ex.Message);
                }

                // Unused moons list that might be used later
                Moons.Add("ExperimentationLevel (SelectableLevel)");
                Moons.Add("AssuranceLevelLevel (SelectableLevel)");
                Moons.Add("VowLevel (SelectableLevel)");
                Moons.Add("OffenseLevel (SelectableLevel)");
                Moons.Add("MarchLevel (SelectableLevel)");
                Moons.Add("RendLevel (SelectableLevel)");
                Moons.Add("DineLevel (SelectableLevel)");
                Moons.Add("TitanLevel (SelectableLevel)");
            }

            public override void OnUpdate()
            {
                _discord?.RunCallbacks();
                if (!inGame) return;
                // You need to set activity to update stats
                SetActivity(defaultActivity);
                if (TimeOfDay.Instance.profitQuota > TimeOfDay.Instance.quotaFulfilled)
                {
                    defaultActivity.Details = "Not meeting quota " + TimeOfDay.Instance.quotaFulfilled + "/" +
                                              TimeOfDay.Instance.profitQuota;
                }
                else
                {
                    defaultActivity.Details = "Meeting quota " + TimeOfDay.Instance.quotaFulfilled + "/" +
                                              TimeOfDay.Instance.profitQuota;
                }

                defaultActivity.State = TimeOfDay.Instance.daysUntilDeadline + " days and " +
                                        TimeOfDay.Instance.hoursUntilDeadline + " hours until deadline";
            }

            public static void LoadEmbeddedDll()
            {
                // Get bytes of the embedded DLL
                byte[] dllBytes = EmbeddedResourceHelper.GetResourceBytes("discord_game_sdk.dll");
                if (dllBytes == null)
                {
                    throw new Exception("Failed to find embedded resource: discord_game_sdk.dll");
                }

                // Save the bytes to a temporary file
                string tempDllPath = Path.Combine(Path.GetTempPath(), "discord_game_sdk.dll");
                File.WriteAllBytes(tempDllPath, dllBytes);

                // Load the DLL from the temporary file
                IntPtr libHandle = DllTools.LoadLibrary(tempDllPath);
                if (libHandle == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to load discord_game_sdk.dll. Error Code: {errorCode}");
                }
            }

            public override void OnSceneWasLoaded(int buildIndex, string sceneName)
            {
                switch (sceneName)
                {
                    // Sample scene relay is the in-game thing
                    case "SampleSceneRelay":
                        inGame = true;
                        break;
                    case "MainMenu":
                        defaultActivity.State = "Deciding what to do";
                        defaultActivity.Details = "In the Menu";
                        SetActivity(defaultActivity);
                        inGame = false;
                        break;
                        
                }
            }

            public class EmbeddedResourceHelper
            {
                public static byte[] GetResourceBytes(String filename)
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    foreach (var resource in assembly.GetManifestResourceNames())
                    {
                        if (resource.Contains(filename))
                        {
                            using (Stream resFilestream = assembly.GetManifestResourceStream(resource))
                            {
                                if (resFilestream == null) return null;
                                byte[] ba = new byte[resFilestream.Length];
                                resFilestream.Read(ba, 0, ba.Length);
                                return ba;
                            }
                        }
                    }
                    return null;
                }
            }
            
            public static class DllTools
            {
                [DllImport("kernel32.dll")]
                public static extern IntPtr LoadLibrary(string dllPath);
            }

            public static void SetActivity(Activity activity)
            {
                _activityManager.UpdateActivity(activity, (result =>
                {
                    if (result != Result.Ok)
                        MelonLogger.Msg("Failed: " + result);
                }));
            }
        }
    }

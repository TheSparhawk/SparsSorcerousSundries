using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using UnityEngine.UI;
using HarmonyLib;
using TabletopTweaks;
using TabletopTweaks.Core.ModLogic;
using SparsSorcerousSundries.ModLogic;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.UMMTools;

namespace SparsSorcerousSundries
{
    internal static class Main
    {
        public static bool Enabled;
        public static Harmony harmony;
        public static ModContextSSSundries SSSContext;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            harmony = new Harmony(modEntry.Info.Id);
            SSSContext = new ModContextSSSundries(modEntry);
            SSSContext.LoadAllSettings();
            SSSContext.ModEntry.OnSaveGUI = OnSaveGUI;
            SSSContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(SSSContext);
            return true;
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            SSSContext.SaveAllSettings();
        }
        public static void Log(string msg)
        {
            SSSContext.Logger.Log(msg);
        }
    }

}

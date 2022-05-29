using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using SparsSorcerousSundries.Config;
using static SparsSorcerousSundries.Main;
using static UnityModManagerNet.UnityModManager;


namespace SparsSorcerousSundries.ModLogic
{
    internal class ModContextSSSundries : ModContextBase
    {
        public AddedContent AddedContent;
        public ModContextSSSundries(ModEntry ModEntry) : base(ModEntry)
        {
            LoadAllSettings();
        }

        public override void LoadAllSettings()
        {
            LoadBlueprints("SparsSorcerousSundries.Config", this);
            LoadSettings("AddedContent.json", "SparsSorcerousSundries.Config", ref AddedContent);
            //LoadSettings("Blueprints.json", "SparsSorcerousSundries.Config", ref Blueprints);
            LoadLocalization("SparsSorcerousSundries.Localization");
        }
        public override void AfterBlueprintCachePatches()
        {
            base.AfterBlueprintCachePatches();
            if (Debug)
            {
                //Blueprints.RemoveUnused();
                //SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
        public override void SaveAllSettings()
        {
            base.SaveAllSettings();
            SaveSettings("AddedContent.json", AddedContent);
        }
    }
}



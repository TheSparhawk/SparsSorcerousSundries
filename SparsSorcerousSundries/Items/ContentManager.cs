using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Prerequisites;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI;
using Kingmaker.UI.Log;
using Kingmaker.UI.Log.CombatLog_ThreadSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.View.Equipment;
using Newtonsoft.Json;
using UnityEngine;
using TabletopTweaks;
using SparsSorcerousSundries.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using System.IO;
using static UnityModManagerNet.UnityModManager;
using static SparsSorcerousSundries.Main;
using SparsSorcerousSundries.Items;
using SparsSorcerousSundries.Items.CustomItems;
using SparsSorcerousSundries.Config;
using BlueprintCore.Blueprints.Configurators.Loot;
using SparsSorcerousSundries.Items.FixedItems;
using SparsSorcerousSundries.Utilities;
using BlueprintCore.Blueprints.Configurators.DialogSystem;

namespace SparsSorcerousSundries.Items
{
    public static class ContentManager
    {
        //This class creates a array of vendors and adds the items to their vendor lists based on their designation.

        //reads json file "Designation:vendorlistGuid"
        public static Dictionary<string, string> vendorList = new Dictionary<string, string>()
        {
            { "C4Heraxa","d3dabf45a0d64bc4aa7b205a6889a05f" },
            { "C2Wilcer","5753b6f35e7db234aa44085a358c27af" },
            { "C3Wilcer","5a425708ce352da4f83e6159bdb73c10" },
            { "C1Barkeep","fc312b3b4e355a842815b5c519924ef7" },
            { "C5Wilcer","5aae5d25fc8e485fbee34a89ab1a2278" },
            { "C3Exotic","9c597a1f92dde2f4f8adb27ee5730188" },
            { "C3Arsinoe","d33d4c7396fc1d74c9569bc38e887e86" },
            { "C5Exotic","73895d43f46b45079e19d1afcb96efdd" },
            { "C5Arsinoe","5b73c93dccd743668734070160dfb82f" },
            { "C1Jorum","a7948df9d37efc34e841284cf883370e" },
            {"C2Scroll","cdd7aa16e900b9146bc6963ca53b8e71" },
            {"C2Woljif","24282bde41338884d840a06987c1b3bf" },
            {"C3Woljif","3c098c8feea9cc44eadad74b5352ab95" },
            {"C4Gilmore","712c04b716fe41ef962139cd8394b62f" }
        };
       
        public static void AddSundriestoVendors()
        {
            
            TextLoader.Process("VendorItemList.txt", line => {
                if (line[0] == '#') return;
                if (line[0] == ' ') return;
                var lineParts = line.Split(' ');

                AddItemtoVendor(lineParts[0], vendorList[lineParts[1]], int.Parse(lineParts[2]), int.Parse(lineParts[3]));
                });
        }

        public static void AddItemtoVendor(string itemId, string vendorTableId, int itemAmount, int itemCost = 0)
        {
            var item = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(itemId);
            if(itemCost != 0) { item.m_Cost = itemCost; }
            item.m_Cost = (int)(item.m_Cost * SSSContext.AddedContent.vendorCostMultiplier);
            var vendor_list = ResourcesLibrary.TryGetBlueprint<BlueprintSharedVendorTable>(vendorTableId);
            vendor_list.AddComponent<LootItemsPackFixed>(pack => { pack.m_Count = itemAmount; pack.m_Item = new LootItem(); pack.m_Item.m_Item = item.ToReference<BlueprintItemReference>(); });
        }
        public static void AddItemtoLoot(string itemId, string lootId, int itemCount)
        {
            var item = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(itemId);
            LootConfigurator.For(lootId).AddToItems(new LootEntry() { m_Item = item.ToReference<BlueprintItemReference>(), Count = itemCount }).Configure();
        }
    }



    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class KingmakerItemInjector
    {
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        static void KingmakerItemInjection()
        {    
        }
    }
    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class CustomItemInjector
    {
        static bool Initialized = true;
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        static void CustomItemInjection()
        {
            //Create Gilmore
            Gilmore.CreateGilmore();

            //Create All the Custom items
            Catskin.CreateCatskin();
            LightningRod.CreateLightningRod(); //Need to Fix
            PotofEndlessFood.CreatePotofEndlessFood();
            PrismaticBlade.CreatePrismaticBlade();
            RingofEvershield.CreateRingofEvershield();
            BookofInfSpells.CreateBookOfInfiniteSpells();

            //QuiverPack
            QuiversAnBolts.CreateQuiversAnBolts();

            //fixes
            MarchingTerror.FixMarchingTerror();

            //Add the items to the places
            if (SSSContext.AddedContent.KingmakerItems.Settings["KingmakerVendorItems"].Enabled)
                ContentManager.AddSundriestoVendors();
            if (SSSContext.AddedContent.CustomItems.Settings["CustomVendorItems"].Enabled)
            {
                //Add Custom Vendor Items here
                RingofEvershield.AddRingofEvershield();
                LightningRod.AddLightningRod();
                PrismaticBlade.AddPrismaticBlade();
                Catskin.AddCatskin();
                QuiversAnBolts.AddQuiverAnBolts();
                BookofInfSpells.AddBookOfInfiniteSpells();
            }

            if (SSSContext.AddedContent.CustomItems.Settings["CustomLootItems"].Enabled)
            {
                //Add Custom Loot Items here
                PotofEndlessFood.AddPotofFood();
            }

            if (SSSContext.AddedContent.QuestRewards.Settings["EnableNewQuestRewards"].Enabled)
            {
                QuestItems.DrezenRingReward.FixGalfreyRewardRing();
            }

            if (SSSContext.AddedContent.GilmoresGoods.Settings["EnableGilmoresStore"].Enabled)
            {
                Gilmore.SpawnGilmore();
            }
            var noober = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("cc50a88bbd8dd3e4da066d33d14fdfc8");
            var nooberpor = ResourcesLibrary.TryGetBlueprint<BlueprintPortrait>("5a7708d879494a61bcd72acac17ecd12");
            nooberpor.Data.InitiativePortrait = false;
            HalfPortraitInjecotr.Replacements[noober.PortraitSafe.Data] = AssetLoaderExtensions.LoadInternalPortrait(SSSContext,"Items",file: "NOOBERMed.png", new Vector2Int(332, 432), TextureFormat.BC5);
        }
    }

    public static class TextLoader
    {
        public static void Process(string file, Action<string> perLine)
        {
            var localPath = Path.Combine(SSSContext.ModEntry.Path,"Items",file);
            //Main.Log(localPath);
            Stream stream;
            if (File.Exists(localPath))
            {
                Main.Log($"Loading file '{file}' from local path");
                stream = File.OpenRead(localPath);
            }
            else
            {
                Main.Log(" ****  ERROR: COULD NOT LOAD STREAM");
                return;
            }

            StreamReader reader = new StreamReader(stream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length == 0) continue;
                perLine(line);
            }

            stream.Dispose();

        }

    }
    [HarmonyPatch(typeof(PortraitData), "get_HalfLengthPortrait")]
    public static class HalfPortraitInjecotr
    {
        public static Dictionary<PortraitData, Sprite> Replacements = new();
        public static bool Prefix(PortraitData __instance, ref Sprite __result)
        {
            if (Replacements.TryGetValue(__instance, out __result))
                return false;
            return true;
        }
    }
    [HarmonyPatch(typeof(PortraitData), "get_SmallPortrait")]
    public static class SmallPortraitInjecotr
    {
        public static Dictionary<PortraitData, Sprite> Replacements = new();
        public static bool Prefix(PortraitData __instance, ref Sprite __result)
        {
            if (Replacements.TryGetValue(__instance, out __result))
                return false;
            return true;
        }
    }



}

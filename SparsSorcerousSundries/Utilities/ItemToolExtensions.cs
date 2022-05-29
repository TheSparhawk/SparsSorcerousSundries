using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;

namespace SparsSorcerousSundries.Utilities
{
    public static class ItemToolExtensions
    {

        //public static void SetName(this BlueprintItemEnchantment enchantment,ModContextBase modContext, string name)
        //{
        //    enchantment.m_EnchantName = Helpers.CreateString(modContext,enchantment.name + ".Name", name);
        //}
        //public static void SetDescription(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string description)
        //{
        //    enchantment.m_Description = Helpers.CreateString(modContext, enchantment.Description + "Description", description);
        //}
        public static void SetFlavorText(this BlueprintItem blueprintItem, ModContextBase modContext, string description)
        {
            blueprintItem.m_FlavorText = Helpers.CreateString(modContext, $"{ blueprintItem.name}.FlavorText", description);
        }

        public static void SetFlavorText(this BlueprintItemWeapon blueprintItem, ModContextBase modContext, string description)
        {
            blueprintItem.m_FlavorText = Helpers.CreateString(modContext, $"{ blueprintItem.name}.FlavorText", description);
        }

        public static void SetNonIdentName(this BlueprintItemWeapon blueprintItem, ModContextBase modContext, string description)
        {
            blueprintItem.m_FlavorText = Helpers.CreateString(modContext, $"{ blueprintItem.name}.NonIdentText", description);
        }

        public static void SetNonIdentDesc(this BlueprintItemWeapon blueprintItem, ModContextBase modContext, string description)
        {
            blueprintItem.m_FlavorText = Helpers.CreateString(modContext, $"{ blueprintItem.name}.NonIdentDesc", description);
        }

        //public static void SetFlavorText(this BlueprintItemEquipmentRing blueprintItem, ModContextBase modContext, string description)
        //{
        //    blueprintItem.m_FlavorText = Helpers.CreateString(modContext, blueprintItem.FlavorText + "Text", description);
        //}
        public static void SetPrefix(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string prefix)
        {
            enchantment.m_Prefix = Helpers.CreateString(modContext, $"{ enchantment.name}.Prefix", prefix);
        }
        public static void SetSuffix(this BlueprintItemEnchantment enchantment, ModContextBase modContext, string suffix)
        {
            enchantment.m_Suffix = Helpers.CreateString(modContext, $"{ enchantment.name}.suffix", suffix);
        }

        public static void SetPrefix(this BlueprintWeaponEnchantment enchantment, ModContextBase modContext, string prefix)
        {
            enchantment.m_Prefix = Helpers.CreateString(modContext, $"{ enchantment.name}.Prefix", prefix);
        }
        public static void SetSuffix(this BlueprintWeaponEnchantment enchantment, ModContextBase modContext, string suffix)
        {
            enchantment.m_Suffix = Helpers.CreateString(modContext, $"{ enchantment.name}.suffix", suffix);
        }
        #region
        //public static BlueprintItemWeapon CreateBlueprintWeapon(ModContextBase modContext, string name, string displayName, 
        //    string description, BlueprintWeaponTypeReference weaponType, DiceFormula? damageOverride = null, DamageTypeDescription form = null, 
        //    BlueprintItemWeaponReference secondWeapon = null, bool primaryNatural = false, string guid = null, int price = 1000)
        //{
        //    var bpWeapon = Helpers.CreateBlueprint<BlueprintItemWeapon>(modContext, "", bp => 
        //    {
        //        bp.m_DisplayNameText = displayName.
        //    
        //    });
        //    return bpWeapon;
        //}
        #endregion


        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint)
        {
            AddBlueprint(blueprint, blueprint.AssetGuid);
        }
        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint, string assetId)
        {
            var Id = BlueprintGuid.Parse(assetId);
            AddBlueprint(blueprint, Id);
        }

        public static List<Guid> AddedBlueprints = new();

        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint, BlueprintGuid assetId)
        {
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint(assetId);
            if (loadedBlueprint == null)
            {
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(assetId, blueprint);
                blueprint.OnEnable();
                Main.Log("Added "+ blueprint.name);
            }
            else
            {
                Main.Log($"Failed to Add: {blueprint.name}");
                Main.Log($"Asset ID: {assetId} already in use by: {loadedBlueprint.name}");
            }
        }
    }
}

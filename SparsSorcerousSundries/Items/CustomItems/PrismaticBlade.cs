using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using System;
using TabletopTweaks.Core.Utilities;
using static SparsSorcerousSundries.Main;
using SparsSorcerousSundries.Utilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.View.Equipment;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;
using BlueprintCore.Blueprints.Configurators.Classes;
using Kingmaker.UnitLogic.Mechanics;
using SparsSorcerousSundries.NewComponents;
using Kingmaker.Designers.Mechanics.Facts;

namespace SparsSorcerousSundries.Items.CustomItems
{
    static class PrismaticBlade
    {
        static readonly string itemName = "Prismatic Blade";
        static readonly string itemGuid = "8675FB30-A6BD-42F4-BC3A-BE1A070CADF6";
        public static string ItemGuid
        {
            get { return itemGuid; }
        }
        public static string ItemName
        {
            get { return itemName; }
        }
        public static void CreatePrismaticBlade()
        {
            var Icon_PrismaticBlade = AssetLoader.LoadInternal(SSSContext, "Items", "PrismaticBlade_2.png");
            //var PrismaticBladeBuff = Helpers.CreateBlueprint<BlueprintBuff>(SSSContext, "", bp => { });
            var PrismBladeFeature = FeatureConfigurator.New("PrismBladeFeature", "AFD7BAA3-AEB9-462B-9B7B-2671F08BE6A0")
                .AddComponent<RandomWeaponDamageOverride>()
                .Configure();
            var PrismaticBladeEnchant = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>(SSSContext, $"PrismaticFluctuation2", bp =>
              {
                  bp.SetName(SSSContext, "");
                  bp.SetDescription(SSSContext, "");
                  bp.SetPrefix(SSSContext, "");
                  bp.SetSuffix(SSSContext, "");
                  bp.m_EnchantmentCost = 65;
                  bp.WeaponFxPrefab = new PrefabLink() { AssetId = "bfafef74d59950242915a8e294e6fac0" };
                  bp.AddComponent<AddUnitFeatureEquipment>(c =>
                  {
                      c.m_Feature = PrismBladeFeature.ToReference<BlueprintFeatureReference>();
                  });
                  bp.AddComponent<WeaponEnhancementBonus>(c =>
                  {
                      c.EnhancementBonus = 2;
                  });
              });
            //WeaponEnchantmentLogic
            WeaponEnchantmentConfigurator.New("PBFXEn1", "12D8411B-FD8C-498B-B141-818CB9400897")
                .SetEnchantName(Constants.Empty.String)
                .SetDescription(Constants.Empty.String)
                .SetWeaponFxPrefab(new PrefabLink() { AssetId = "bfafef74d59950242915a8e294e6fac0" })
                .Configure();
            WeaponEnchantmentConfigurator.New("PBFXEn2", "DE8A5498-13EE-414C-AF30-476BCD8C852C")
                .SetEnchantName(Constants.Empty.String)
                .SetDescription(Constants.Empty.String)
                .SetWeaponFxPrefab(new PrefabLink() { AssetId = "91e5a56dd421a2941984a36a2af164b6" })
                .Configure();
            WeaponEnchantmentConfigurator.New("PBFXEn3", "5454AFBB-B9E4-4516-9119-F2A5791E7EEB")
                .SetEnchantName(Constants.Empty.String)
                .SetDescription(Constants.Empty.String)
                .SetWeaponFxPrefab(new PrefabLink() { AssetId = "1d1465ffa2699644ba8dfac48cb33195" })
                .Configure();

            var PrismaticBladeItem = ItemWeaponConfigurator.New(ItemName, ItemGuid)
                .SetDisplayNameText(Helpers.CreateString(SSSContext, "PrismBladeName", "Prismatic Blade"))
                .SetDescriptionText(Helpers.CreateString(SSSContext, "PrismBladeDesc", "This bastard sword deals 1d4 magic damage and 4d6 elemental damage of a random type."))
                .SetFlavorText(Helpers.CreateString(SSSContext, "PrismBladeFT", "Few beings have seen the like of this bastardsword. The blade is made of colored light, continuously shimmering and changing chaotically,  hues spilling and racing after each other."))
                .SetType("d2fe2c5516b56f04da1d5ea51ae3ddfe")
                .SetIcon(Icon_PrismaticBlade)
                .SetCost(230000)
                .SetOverrideDamageDice(true)
                .SetDamageDice(new DiceFormula(2, DiceType.D10))
                .SetOverrideDamageType(true)
                .SetCR(7)
                .SetDamageType(new DamageTypeDescription() { Type = DamageType.Energy, Common = new DamageTypeDescription.CommomData(), Physical = new DamageTypeDescription.PhysicalData(), Energy = DamageEnergyType.Magic })
                .AddToEnchantments(new Blueprint<BlueprintWeaponEnchantmentReference>[] 
                { PrismaticBladeEnchant,
                 "12D8411B-FD8C-498B-B141-818CB9400897",
                 "DE8A5498-13EE-414C-AF30-476BCD8C852C",
                 "5454AFBB-B9E4-4516-9119-F2A5791E7EEB"
                })
                .SetVisualParameters(new WeaponVisualParameters()
                {
                    m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                    m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                    m_WeaponModel = new PrefabLink() { AssetId = "7ee97c8792e9adf4288f71753af3d350" }
                })
                .Configure();//.ToReference<BlueprintItemWeaponReference>();
            //return PrismaticBladeItem;

        }
        public static void AddPrismaticBlade()
        {
            ContentManager.AddItemtoVendor(ItemGuid, ContentManager.vendorList["C4Gilmore"], 1);
        }
    }
}
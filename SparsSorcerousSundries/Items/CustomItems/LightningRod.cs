using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using System;
using TabletopTweaks.Core.Utilities;
using static SparsSorcerousSundries.Main;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.View.Equipment;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using BlueprintCore.Blueprints.Configurators;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using BlueprintCore.Blueprints;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Facts;

namespace SparsSorcerousSundries.Items.CustomItems
{
    class LightningRod
    {

            static readonly string itemName = "Lightning Rod";
            static readonly string itemGuid = "550380f4-d25e-4d70-af75-9dd599e9379a";
            public static string ItemGuid
            {
                get { return itemGuid; }
            }
            public static string ItemName
            {
                get { return itemName; }
            }
        public static void CreateLightningRod()
        {
            var Icon_LightningRod = AssetLoader.LoadInternal(SSSContext, "Items", "LongspearAngry.png");

            var LightningRodFact = BlueprintCore.Blueprints.Configurators.Facts.UnitFactConfigurator.New("LightningRodFact", "A4BC8044-3800-40D9-84CB-AA36D1F5186F").Configure();

            var LightningRodAbility = AbilityConfigurator.New("LightningRodZap", "a5877f5b-8a48-4812-8f82-d4ca43d73a05")
                .SetDisplayName(Helpers.CreateString(SSSContext, "LightRodAbilName", "Cloud to Ground"))
                .SetDescription(Helpers.CreateString(SSSContext, "LightRodAbildesc", ""))
                .SetDescriptionShort(Helpers.CreateString(SSSContext, "LightRodAbilSDesc", ""))
                .AddComponent<AbilityEffectRunAction>(c =>
                {
                    //c.SavingThrowType = SavingThrowType.Reflex;
                    c.Actions = ActionsBuilder.New()
                    .DealDamage(damageType:new DamageTypeDescription() { Type = DamageType.Energy, Energy = DamageEnergyType.Electricity },
                                value:new ContextDiceValue() { DiceType = DiceType.D8, DiceCountValue = 2, BonusValue = new ContextValue()})
                    .Build();
                })
                .AllowTargeting(null, true, true, false)//Set to false so not hit friendly after test
                .AddAbilitySpawnFx(prefabLink: new PrefabLink() { AssetId = "503b78b507366cc4da0f462cb40131f6" }, anchor: AbilitySpawnFxAnchor.SelectedTarget, positionAnchor: AbilitySpawnFxAnchor.SelectedTarget)
                .Configure();

            //WeaponEnchantmentLogic
            var LightningRodEnchant = WeaponEnchantmentConfigurator.New("LightningRodEnchant", "5ce35a7d-6221-4492-aed0-926cce0059ba")
                    .SetEnchantName(Helpers.CreateString(SSSContext, "LightRodEnchName", "Cloud to Ground"))
                    .SetDescription(Helpers.CreateString(SSSContext, "LightRodEnchDesc", "Twenty percent chance on hit this weapon inflicts 2d8 lightning damage to an enemy within 30 yards."))
                    .AddInitiatorAttackWithWeaponTrigger(onlyHit: true, criticalHit: false, onCharge: false,
                     action: ActionsBuilder.New()
                    .Randomize(weightedActions: new (ActionsBuilder actions, int weight)[]
                    {(ActionsBuilder.New().OnRandomTargetsAround(ActionsBuilder.New().CastSpell("a5877f5b-8a48-4812-8f82-d4ca43d73a05",logIfCanNotTarget:true)
                    , numberOfTargets: 1, onEnemies: true, radius: 30.Feet()),20),
                    (ActionsBuilder.New(),80)}))
                    .Configure();
             
            var LightningRodItem = ItemWeaponConfigurator.New(ItemName, ItemGuid)
             .SetDisplayNameText(Helpers.CreateString(SSSContext, "LightRodName", itemName))
            .SetDescriptionText(Helpers.CreateString(SSSContext, "LightRodDesc", "This spear is wreathed in electrical energy. On hit it may release this energy on a enemy in a 30 yard radius."))
            .SetFlavorText(Helpers.CreateString(SSSContext, "LightRodFT", ""))
            .SetType("fa2dd17cbde7d3f4aa918d467c30516e")
            .SetIcon(Icon_LightningRod)
            .AddToEnchantments(
                "eb2faccc4c9487d43b3575d7e77ff3f5",
                "27e28279803f4ef58fdd3ab76e68c376",
                "5ce35a7d-6221-4492-aed0-926cce0059ba")
            .SetCost(56000)
            .SetVisualParameters(new WeaponVisualParameters()
            {
                m_Projectiles = Array.Empty<BlueprintProjectileReference>(),
                m_PossibleAttachSlots = Array.Empty<UnitEquipmentVisualSlotType>(),
                m_WeaponModel = new PrefabLink() { AssetId = "2eeaa76f450f39f41af2a52fd28de7bc" }
            })
            .SetCR(7)
                .Configure();//.ToReference<BlueprintItemWeaponReference>();
                //return LightningRodItem;
            }
        public static void AddLightningRod()
        {
            ContentManager.AddItemtoVendor(ItemGuid, ContentManager.vendorList["C2Wilcer"], 1);
        }
    }
}


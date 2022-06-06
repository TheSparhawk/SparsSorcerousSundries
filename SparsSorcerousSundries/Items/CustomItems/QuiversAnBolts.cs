using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using SparsSorcerousSundries.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static SparsSorcerousSundries.Main;

namespace SparsSorcerousSundries.Items.CustomItems
{
    class QuiversAnBolts
    {

        public static void CreateQuiversAnBolts()
        {
            //Fix Cold Iron
            ItemEquipmentUsableConfigurator.For("a5a537ad28053ad48a7be1c53d7fd7ed").SetCharges(80).SetRestoreChargesOnRest(true).Configure();
            //Fix Flaming
            ItemEquipmentUsableConfigurator.For("25f9b5ef564cbef49a1e54c48e67dfc1").SetCharges(80).SetRestoreChargesOnRest(true).SetCost(2000).Configure();

            //Create Ice Arrows
            var Icon_IceArrowQuiver = AssetLoader.LoadInternal(SSSContext, "Items", file: "IceArrowsQuiverItem.png");

            
            BlueprintTool.AddGuidsByName(
                ("IceArrowQuiverBuff", "a32b2e2b-9db1-4a9b-956a-411353923fd3"),
                ("IceArrowQuiverAbil", "1f03b8b6-c2c4-4c77-9aa2-ef345deba9c1"));

            var IceQuiverBuff = CreateQuiverBaseBuff("IceArrowQuiverBuff", "The owner of this quiver can use it to shoot 80 units of ice ammunition per day. Ice ammunition deals additional {g|Encyclopedia:Dice}1d6{/g} {g|Encyclopedia:Energy_Damage}ice damage{/g} on hit.")
                .RemoveComponents(BuffEnchantWornItem=>true)
                .AddBuffEnchantWornItem(allWeapons: false, enchantmentBlueprint: "421e54078b7719d40915ce0672511d0b", slot: Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
                    .Configure();
            var IceQuiverAbil = CreateQuiverBaseAbility("IceArrowQuiverAbil", "The owner of this quiver can use it to shoot 80 units of ice ammunition per day. Ice ammunition deals additional {g|Encyclopedia:Dice}1d6{/g} {g|Encyclopedia:Energy_Damage}ice damage{/g} on hit.")
                .SetBuff(IceQuiverBuff)
                .Configure();
            CreateQuiverItemBase("IceArrowQuiverItem", "Ice Arrow Quiver", "b9a5fe38-d91f-4aa4-a761-107f8864a172", 2000, "The owner of this quiver can use it to shoot 80 units of ice ammunition per day. Ice ammunition deals additional {g|Encyclopedia:Dice}1d6{/g} {g|Encyclopedia:Energy_Damage}ice damage{/g} on hit.")
                .SetActivatableAbility(IceQuiverAbil)
                .SetIcon(Icon_IceArrowQuiver)
                .Configure();
            
            //ItemEquipmentUsableConfigurator.For("25f9b5ef564cbef49a1e54c48e67dfc1").SetCharges(80)
            //
            //    .Configure().CreateCopy<BlueprintItemEquipmentUsable>(SSSContext, "HellfireQuiver", init =>
            //    {
            //        init.Charges = 69;
            //    });
            //ItemEquipmentUsableConfigurator.For("5ca3486f-9823-4118-80b0-8f9c5dbdf7c3")
            //    .SetDescriptionText(Helpers.CreateString(SSSContext,"hellfirename.txt","Oogeidy Boodegi"))
            //    .SetCharges(420).Configure();
            ////Hellfire Quiver
            //// CreateQuiverBase("HellFireQuiverItem", "Hellfire Quiver", "2a8f1eb4-e617-48f7-ae87-aef7b43b189e", 10000,
            ////     "Hellfire Quiver Test").Configure();


        }
        public static ItemEquipmentUsableConfigurator CreateQuiverItemBase(string name, string dispName, string key, int cost, string desc)
        {
            var clone = ResourcesLibrary.TryGetBlueprint<BlueprintItemEquipmentUsable>("25f9b5ef564cbef49a1e54c48e67dfc1");
            var quiver = ItemEquipmentUsableConfigurator.New(name, key)
                .SetDisplayNameText(Helpers.CreateString(SSSContext, name + ".DispName", dispName))
                .SetDescriptionText(Helpers.CreateString(SSSContext, name + ".DescName", desc))
                .SetNonIdentifiedNameText(clone.m_NonIdentifiedNameText)
                .SetNonIdentifiedDescriptionText(clone.m_NonIdentifiedDescriptionText)
                .SetInventoryEquipSound("WandPut")
                .SetInventoryPutSound("WandPut")
                .SetInventoryTakeSound("WandTake")
                .SetCost(cost)
                .SetWeight((float)1.0)
                .SetIsNotable(false)
                .SetDestructible(false)
                .SetType(UsableItemType.Other)
                .SetMiscellaneousType(Kingmaker.Blueprints.Items.BlueprintItem.MiscellaneousItemType.None)
                .SetSpendCharges(true)
                .SetCharges(80)
                .SetRestoreChargesOnRest(true)
                .SetBeltItemPrefab(clone.m_BeltItemPrefab);

            return quiver;
        }

        public static ActivatableAbilityConfigurator CreateQuiverBaseAbility(string name, string description)
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintActivatableAbility>("5a769135b9b00684ea010d36ae49848e")
                .CreateCopy<BlueprintActivatableAbility>(SSSContext, name, init =>
             {
                 init.m_Description = Helpers.CreateString(SSSContext, name + "DescText", description);
             });
            var ability = ActivatableAbilityConfigurator.For(name);

            return ability;
        }

        public static BuffConfigurator CreateQuiverBaseBuff(string name, string description)
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("19cb147ab37f9234eb2fed40ca15b774")
                .CreateCopy<BlueprintBuff>(SSSContext, name, init =>
                {
                    init.m_Description = Helpers.CreateString(SSSContext, name + "DescText", description);
                    init.RemoveComponents<BuffEnchantWornItem>();
                });
            var buff = BuffConfigurator.For(name);

            return buff;
        }

        public static AbilityConfigurator CreateBoltQuiverBaseAbility()
        {
            var ability = AbilityConfigurator.New("", "");

            return ability;
        }


        //GilmoreVendorBlueprint = clone.CreateCopy<BlueprintUnit>(SSSContext, "GilmoreBlueprintUnit", BlueprintGuid.Parse(Guid));

        //var spawn = Helpers.CreateCopy<BlueprintUnit>(clone);

        //ItemToolExtensions.AddBlueprint(spawn, Guid);

        public static void AddQuiverAnBolts()
        {
            //Add Cold Iron Arrows to 
            ContentManager.AddItemtoVendor("a5a537ad28053ad48a7be1c53d7fd7ed", ContentManager.vendorList["C1Jorum"], 2);
            ContentManager.AddItemtoVendor("a5a537ad28053ad48a7be1c53d7fd7ed", ContentManager.vendorList["C2Wilcer"], 2);

            //Add Flaming Arrows
            ContentManager.AddItemtoVendor("25f9b5ef564cbef49a1e54c48e67dfc1", ContentManager.vendorList["C2Wilcer"], 1);
            ContentManager.AddItemtoVendor("25f9b5ef564cbef49a1e54c48e67dfc1", ContentManager.vendorList["C3Wilcer"], 1);
            //Add Cold Arrows
            ContentManager.AddItemtoVendor("b9a5fe38-d91f-4aa4-a761-107f8864a172", ContentManager.vendorList["C2Wilcer"], 1);
            ContentManager.AddItemtoVendor("b9a5fe38-d91f-4aa4-a761-107f8864a172", ContentManager.vendorList["C3Wilcer"], 1);

            ContentManager.AddItemtoVendor("b9a5fe38-d91f-4aa4-a761-107f8864a172", ContentManager.vendorList["C4Gilmore"], 1);
            //Add Bolts of Piercing
            //ContentManager.AddItemtoVendor("5ca3486f-9823-4118-80b0-8f9c5dbdf7c3", ContentManager.vendorList["C4Gilmore"], 1);

            //Add Bolts of Slaying
            //ContentManager.AddItemtoVendor("03a2b05f-0f7b-4516-81ec-a8bcd2fede37", ContentManager.vendorList["C4Gilmore"], 1);
        }
    }
}

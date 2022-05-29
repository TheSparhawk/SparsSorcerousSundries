using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using SparsSorcerousSundries.Utilities;
using static SparsSorcerousSundries.Main;
using BlueprintCore.Blueprints.Configurators.Items;
using Kingmaker.Blueprints.Loot;

namespace SparsSorcerousSundries.Items.CustomItems
{
    static class RingofEvershield
    {
        static readonly string itemName = "RingofShieldItem";
        static readonly string itemGuid = "11951E72-E548-4624-9A44-FF3AF70815CF";
        public static string ItemGuid
        {
            get { return itemGuid; }
        }
        public static string ItemName
        {
            get { return itemName; }
        }
        public static void CreateRingofEvershield()
        {
            var Icon_RingofShield = AssetLoader.LoadInternal(SSSContext, "Items", file: "Icon_RingofShield.png");
            
            //Create Ring Feature
            var RingofShieldFeat = Helpers.CreateBlueprint<BlueprintFeature>(SSSContext, "RingofShieldFeature", bp => {
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]
                    {
                        ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741").ToReference<BlueprintUnitFactReference>(),
                    };

                });
            });

            //Create Ring Enchantment
            var RingofShieldEnc = Helpers.CreateBlueprint<BlueprintEquipmentEnchantment>(SSSContext,"RingofShieldEnhancement", bp => 
            {
                bp.AddComponent<AddUnitFeatureEquipment>(c => 
                {
                    c.m_Feature = RingofShieldFeat.ToReference<BlueprintFeatureReference>();
                });
                bp.SetName(SSSContext, "");
                bp.SetDescription(SSSContext, "");
            });
            var RingofShieldItem = Helpers.CreateBlueprint<BlueprintItemEquipmentRing>(SSSContext, "RingofShieldItem", bp =>
           {
               bp.SetName(SSSContext, "Ring of Evershield");
               bp.SetDescription(SSSContext, "This ring grants its wearer a continuous shield spell effect.");
               bp.m_Icon = Icon_RingofShield;
               bp.m_Cost = 22111;
               bp.SetFlavorText(SSSContext, "This is a dull iron ring, with a faded engraving that reads: ..ever .. ... Shield. .... .");
               bp.m_InventoryTakeSound = "RingTake";
               bp.m_InventoryPutSound = "RingPut";
               bp.m_InventoryEquipSound = "RingPut";
               bp.m_Enchantments = new BlueprintEquipmentEnchantmentReference[]
               {
                    RingofShieldEnc.ToReference<BlueprintEquipmentEnchantmentReference>()
               };
           });
            
            //SharedVendorTableConfigurator.For("").AddLootItemsPackFixed(new LootItem() { m_Item = RingofShieldItem });

        }
        public static void AddRingofEvershield()
        {
            ContentManager.AddItemtoVendor(ItemGuid, ContentManager.vendorList["C2Scroll"], 1);
        }
    }
}

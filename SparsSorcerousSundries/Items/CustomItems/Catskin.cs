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
using BlueprintCore.Blueprints.Configurators.Items.Armors;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;

namespace SparsSorcerousSundries.Items.CustomItems
{
    class Catskin
    {

        static readonly string itemName = "CatskinArmor";
        static readonly string itemGuid = "20e38b76-580c-4cd7-bb4d-97415b4a800e";
        public static string ItemGuid
        {
            get { return itemGuid; }
        }
        public static string ItemName
        {
            get { return itemName; }
        }

        public static void CreateCatskin()
        {
            var Icon_Catskin= AssetLoader.LoadInternal(SSSContext, "Items", "CatskinArmor.png");

            var CatskinEnch = ArmorEnchantmentConfigurator.New("CatskinEnch", "a9615052-aec1-497f-9c47-24292f597b78")

                .AddArmorEnhancementBonus(2)
                .AddAdvanceArmorStats( maxDexBonusShift:4,arcaneSpellFailureShift:-10 )
                .SetEnchantName(Helpers.CreateString(SSSContext, "CatskinEnchEnchName", "Purrfect Fit"))
                 .SetDescription(Helpers.CreateString(SSSContext, "CatskinEnchEnchDesc", "This +2 leather armor is made of a smooth supple leather and seems to mold to your form, making it more of a second skin."))


                .Configure();

            var CatskinArmor = ItemArmorConfigurator.New(itemName, itemGuid)
                 .SetDisplayNameText(Helpers.CreateString(SSSContext, "CatskinArmorName", itemName))
                .SetDescriptionText(Helpers.CreateString(SSSContext, "CatskinArmorDesc", "A +2 Leather Armour with an increase to Max Dexterity of +4 and no arcane failure bonus."))

                .AddToEnchantments("a9615052-aec1-497f-9c47-24292f597b78")
                .SetCost(12000)
                .SetIcon(Icon_Catskin)
                .SetCR(12)
                .SetIcon(Icon_Catskin)
                .SetType("c850ba40ed3a61b489b438a5ffa71de9")
                .SetEquipmentEntity("f5dd4a07d53e92042be6ecec6b3c802e")
                .Configure();//.ToReference<BlueprintItemArmorReference>();


            //return CatskinArmor;


        }
        public static void AddCatskin()
        {
            ContentManager.AddItemtoVendor(ItemGuid, ContentManager.vendorList["C2Woljif"], 1);
        }


    }
}

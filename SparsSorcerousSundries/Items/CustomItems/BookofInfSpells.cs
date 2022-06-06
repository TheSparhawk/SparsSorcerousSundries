using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;
using static SparsSorcerousSundries.Main;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Items.Equipment;
using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using BlueprintCore.Blueprints.Configurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Actions.Builder;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using System.Collections.Generic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Utility;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Utils.Types;

namespace SparsSorcerousSundries.Items.CustomItems
{
    class BookofInfSpells
    {
        static readonly string itemName = "BookOfInfiniteSpellsItem";
        static readonly string itemGuid = "42f04700-40ee-4782-a017-0d9175c2ec36";
        public static string ItemGuid
        {
            get { return itemGuid; }
        }
        public static string ItemName
        {
            get { return itemName; }
        }
        public static void CreateBookOfInfiniteSpells()
        {
            var Icon_BookOfSpells = AssetLoader.LoadInternal(SSSContext, "Items", file: "Icon_BookOfInfSpells.png");

            var SpellBookAbility = AbilityConfigurator.New("BookOfSpellsAbil", "9de6f5ec-6f6c-40f7-9232-67ec663b26f2")
                .SetType(AbilityType.Special)
                .SetDisplayName(Helpers.CreateString(SSSContext, "SpellBookAbilName", "Shifting Page"))
                .SetDescription(Helpers.CreateString(SSSContext, "SpellBookAbilDesc", "placeholder"))
                .SetDescriptionShort(Helpers.CreateString(SSSContext, "SpellBookAbilDescS", "placeholdershort"))
                .SetAnimationStyle(Kingmaker.View.Animation.CastAnimationStyle.CastActionSelf)
                .SetCustomRange(5.Feet())
                .AllowTargeting(self: true)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .AddRandomTrashItem(lootType:Kingmaker.Enums.TrashLootType.Scrolls_RE,4000))
                .Configure();

            var BookOfSpellsItem = ItemEquipmentUsableConfigurator.New(ItemName, ItemGuid)
                .SetDisplayNameText(Helpers.CreateString(SSSContext, ItemName + "Name", "Book of Infinite Spells"))
                .SetDescriptionText(Helpers.CreateString(SSSContext, ItemName + "Desc", "Once per day this book may be used to grant a random spell scroll."))
                .SetFlavorText(Helpers.CreateString(SSSContext, ItemName + "Flav", ""))
                .SetIcon(Icon_BookOfSpells)
                .SetCharges(1)
                .SetInventoryPutSound("CommonPut")
                .SetInventoryTakeSound("CommonTake")
                .SetRestoreChargesOnRest(true)
                .SetCost(20000)
                .SetWeight((float)5.0)
                .SetType(UsableItemType.Other)
                .SetMiscellaneousType(Kingmaker.Blueprints.Items.BlueprintItem.MiscellaneousItemType.None)
                .SetSpendCharges(true)
                .SetAbility(SpellBookAbility.ToReference<BlueprintAbilityReference>().ToString())
                .Configure();

        }

        public static void AddBookOfInfiniteSpells()
        {

            ContentManager.AddItemtoVendor(itemGuid, ContentManager.vendorList["C3Woljif"], 1);
        }

    }
}

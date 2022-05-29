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
    class PotofEndlessFood
    {
        static readonly string itemName = "PotofEndlessFoodItem";
        static readonly string itemGuid = "53549E29-6E83-4222-8B45-9311F17F21B5";
        public static string ItemGuid
        {
            get { return itemGuid; }
        }
        public static string ItemName
        {
            get { return itemName; }
        }
        static List<string> foodBuffList = new List<string>()
        {
            "b6cdcbd81eac742489605dcb197999bb",
            "192e675439e3ebc4d880b618b4904107",
            "5967611998c5bf44fbde4047ee47e386",
            "0c283d43dde431f43aa13ace4488fba4",
            "667228bd1dd4028438a32e9c64ff5076",
            "1474be355e341ad419cd7813e5239f66",
            "b7765edf798d0324a9d93ece55650a86",
            "1033f520b4947c04f8e3cc34c487fb0d",
            "f599a71369b6fd544b0b0ba827e4846f",
            "865906cd7c7949244bf9205763401194",
            "ca6a914c1c9c8d747997f13802c8ace3",
            "b3f46b546f6532b4691e106b008c8171",
            "04415e7e591d82649abd3c65a12d2e15",
            "ca6a914c1c9c8d747997f13802c8ace3",
            "4583ecddd77443c4bb2f855bed7f736d",
            "a0b88a628424b5a4fbd5d206a3bad8eb",//default recipe
            "62958f0ac8986d143a04e05d73d75a1d",
            "d577e126f13b1694da4fa7fcbb67eeaf",
            "600566c4e43f0254699e77f0d8e87a67",
            "78b04e09372a87643af30786a85416d3",
            "a1a2be9cb4afc7e4ca051efa99c52908",
            "58879110ddb29e740acfc313e8d57d35",
            "addbf512681855446b7b3601633b215a",
            "18b71e7726984b96988b2f665819f4ee",
        };

       public static void CreatePotofEndlessFood()
       { 
            var Icon_PotofEndlessFood = AssetLoader.LoadInternal(SSSContext, "Items", file: "Icon_PotofFood.png");
            var foodBuffDuration = ContextDuration.Fixed(24, DurationRate.Hours);

            //please dont look at this. Just pretend it doesnt exist. 
            var PotofEndlessFoodAbility = AbilityConfigurator.New("PotofFoodAbil", "53549E29-72FF-4222-8B45-9311F17F21B5")
                 .SetType(AbilityType.Special)
                 .SetDisplayName(Helpers.CreateString( SSSContext,"PotofFoodAbilName","Rumbling Satisfaction"))
                 .SetDescription(Helpers.CreateString(SSSContext, "PotofFoodAbilDesc", "Once per day you may use this item to grant you party members each a random food buff, doing so removes any current food buff."))
                 .SetDescriptionShort(Helpers.CreateString(SSSContext, "PotofFoodAbilDescS", "Grats a different food buff"))
                 .SetAnimationStyle( Kingmaker.View.Animation.CastAnimationStyle.CastActionSelf)
                 .SetCustomRange(5.Feet())
                 .AllowTargeting(self: true)
                 .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                 .AddComponent<AbilityEffectRunAction>(c =>
                 {
                     c.Actions = Helpers.CreateActionList(
                         new ContextActionPartyMembers()
                         {
                             Action = createFoodBuffPartyAction()
                             .Build()
                         });
                 }).Configure();

            var PotofEndlessFoodItem = ItemEquipmentUsableConfigurator.New(itemName, itemGuid)
                .SetDisplayNameText(Helpers.CreateString(SSSContext, "PotofFoodName", "Pot of Plenty"))
                .SetDescriptionText(Helpers.CreateString(SSSContext, "PotofFoodDesc", "Once per you may use this item to grant you party members each a random food buff, doing so removes any current food buff."))
                .SetFlavorText(Helpers.CreateString(SSSContext, "PotofFoodFlav", "The pot never runs out of food, but you swear you've seen a paw clawing for help, or a tail wriggling about in the boiling slop."))
                .SetIcon(Icon_PotofEndlessFood)
                .SetCharges(1)
                .SetInventoryPutSound("CommonPut")
                .SetInventoryTakeSound("CommonTake")
                .SetRestoreChargesOnRest(true)
                .SetCost(7300)
                .SetWeight((float)5.0)
                .SetType(UsableItemType.Other)
                .SetMiscellaneousType(Kingmaker.Blueprints.Items.BlueprintItem.MiscellaneousItemType.None)
                .SetSpendCharges(true)
                .SetAbility(PotofEndlessFoodAbility.ToReference<BlueprintAbilityReference>().ToString())
                .Configure();//.ToReference<BlueprintItemEquipmentUsableReference>();
            //return PotofEndlessFoodItem;
       }

        public static ActionsBuilder createFoodBuffPartyAction()
        {
            var foodBuffDuration = ContextDuration.Fixed(24, DurationRate.Hours);
            var action = ActionsBuilder.New();
            List<(ActionsBuilder, int)> buffList = new List<(ActionsBuilder, int)>();
            foreach(var food in foodBuffList)
            {
                action.Conditional(ConditionsBuilder.New().HasBuff(food), ifTrue: ActionsBuilder.New().RemoveBuff(food));
            }
            foreach(var food in foodBuffList)
            {
                buffList.Add((ActionsBuilder.New().ApplyBuff(buff:food, durationValue: foodBuffDuration), 0));
            }
            action.Randomize(buffList.ToArray());
            return action;
        }
        public static void AddPotofFood()
        {
            //ContentManager.AddItemtoVendor(ItemGuid, ContentManager.vendorList["C4Heraxa"], 1);
            ContentManager.AddItemtoLoot(ItemGuid, "aacb725eed36c8249a59e47dc5acc413", 1);
        }

    }
}

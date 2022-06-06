using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static SparsSorcerousSundries.Main;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Utils;

namespace SparsSorcerousSundries.Items.FixedItems
{
    class MarchingTerror
    {
        public static void FixMarchingTerror()
        {
            //var icon = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("25ec6cb6ab1845c48a95f9c20b034220").m_Icon;
            var shakenDebuff = ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().IsMainTarget(true),
                    ifTrue: ActionsBuilder.New().SavingThrow(type:Kingmaker.EntitySystem.Stats.SavingThrowType.Will,
                        fromBuff:false, onResult:ActionsBuilder.New().ConditionalSaved(
                        failed: ActionsBuilder.New().ApplyBuff(buff: "25ec6cb6ab1845c48a95f9c20b034220",
                            durationValue: new ContextDurationValue()
                            {
                                DiceCountValue = new() {ValueType=0,Value=0 },
                                BonusValue = new() { ValueType = 0, Value = 3 },
                                Rate = DurationRate.Rounds,
                                DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                                m_IsExtendable = false
                            }, asChild: true, isFromSpell: false, isNotDispelable: false, sameDuration: false, toCaster: false)))).Build();
           
            //var clone = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("c1af79dcadc9c4648b7cdac512186df5");
            ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("f78d8e9ea51063a4fad861d7587eb7d8").CreateCopy<BlueprintAbility>(SSSContext, "marchingShakenAbility",init=>
            {
                init.DisableLog = false;
            });
            var marchingShakenAbility = AbilityConfigurator.For("f513a0d7eb0647d0a5baf57350f23ede")
              .EditComponent<AbilityEffectRunAction>(edit => 
              {
                  edit.Actions.Actions[0] = shakenDebuff.Actions[0];
              })
              .EditComponent<AbilityTargetsAround>(edit =>
              {
              })
              .EditComponent<ContextSetAbilityParams>(edit =>
              {
                  edit.DC.Value = 21;
              })                
              //.SetIcon(null)
               .Configure();


            var marching_enchant = WeaponEnchantmentConfigurator.For("c1af79dcadc9c4648b7cdac512186df5");
            var marchingShakenAction = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().HasBuff(buff: "1149b7216d824a443bfe14415cf7e3f1", negate: false),
                    ifFalse: ActionsBuilder.New().ApplyBuffPermanent(buff: "1149b7216d824a443bfe14415cf7e3f1", true, false, false, false, false)
                                                 .CastSpell(spell: "f513a0d7eb0647d0a5baf57350f23ede", castByTarget: false)
                            ).Build();

            marching_enchant.EditComponent<AddInitiatorAttackWithWeaponTrigger>(edit =>
            {
                edit.Action.Actions[0] = marchingShakenAction.Actions[0];
            }).Configure();

            WeaponEnchantmentConfigurator.New("MTEn1", "3ab8998e-d32d-4cb2-970e-856fcf600bd4")
                .SetEnchantName(Constants.Empty.String)
                .SetDescription(Constants.Empty.String)
                .AddInitiatorAttackWithWeaponTrigger(reduceHPToZero:true,
                    action:ActionsBuilder.New()
                    .CastSpell("f78d8e9ea51063a4fad861d7587eb7d8",castByTarget:false))
                .Configure();

                


            var marching = ItemWeaponConfigurator.For("75790eb681ee60344b1a2223e9c8c3a6")
                .SetDescriptionText(Helpers.CreateString(SSSContext, "MTerrordesc",
                "Whenever this +1 glaive lands a hit on a new enemy for the first time, the target receives {g|Encyclopedia:Dice}1d6{/g} negative {g|Encyclopedia:Damage}damage{/g}, and all other enemies in a 15-foot radius must pass a {g|Encyclopedia:Saving_Throw}Will saving throw{/g} ({g|Encyclopedia:DC}DC{/g} 21) or become shaken for three {g|Encyclopedia:Combat_Round}round{/g}." +
                "If the target is killed, all other enemies in a 20-foot radius must pass a {g|Encyclopedia:Saving_Throw}Will saving throw{/g} ({g|Encyclopedia:DC}DC{/g} 17) or become feared for one {g|Encyclopedia:Combat_Round}round{/g}."))
                .SetEnchantments(
                    "c1af79dcadc9c4648b7cdac512186df5",
                    "d42fc23b92c640846ac137dc26e000d4",
                    "3ab8998e-d32d-4cb2-970e-856fcf600bd4")
                .Configure();

            //ContentManager.AddItemtoVendor("75790eb681ee60344b1a2223e9c8c3a6", ContentManager.vendorList["C4Gilmore"], 1);
        }
    }
}

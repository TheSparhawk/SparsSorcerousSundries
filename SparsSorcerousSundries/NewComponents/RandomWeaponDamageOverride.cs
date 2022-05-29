using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SparsSorcerousSundries.NewComponents
{
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("406B4703D7844094B7270744092B2658")]
    class RandomWeaponDamageOverride : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
	{

		public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
		{
			//evt.WeaponDamageDiceOverride = new DiceFormula(8, DiceType.D8);
			if (evt.Weapon.Blueprint.Name == "Prismatic Blade")
				evt.Weapon.Blueprint.DamageType.Energy = (DamageEnergyType)(randomActualElement());
		}

		public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
		{
		}

		public int randomActualElement()
        {
			int result = UnityEngine.Random.Range(0, 9);
			if (result == 5 || result == 6) { result = result - 2; }
			return result;
		}
	}
}

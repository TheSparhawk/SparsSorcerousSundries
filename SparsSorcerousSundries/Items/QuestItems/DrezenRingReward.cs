using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Blueprints.Configurators.DialogSystem;
using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using TabletopTweaks.Core.Utilities;
using static SparsSorcerousSundries.Main;
using Kingmaker.Designers.EventConditionActionSystem.Actions;

namespace SparsSorcerousSundries.Items.QuestItems
{
    class DrezenRingReward
    {

        public static void FixGalfreyRewardRing()
        {
            var ring = ItemEquipmentRingConfigurator.For("9772ab64fdf645245ac8ee27b1e5ba69")
                .AddToEnchantments("be195c1cea964e745a198ddadecbaec5")
                .Configure();

            var rewardque = CueConfigurator.For("eb21542ab3025aa4ca7e9cd39513000e")
                .ModifyOnShow(action: init =>
                {
                    init.Actions[1] = new AddItemToPlayer()
                    {
                        name = "16d7e874-e2ef-44fd-a3b5-1d2250a0003f",
                        m_ItemToGive = ring.ToReference<BlueprintItemReference>(),
                        Silent = false,
                        Quantity = 1,
                        Identify = true,
                        Equip = false,
                        EquipOn = null,
                        PreferredWeaponSet = 0,
                        ErrorIfDidNotEquip = true
                    };
                })
                .Configure();
        }
    }
}

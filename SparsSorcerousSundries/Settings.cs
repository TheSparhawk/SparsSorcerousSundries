using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace SparsSorcerousSundries
{
    public class Settings : UnityModManager.ModSettings
    {
        public float VendorItemCostModifier = 2f;
        public bool MyBoolOption = true;
        public string MyTextOption = "Bubbles wished this was a tweak mod";

        public bool KingmakerVendorItems = true;
        public bool CustomVendorItems = true;
        public bool CustomLootItems = true;
        public bool GilmoresSundries = true;
        public bool CustomEnemyGearItems = true;
        public bool NewQuestRewards = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}

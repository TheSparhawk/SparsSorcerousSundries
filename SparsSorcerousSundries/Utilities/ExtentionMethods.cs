using Kingmaker.Blueprints;
using Kingmaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static SparsSorcerousSundries.Main;

namespace SparsSorcerousSundries.Utilities
{
    static class ExtentionMethods
    {
        public static void SetLocalisedName(this BlueprintUnit unit, string key, string name)
        {
            if (unit.LocalizedName.String.Key != key)
            {
                unit.LocalizedName = ScriptableObject.CreateInstance<SharedStringAsset>();
                unit.LocalizedName.String = Helpers.CreateString(SSSContext,key, name);
            }
        }
    }
}

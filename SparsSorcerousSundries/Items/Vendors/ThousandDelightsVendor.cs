using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;


namespace SparsSorcerousSundries.Items
{
    class ThousandDelightsVendor : IAreaActivationHandler
    {
        public void OnAreaActivated()
        {
            Main.Log(Game.Instance.CurrentlyLoadedArea.AssetGuid.m_Guid.ToString());
            if (Game.Instance.CurrentlyLoadedArea.AssetGuid==("180cdb4b48d561f4cb4ef9a066727960") )
            {
                Main.Log("We are in Alushinera");
                SpawnGilmoreUnit();
            }
        }
        public void SpawnGilmoreUnit()
        {
            Main.Log("Starting Gilmore Spawn Check");
            if (Game.Instance.CurrentlyLoadedAreaPart.AssetGuid==("27b0684aedfca0a4ca1eb437e77abb3f"))//check if ThousandDelights
            {
                Main.Log("This is the ThousandDelights");
                if (!Gilmore.IsSpawned)
                {
                    Main.Log("Looking For Gilmore");
                    foreach (UnitEntityData unit in Game.Instance.State.Units)
                    {
                        if (unit.Blueprint.AssetGuid.Equals(Gilmore.Guid))
                        {
                            Main.Log("FoundGilmore!");
                            Gilmore.IsSpawned = true;
                        }
                    }
                }

                if (!Gilmore.IsSpawned)//Check if Gilmore lives
                {
                    Main.Log("Spawning Gilmore!");
                    //var temp = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("0234cbc0cc844da4d9cb225d6ed76a18");
                    UnitEntityData gilmore = Game.Instance.EntityCreator.SpawnUnit(BlueprintTools.GetBlueprint<BlueprintUnit>(Gilmore.Guid),new Vector3(110.1f, 3.41f, 89.04f), Quaternion.identity,null);
                    Main.Log("Spawned Gilmore!");
                    Gilmore.IsSpawned = true;
                }
                
            }
            
        }

    }
}

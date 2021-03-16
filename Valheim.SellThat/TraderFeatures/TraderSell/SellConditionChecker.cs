using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SellThat.TraderFeatures
{
    /*
    [HarmonyPatch(typeof(FejdStartup), "OnWorldStart")]
    public static class SellConditionResetterPatch
    {
        [HarmonyPostfix]
        private static void ResetConditions()
        {

        }
    }
    */

    public class SellConditionChecker
    {
        private static SellConditionChecker instance;

        private SellConditionChecker() { }

        public static SellConditionChecker Instance 
        { 
            get
            {
                if(instance == null)
                {
                    return instance = new SellConditionChecker();
                }
                return instance;
            } 
        }

        public bool ShouldFilter(Trader trader, TraderSellConfig config)
        {
            if(!string.IsNullOrEmpty(config.RequiredGlobalKeys.Value))
            {
                var keys = config.RequiredGlobalKeys.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var zoneSystem = ZoneSystem.instance;

                foreach (var key in keys)
                {
                    if(!zoneSystem.GetGlobalKey(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

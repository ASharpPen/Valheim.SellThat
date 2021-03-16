using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat.TraderFeatures.TraderSell
{
    [HarmonyPatch(typeof(StoreGui))]
    public static class StoreRefresher
    {
        private const string TraderRefreshSeedKey = "TraderRefreshSeed";

        [HarmonyPatch("Show")]
        private static void OnShow(StoreGui __instance, Trader ___m_trader)
        {
            var state = StoreCache.GetState(__instance);

            var currentSeed = CalculateRefreshSeed();

            var view = TraderCache.Instance.GetZNetView(___m_trader);
            var zdo = view.GetZDO();

            var lastSeed = zdo.GetInt(TraderRefreshSeedKey);

            if(lastSeed != currentSeed)
            {
                //Refresh time.
                SellLimitter.RefreshLimits(___m_trader, ConfigurationManager.TraderSellConfig);
                zdo.Set(TraderRefreshSeedKey, currentSeed);
            }
        }

        public static int CalculateRefreshSeed()
        {
            //Get time
            var seconds = ZNet.instance.GetTimeSeconds();
            var day = EnvMan.instance.GetDay(seconds);

            //Calculate random seed
            var worldSeed = WorldGenerator.instance.GetSeed();

            var refreshRate = ConfigurationManager.GeneralConfig.DaysToRefresh.Value;
            var daySeed = refreshRate > 0
                ? (day / refreshRate)
                : 0;
            var currentSeed = worldSeed + daySeed;

            return currentSeed;
        }
    }
}

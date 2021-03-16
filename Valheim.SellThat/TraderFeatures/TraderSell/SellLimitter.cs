using HarmonyLib;
using System.Collections.Generic;

namespace Valheim.SellThat.TraderFeatures
{
    [HarmonyPatch(typeof(StoreGui))]
    public static class SellLimitter
    {
        public static void RefreshLimits(Trader trader, List<TraderSellConfig> configs)
        {
            var zdo = TraderCache.Instance.GetZNetView(trader).GetZDO();

            foreach(var config in configs)
            {
                if (config.LimitNrOfItems.Value > 0)
                {
                    zdo.Set(GetItemsSoldKey(config), 0);
                }
            }
        }

        public static int CanBuyNr(Trader trader, ItemDrop itemDrop, TraderSellConfig config)
        {
            if (config.LimitNrOfItems.Value < 0)
            {
                return int.MaxValue;
            }

            var soldItems = TraderCache.Instance
                .GetZNetView(trader)
                .GetZDO()
                .GetInt(GetItemsSoldKey(config), 0);

            return config.LimitNrOfItems.Value - soldItems;
        }


        [HarmonyPatch("OnBuyItem")]
        [HarmonyPrefix]
        private static bool RecordSoldOnBuy(StoreGui __instance, Trader ___m_trader)
        {
            var state = StoreCache.GetState(__instance);

            if (state.CanAfford)
            {
                if (state.CurrentItemIndex >= 0 && state.CurrentItemIndex < state.CurrentItems.Count)
                {
                    var boughtItem = state.CurrentItems[state.CurrentItemIndex];

                    var zdo = TraderCache.Instance
                        .GetZNetView(___m_trader)
                        .GetZDO();

                    var key = GetItemsSoldKey(boughtItem.Config);

                    var sold = zdo.GetInt(key, 0);
                    sold += boughtItem.Item.m_stack;

                    zdo.Set(key, sold);
                }
            }

            return true;
        }

        private static string GetItemsSoldKey(TraderSellConfig config) => $"{config.GroupName}:Sold";
    }
}

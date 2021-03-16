using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valheim.SellThat.ConfigurationCore;
using Valheim.SellThat.Configurations;
using Valheim.SellThat.Extensions;

namespace Valheim.SellThat.TraderFeatures.TraderSell
{
    [HarmonyPatch(typeof(StoreGui))]
    public static class StoreItemsPatch
    {
        private static GeneralConfig Config => ConfigurationManager.GeneralConfig;
        private static List<TraderSellConfig> ItemConfigs => ConfigurationManager.TraderSellConfig;

        [HarmonyPatch("SelectItem")]
        [HarmonyPostfix]
        private static void GetSelectedItem(StoreGui __instance, int index)
        {
            StoreCache.GetState(__instance).CurrentItemIndex = index;
        }

        [HarmonyPatch("CanAfford")]
        [HarmonyPostfix]
        private static void RecordAffordable(StoreGui __instance, bool __result)
        {
            StoreCache.GetState(__instance).CanAfford = __result;
        }

        [HarmonyPatch("FillList")]
        [HarmonyPrefix]
        private static bool SetItemList(StoreGui __instance, Trader ___m_trader)
        {
            var trader = ___m_trader;
            List<Trader.TradeItem> tradeItems;
            List<ItemWithConfig> items;

            if (Config.ClearAllExisting.Value)
            {
                tradeItems = new List<Trader.TradeItem>(ItemConfigs.Count);
                items = new List<ItemWithConfig>(ItemConfigs.Count);
            }
            else
            {
                tradeItems = TraderCache.Instance.GetDefaultItems(trader).ToList();
                items = new List<ItemWithConfig>(tradeItems.Count + ItemConfigs.Count);
                items.AddRange(tradeItems.Select(x => new ItemWithConfig { Item = x }));
            }

            foreach (var itemConfig in ItemConfigs.OrderBy(x => x.Order.Value))
            {
                //Sanity checks
                if (itemConfig == null || !itemConfig.IsValid())
                {
                    continue;
                }

                if (SellConditionChecker.Instance.ShouldFilter(trader, itemConfig))
                {
                    continue;
                }

                GameObject item = ObjectDB.instance.GetItemPrefab(itemConfig.ItemName?.Value);

                ItemDrop itemDrop = item?.GetComponent<ItemDrop>();

                if (itemDrop == null)
                {
                    Log.LogWarning($"Couldn't find item '{itemConfig.ItemName}'");
                    continue;
                }

                var stackSize = SellLimitter.CanBuyNr(trader, itemDrop, itemConfig);

                var itemStack = stackSize < itemConfig.StackSize.Value
                    ? stackSize.Clamp(0)
                    : itemConfig.StackSize.Value;

                Trader.TradeItem newItem = new Trader.TradeItem
                {
                    m_prefab = itemDrop,
                    m_price = itemConfig.Price.Value,
                    m_stack = itemStack,
                };

                Insert(items, trader, itemConfig, newItem);
            }

            trader.m_items = items.Select(x => x.Item).ToList();
            StoreCache.GetState(__instance).CurrentItems = items;

            return true;
        }

        public static List<ItemWithConfig> PossibleItems(Trader trader)
        {
            List<Trader.TradeItem> tradeItems;
            List<ItemWithConfig> items;

            if (Config.ClearAllExisting.Value)
            {
                tradeItems = new List<Trader.TradeItem>(ItemConfigs.Count);
                items = new List<ItemWithConfig>(ItemConfigs.Count);
            }
            else
            {
                tradeItems = TraderCache.Instance.GetDefaultItems(trader).ToList();
                items = new List<ItemWithConfig>(tradeItems.Count + ItemConfigs.Count);
                items.AddRange(tradeItems.Select(x => new ItemWithConfig { Item = x }));
            }

            foreach (var itemConfig in ItemConfigs.OrderBy(x => x.Order.Value))
            {
                //Sanity checks
                if (itemConfig == null || !itemConfig.IsValid())
                {
                    continue;
                }

                if (SellConditionChecker.Instance.ShouldFilter(trader, itemConfig))
                {
                    continue;
                }

                GameObject item = ObjectDB.instance.GetItemPrefab(itemConfig.ItemName?.Value);

                ItemDrop itemDrop = item?.GetComponent<ItemDrop>();

                if (itemDrop == null)
                {
                    Log.LogWarning($"Couldn't find item '{itemConfig.ItemName}'");
                    continue;
                }

                var stackSize = SellLimitter.CanBuyNr(trader, itemDrop, itemConfig);

                var itemStack = stackSize < itemConfig.StackSize.Value
                    ? stackSize.Clamp(0)
                    : itemConfig.StackSize.Value;

                Trader.TradeItem newItem = new Trader.TradeItem
                {
                    m_prefab = itemDrop,
                    m_price = itemConfig.Price.Value,
                    m_stack = itemStack,
                };

                Insert(items, trader, itemConfig, newItem);
            }
        }

        private static void Insert(List<ItemWithConfig> items, Trader trader, TraderSellConfig config, Trader.TradeItem item)
        {
            var newItem = new ItemWithConfig { Item = item, Config = config };

            if (config.Order.Value >= 0 && items.Count >= config.Order.Value)
            {
                Log.LogTrace($"[{trader.name}]: Inserting item {config.ItemName.Value} at index '{config.Order.Value}'.");

                items.Insert(config.Order.Value, newItem);
            }
            else
            {
                Log.LogTrace($"[{trader.name}]: Adding item {config.ItemName.Value}.");
                items.Add(newItem);
            }
        }
    }
}

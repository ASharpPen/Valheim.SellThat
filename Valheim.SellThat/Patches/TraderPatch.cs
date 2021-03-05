using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat
{
    [HarmonyPatch(typeof(Trader), "Start")]
    public static class ModifyTrader
    {
        public static GeneralConfig Config => ConfigurationManager.GeneralConfig;
        public static TraderSellConfig TraderConfig => ConfigurationManager.TraderSellConfig;

        private static void Postfix(ref Trader __instance)
        {
            string name = __instance.gameObject.name;
            if (Config.DebugMode.Value) Debug.Log($"[{__instance.name}]: Modifying trade items.");


            if (Config.DumpDefaultTraderItemsToFile.Value)
            {
                WriteToFile(__instance.m_items);
            }

            if (Config.ClearAllExisting.Value)
            {
                if (Config.DebugMode.Value) Debug.Log($"[{__instance.name}]: Clearing existing {__instance.m_items.Count} items.");
                __instance.m_items.Clear();
            }

            foreach (var itemConfig in TraderConfig.Items.OrderBy(x => x.Order.Value))
            {
                //Sanity checks
                if (itemConfig == null || !itemConfig.IsValid())
                {
                    continue;
                }

                GameObject item = ObjectDB.instance.GetItemPrefab(itemConfig.ItemName?.Value);

                ItemDrop itemDrop = item?.GetComponent<ItemDrop>();

                if (itemDrop == null)
                {
                    Debug.LogWarning($"Couldn't find item '{itemConfig.ItemName}'");
                    continue;
                }

                Trader.TradeItem newItem = new Trader.TradeItem
                {
                    m_prefab = itemDrop,
                    m_price = itemConfig.Price.Value,
                    m_stack = itemConfig.StackSize.Value,
                };

                Insert(__instance, itemConfig, newItem);
            }
        }

        private static void Insert(Trader trader, ItemConfig config, Trader.TradeItem item)
        {
            if (config.Order.Value >= 0 && trader.m_items.Count >= config.Order.Value)
            {
                if (Config.DebugMode.Value) Debug.Log($"[{trader.name}]: Inserting item {config.ItemName.Value} at index '{config.Order.Value}'.");

                trader.m_items.Insert(config.Order.Value, item);
            }
            else
            {
                if (Config.DebugMode.Value) Debug.Log($"[{trader.name}]: Adding item {config.ItemName.Value}.");
                trader.m_items.Add(item);
            }
        }

        public static void WriteToFile(List<Trader.TradeItem> tradeItems)
        {
            string filePath = Path.GetFullPath(@".\trader_items.txt");
            if (Config.DebugMode.Value) Debug.Log($"Writing default trader items to '{filePath}'.");

            var fields = typeof(Trader.TradeItem).GetFields();
            List<string> lines = new List<string>(tradeItems.Count * fields.Length);

            foreach (var item in tradeItems)
            {
                foreach (var field in fields)
                {
                    lines.Add($"{field.Name}: {field.GetValue(item)}");
                }
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}

using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using Valheim.SellThat.ConfigurationCore;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat.TraderFeatures.Sell
{
    [HarmonyPatch(typeof(Trader))]
    public static class TraderItemList
    {
        internal static GeneralConfig Config => ConfigurationManager.GeneralConfig;
        internal static List<TraderSellConfig> TraderConfig => ConfigurationManager.TraderSellConfig;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void RecordDefaultItems(Trader __instance)
        {
            if (Config.DumpDefaultTraderItemsToFile.Value)
            {
                WriteToFile(__instance.m_items);
            }
        }

        public static void WriteToFile(List<Trader.TradeItem> tradeItems)
        {
            string filePath = Path.Combine(Paths.PluginPath, "trader_default_items.txt");

            Log.LogInfo($"Writing default trader items to '{filePath}'.");

            var fields = typeof(Trader.TradeItem).GetFields();
            List<string> lines = new List<string>(tradeItems.Count * fields.Length);

            foreach (var item in tradeItems)
            {
                lines.Add($"[{item.m_prefab.name}]");

                foreach (var field in fields)
                {
                    lines.Add($"{field.Name}={field.GetValue(item)}");
                }
                lines.Add("");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}

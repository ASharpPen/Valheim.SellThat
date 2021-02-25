using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Valheim.SellThat
{
    public class TraderSellConfigurationLoader
    {
        public static void WriteToFile(List<Trader.TradeItem> tradeItems)
        {
            string filePath = Path.GetFullPath(@".\trader_items.txt");
            if (SellThatPlugin.Config.DebugMode.Value) Debug.Log($"Writing default trader items to '{filePath}'.");

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

        public static void InitializeDefault(ConfigFile config)
        {
            config.Bind("HelmetYule", nameof(ItemConfig.Enabled), true);
            config.Bind("HelmetYule", nameof(ItemConfig.Order), 0);
            config.Bind("HelmetYule", nameof(ItemConfig.ItemName), "HelmetYule");
            config.Bind("HelmetYule", nameof(ItemConfig.Price), 100);
            config.Bind("HelmetYule", nameof(ItemConfig.StackSize), 1);

            config.Bind("HelmetDverger", nameof(ItemConfig.Enabled), true);
            config.Bind("HelmetDverger", nameof(ItemConfig.Order), 1);
            config.Bind("HelmetDverger", nameof(ItemConfig.ItemName), "HelmetDverger");
            config.Bind("HelmetDverger", nameof(ItemConfig.Price), 620);
            config.Bind("HelmetDverger", nameof(ItemConfig.StackSize), 1);

            config.Bind("BeltStrength", nameof(ItemConfig.Enabled), true);
            config.Bind("BeltStrength", nameof(ItemConfig.Order), 2);
            config.Bind("BeltStrength", nameof(ItemConfig.ItemName), "BeltStrength");
            config.Bind("BeltStrength", nameof(ItemConfig.Price), 950);
            config.Bind("BeltStrength", nameof(ItemConfig.StackSize), 1);

            config.Bind("YmirRemains", nameof(ItemConfig.Enabled), true);
            config.Bind("YmirRemains", nameof(ItemConfig.Order), 3);
            config.Bind("YmirRemains", nameof(ItemConfig.ItemName), "YmirRemains");
            config.Bind("YmirRemains", nameof(ItemConfig.Price), 120);
            config.Bind("YmirRemains", nameof(ItemConfig.StackSize), 1);

            config.Bind("FishingRod", nameof(ItemConfig.Enabled), true);
            config.Bind("FishingRod", nameof(ItemConfig.Order), 4);
            config.Bind("FishingRod", nameof(ItemConfig.ItemName), "FishingRod");
            config.Bind("FishingRod", nameof(ItemConfig.Price), 350);
            config.Bind("FishingRod", nameof(ItemConfig.StackSize), 1);

            config.Bind("FishingBait", nameof(ItemConfig.Enabled), true);
            config.Bind("FishingBait", nameof(ItemConfig.Order), 5);
            config.Bind("FishingBait", nameof(ItemConfig.ItemName), "FishingBait");
            config.Bind("FishingBait", nameof(ItemConfig.Price), 10);
            config.Bind("FishingBait", nameof(ItemConfig.StackSize), 50);
        }

        public static void ScanBindings(ConfigFile config)
        {
            var lines = File.ReadAllLines(config.ConfigFilePath);

            string lastSection = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("["))
                {
                    string sectionName = new Regex(@"(?<=[[]).+(?=[]])").Match(line).Value;
                    lastSection = sectionName;
                }
                else if (line.Length > 0 && line.Contains("="))
                {
                    var keyValue = line.Split('=');

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();

                        if (SellThatPlugin.Config.DebugMode.Value) Debug.Log($"Binding {lastSection}:{key}");

                        switch (key)
                        {
                            case nameof(ItemConfig.Enabled):
                                _ = config.Bind(lastSection, key, true);
                                break;
                            case nameof(ItemConfig.Order):
                                _ = config.Bind(lastSection, key, -1);
                                break;
                            case nameof(ItemConfig.ItemName):
                                _ = config.Bind(lastSection, key, "");
                                break;
                            case nameof(ItemConfig.Price):
                                _ = config.Bind(lastSection, key, 1);
                                break;
                            case nameof(ItemConfig.StackSize):
                                _ = config.Bind(lastSection, key, 1);
                                break;
                            default:
                                _ = config.Bind(lastSection, key, "");
                                break;
                        }
                    }
                }
            }
        }

        public static TraderSellConfig CreateTraderConfigFromBindings(ConfigFile configFile)
        {
            var items = configFile.Keys.GroupBy(x => x.Section);

            TraderSellConfig traderConfig = new TraderSellConfig();

            foreach (var item in items)
            {
                if (SellThatPlugin.Config.DebugMode.Value) Debug.Log(item.Key);

                ItemConfig itemConfig = new ItemConfig
                {
                    Enabled = configFile.Bind(item.Key, nameof(itemConfig.Enabled), true),
                    Order = configFile.Bind(item.Key, nameof(itemConfig.Order), -1),
                    ItemName = configFile.Bind(item.Key, nameof(itemConfig.ItemName), ""),
                    Price = configFile.Bind(item.Key, nameof(itemConfig.Price), 1),
                    StackSize = configFile.Bind(item.Key, nameof(itemConfig.StackSize), 1),
                };

                traderConfig.Items.Add(itemConfig);
            }

            return traderConfig;
        }
    }
}

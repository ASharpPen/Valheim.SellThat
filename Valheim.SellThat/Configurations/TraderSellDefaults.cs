using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Valheim.SellThat.ConfigurationCore;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat
{
    public static class TraderSellDefaults
    {
        public static void InitializeDefault(ConfigFile config)
        {
            config.Bind("HelmetYule", nameof(TraderSellConfig.Enabled), true, "Enable/disable this section.");
            config.Bind("HelmetYule", nameof(TraderSellConfig.Order), 0, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");
            config.Bind("HelmetYule", nameof(TraderSellConfig.ItemName), "HelmetYule", "Item prefab name for trader to sell.");
            config.Bind("HelmetYule", nameof(TraderSellConfig.Price), 100, "Price to buy this item.");
            config.Bind("HelmetYule", nameof(TraderSellConfig.StackSize), 1, "Number of items to sell at a time.");

            config.Bind("HelmetDverger", nameof(TraderSellConfig.Enabled), true, "Enable/disable this section.");
            config.Bind("HelmetDverger", nameof(TraderSellConfig.Order), 1, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");
            config.Bind("HelmetDverger", nameof(TraderSellConfig.ItemName), "HelmetDverger", "Item prefab name for trader to sell.");
            config.Bind("HelmetDverger", nameof(TraderSellConfig.Price), 620, "Price to buy this item.");
            config.Bind("HelmetDverger", nameof(TraderSellConfig.StackSize), 1, "Number of items to sell at a time.");

            config.Bind("BeltStrength", nameof(TraderSellConfig.Enabled), true, "Enable/disable this section.");
            config.Bind("BeltStrength", nameof(TraderSellConfig.Order), 2, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");
            config.Bind("BeltStrength", nameof(TraderSellConfig.ItemName), "BeltStrength", "Item prefab name for trader to sell.");
            config.Bind("BeltStrength", nameof(TraderSellConfig.Price), 950, "Price to buy this item.");
            config.Bind("BeltStrength", nameof(TraderSellConfig.StackSize), 1, "Number of items to sell at a time.");

            config.Bind("YmirRemains", nameof(TraderSellConfig.Enabled), true, "Enable/disable this section.");
            config.Bind("YmirRemains", nameof(TraderSellConfig.Order), 3, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");
            config.Bind("YmirRemains", nameof(TraderSellConfig.ItemName), "YmirRemains", "Item prefab name for trader to sell.");
            config.Bind("YmirRemains", nameof(TraderSellConfig.Price), 120, "Price to buy this item.");
            config.Bind("YmirRemains", nameof(TraderSellConfig.StackSize), 1, "Number of items to sell at a time.");

            config.Bind("FishingRod", nameof(TraderSellConfig.Enabled), true, "Enable/disable this section.");
            config.Bind("FishingRod", nameof(TraderSellConfig.Order), 4, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");
            config.Bind("FishingRod", nameof(TraderSellConfig.ItemName), "FishingRod", "Item prefab name for trader to sell.");
            config.Bind("FishingRod", nameof(TraderSellConfig.Price), 350, "Price to buy this item.");
            config.Bind("FishingRod", nameof(TraderSellConfig.StackSize), 1, "Number of items to sell at a time.");

            config.Bind("FishingBait", nameof(TraderSellConfig.Enabled), true, "Enable/disable this section.");
            config.Bind("FishingBait", nameof(TraderSellConfig.Order), 5, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");
            config.Bind("FishingBait", nameof(TraderSellConfig.ItemName), "FishingBait");
            config.Bind("FishingBait", nameof(TraderSellConfig.Price), 10);
            config.Bind("FishingBait", nameof(TraderSellConfig.StackSize), 50);
        }
    }
}

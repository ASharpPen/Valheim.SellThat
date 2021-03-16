using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using Valheim.SellThat.ConfigurationCore;

namespace Valheim.SellThat
{
    [Serializable]
    public class TraderSellConfig : ConfigurationGroup<ItemConfig>
    {
        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable this section.");

        public ConfigurationEntry<int> Order = new ConfigurationEntry<int>(-1, "Where to attempt to put item in traders list. Smaller is higher, -1 means at the end.");

        public ConfigurationEntry<string> ItemName = new ConfigurationEntry<string>("", "Item prefab name for trader to sell.");

        public ConfigurationEntry<int> StackSize = new ConfigurationEntry<int>(1, "Number of items to sell at a time.");

        public ConfigurationEntry<int> Price = new ConfigurationEntry<int>(1, "Price to buy this item.");

        public ConfigurationEntry<string> RequiredGlobalKeys = new ConfigurationEntry<string>("", "Array (separate by \",\") of global keys required for item to show.");

        public ConfigurationEntry<int> LimitNrOfItems = new ConfigurationEntry<int>(-1, "Set a limitted number of items that will be sold at a time. Refreshed based on the other limit-reset configurations.");

        public ConfigurationEntry<float> ChanceToSell = new ConfigurationEntry<float>(100, "Chance to sell item per refresh.");

        public ConfigurationEntry<float> PriceRandomMin = new ConfigurationEntry<float>(0, "When available, will randomly set a price at minimum this value.");

        public ConfigurationEntry<float> PriceRandomMax = new ConfigurationEntry<float>(0, "When available, will randomly set a price at maximum this value.");

        public bool IsValid()
        {
            if (Enabled == null || !Enabled.Value)
            {
                return false;
            }

            if (ItemName == null || string.IsNullOrEmpty(ItemName.Value))
            {
                return false;
            }

            if (Price == null || Price.Value < 0)
            {
                return false;
            }

            if (StackSize == null || StackSize.Value < 0)
            {
                return false;
            }

            return true;
        }
    }

    [Serializable]
    public class ItemConfig : ConfigurationSection
    {
    }
}

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

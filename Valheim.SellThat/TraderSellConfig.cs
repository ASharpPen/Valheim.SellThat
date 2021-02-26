using BepInEx.Configuration;
using System.Collections.Generic;

namespace Valheim.SellThat
{
    public class TraderSellConfig
    {
        public List<ItemConfig> Items { get; set; } = new List<ItemConfig>();
    }

    public class ItemConfig
    {
        public ConfigEntry<bool> Enabled { get; set; }

        public ConfigEntry<int> Order { get; set; }

        public ConfigEntry<string> ItemName { get; set; }

        public ConfigEntry<int> StackSize { get; set; }

        public ConfigEntry<int> Price { get; set; }

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
}

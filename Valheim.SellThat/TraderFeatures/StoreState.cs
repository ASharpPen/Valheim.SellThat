using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SellThat.TraderFeatures
{
    public class StoreState
    {
        public int CurrentItemIndex;

        public bool CanAfford;

        public Trader.TradeItem SelectedItem;

        public TraderSellConfig SelectedItemConfig;

        public List<ItemWithConfig> CurrentItems;
    }

    public class ItemWithConfig
    {
        public Trader.TradeItem Item;

        public TraderSellConfig Config;
    }
}

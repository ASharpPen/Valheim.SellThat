using System;
using System.Collections.Generic;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat.Multiplayer
{
    [Serializable]
    internal class ConfigurationPackage
    {
        public GeneralConfig GeneralConfig;

        public List<TraderBuyingConfig> BuyConfigs;

        public List<TraderSellConfig> SellConfig;

        public ConfigurationPackage(){ }

        public ConfigurationPackage(
            GeneralConfig generalConfig, 
            List<TraderBuyingConfig> buyConfigs,
            List<TraderSellConfig> sellConfig)
        {
            GeneralConfig = generalConfig;
            BuyConfigs = buyConfigs;
            SellConfig = sellConfig;
        }
    }
}

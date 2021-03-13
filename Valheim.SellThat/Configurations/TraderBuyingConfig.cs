using System;
using Valheim.SellThat.ConfigurationCore;

namespace Valheim.SellThat.Configurations
{
    [Serializable]
    public class TraderBuyingConfig : ConfigurationGroup<BuyPlaceholder>
    {
        public ConfigurationEntry<string> ItemName = new ConfigurationEntry<string>("", "Name of item prefab to configure.");

        public ConfigurationEntry<int> Price = new ConfigurationEntry<int>(0, "Value that trader will buy item at.");
    }

    [Serializable]
    public class BuyPlaceholder : ConfigurationSection
    {

    }
}

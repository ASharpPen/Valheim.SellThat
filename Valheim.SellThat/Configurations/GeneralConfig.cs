using BepInEx.Configuration;
using System;
using Valheim.SellThat.ConfigurationCore;

namespace Valheim.SellThat
{
    [Serializable]
    public class GeneralConfig
    {
        #region General

        public ConfigurationEntry<bool> EnableMod = new ConfigurationEntry<bool>(true, "Kill switch for disabling all mod features. Only works on game launch.");

        public ConfigurationEntry<bool> ClearAllExisting = new ConfigurationEntry<bool>(true, "When enabled, all existing items for sales gets removed before adding configured.");

        public ConfigurationEntry<bool> StopTouchingMyConfigs = new ConfigurationEntry<bool>(false, "Disables automatic updating and saving of drop table configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");

        #endregion

        #region Trader

        public ConfigurationEntry<int> DaysToRefresh = new ConfigurationEntry<int>(-1, "Set a number of days between trader refreshing his wares.");

        #endregion

        #region Debug
        public ConfigurationEntry<bool> DebugMode = new ConfigurationEntry<bool>(false, "Enable debug logging.");

        public ConfigurationEntry<bool> EnableTraceLogging = new ConfigurationEntry<bool>(false, "Enable debug trace logging.");

        public ConfigurationEntry<bool> DumpDefaultTraderItemsToFile = new ConfigurationEntry<bool>(false, "When Trader starts, his items will be logged in a file on disk in the plugin folder.");

        #endregion

        public void Load(ConfigFile configFile)
        {
            EnableTraceLogging.Bind(configFile, "Debug", "EnableTrace");
            DebugMode.Bind(configFile, "Debug", "EnableDebug");
            DumpDefaultTraderItemsToFile.Bind(configFile, "Debug", "DumpDefaultTraderItemsToFile");

            DaysToRefresh.Bind(configFile, "Trader", nameof(DaysToRefresh));

            StopTouchingMyConfigs.Bind(configFile, "General", nameof(StopTouchingMyConfigs));
            ClearAllExisting.Bind(configFile, "General", "ClearAllExisting");
            EnableMod.Bind(configFile, "General", "EnableMod");
        }
    }

}

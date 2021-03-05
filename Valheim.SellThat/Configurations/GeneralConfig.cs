using BepInEx.Configuration;

namespace Valheim.SellThat
{
    public class GeneralConfig
    {
        #region General

        public ConfigEntry<bool> EnableMod { get; set; }

        public ConfigEntry<bool> ClearAllExisting { get; set; }

        public ConfigEntry<bool> StopTouchingMyConfigs;

        #endregion

        #region Debug
        public ConfigEntry<bool> DebugMode { get; set; }

        public ConfigEntry<bool> EnableTraceLogging { get; set; }

        public ConfigEntry<bool> DumpDefaultTraderItemsToFile { get; set; }

        #endregion

        public void Load(ConfigFile configFile)
        {
            EnableMod = configFile.Bind("General", "EnableMod", true, "Kill switch for disabling all mod features.");
            ClearAllExisting = configFile.Bind("General", "ClearAllExisting", true, "When enabled, all existing items for sales gets removed before adding configured.");
            StopTouchingMyConfigs = configFile.Bind<bool>("General", nameof(StopTouchingMyConfigs), false, "Disables automatic updating and saving of drop table configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");

            DebugMode = configFile.Bind("Debug", "EnableDebug", false, "Enable debug logging.");
            EnableTraceLogging = configFile.Bind("Debug", "EnableTrace", false, "Enable debug trace logging.");
            DumpDefaultTraderItemsToFile = configFile.Bind("Debug", "DumpDefaultTraderItemsToFile", false, "When Trader starts, his items will be logged in a file on disk.");
        }
    }

}

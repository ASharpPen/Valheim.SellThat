using BepInEx.Configuration;

namespace Valheim.SellThat
{
    public class DefaultConfig
    {
        #region General

        public ConfigEntry<bool> ClearAllExisting { get; set; }

        #endregion

        #region Debug
        public ConfigEntry<bool> DebugMode { get; set; }

        public ConfigEntry<bool> DumpDefaultTraderItemsToFile { get; set; }

        #endregion

        public void Load(ConfigFile configFile)
        {
            ClearAllExisting = configFile.Bind("General", "ClearAllExisting", false, "When enabled, all existing items for sales gets removed before adding configured.");

            DebugMode = configFile.Bind("Debug", "EnableDebug", false, "Enable debug logging.");

            DumpDefaultTraderItemsToFile = configFile.Bind("Debug", "DumpDefaultTraderItemsToFile", false, "When Trader starts, his items will be logged in a file on disk.");
        }
    }

}

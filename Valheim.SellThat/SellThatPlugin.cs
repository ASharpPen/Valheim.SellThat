using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;

namespace Valheim.SellThat
{
    [BepInPlugin("asharppen.valheim.sell_that", "Sell That!", "1.0.0")]
    public class SellThatPlugin : BaseUnityPlugin
    {
        public static DefaultConfig Config { get; set; }

        public static TraderSellConfig TraderConfig { get; set; }

        void Awake()
        {
            Logger.LogInfo("Loading configurations...");

            var configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "sell_that.cfg"), true);
            Config = new DefaultConfig();
            Config.Load(configFile);

            if(!Config.EnableMod.Value)
            {
                Logger.LogInfo("Mod disabled. Stopping.");
                return;
            }

            string traderConfigPath = Path.Combine(Paths.ConfigPath, "sell_that.selling.cfg");

            ConfigFile traderConfig;
            if (!File.Exists(traderConfigPath))
            {
                traderConfig = new ConfigFile(traderConfigPath, true);
                TraderSellConfigurationLoader.InitializeDefault(traderConfig);
            }
            else
            {
                traderConfig = new ConfigFile(traderConfigPath, true);
            }

            TraderSellConfigurationLoader.ScanBindings(traderConfig);
            TraderConfig = TraderSellConfigurationLoader.CreateTraderConfigFromBindings(traderConfig);

            Logger.LogInfo("Configurations loaded.");

            var harmony = new Harmony("mod.sell_that");
            harmony.PatchAll();
        }
    }
}

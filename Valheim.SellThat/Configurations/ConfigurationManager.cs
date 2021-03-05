using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Valheim.SellThat.ConfigurationCore;

namespace Valheim.SellThat.Configurations
{
    public static class ConfigurationManager
    {
        public static GeneralConfig GeneralConfig;

        public static List<TraderBuyingConfig> TraderBuyConfig;

        public static TraderSellConfig TraderSellConfig;

        private const string DefaultTraderBuyConfigFile = "sell_that.buying.cfg";

        private const string DefaultGeneralConfigFile = "sell_that.cfg";

        private const string DefaultTraderSellConfigConfile = "sell_that.selling.cfg";

        public static bool DebugOn => GeneralConfig?.DebugMode?.Value ?? false;

        public static void LoadAllConfigurations()
        {
            GeneralConfig = LoadGeneralConfig(DefaultGeneralConfigFile);

            TraderBuyConfig = LoadBuyersList(DefaultTraderBuyConfigFile);

            TraderSellConfig = LoadSellConfig(DefaultTraderSellConfigConfile);
        }

        private static GeneralConfig LoadGeneralConfig(string configName)
        {
            Log.LogDebug($"Loading general configurations from {configName}.");

            string configPath = Path.Combine(Paths.ConfigPath, configName);

            var configFile = new ConfigFile(configPath, true);

            var config = new GeneralConfig();
            config.Load(configFile);

            return config;
        }

        private static List<TraderBuyingConfig> LoadBuyersList(string configName)
        {
            Log.LogDebug($"Loading trader buying configurations from {configName}.");

            string configPath = Path.Combine(Paths.ConfigPath, configName);

            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig.StopTouchingMyConfigs.Value) configFile.SaveOnConfigSet = false;

            Dictionary<string, TraderBuyingConfig> configurations = ConfigurationLoader.LoadConfigurationGroup<TraderBuyingConfig, BuyPlaceholder>(configFile);

            return configurations.Values.ToList();
        }

        private static TraderSellConfig LoadSellConfig(string configName)
        {
            Log.LogDebug($"Loading trader selling configurations from {configName}.");

            string traderConfigPath = Path.Combine(Paths.ConfigPath, configName);

            ConfigFile traderConfig;
            if (!File.Exists(traderConfigPath))
            {
                traderConfig = new ConfigFile(traderConfigPath, true);

                if (GeneralConfig.StopTouchingMyConfigs.Value) traderConfig.SaveOnConfigSet = false;

                TraderSellConfigurationLoader.InitializeDefault(traderConfig);
            }
            else
            {
                traderConfig = new ConfigFile(traderConfigPath, true);

                if (GeneralConfig.StopTouchingMyConfigs.Value) traderConfig.SaveOnConfigSet = false;
            }

            TraderSellConfigurationLoader.ScanBindings(traderConfig);
            return TraderSellConfigurationLoader.CreateTraderConfigFromBindings(traderConfig);
        }
    }
}

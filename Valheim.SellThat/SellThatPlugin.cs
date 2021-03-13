using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat
{
    [BepInPlugin("asharppen.valheim.sell_that", "Sell That!", "1.2.0")]
    public class SellThatPlugin : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogInfo("Loading configurations...");

            ConfigurationManager.LoadAllConfigurations();

            Logger.LogInfo("Configurations loaded.");

            if(!ConfigurationManager.GeneralConfig.EnableMod.Value)
            {
                Logger.LogInfo("Mod disabled. Stopping.");
                return;
            }

            var harmony = new Harmony("mod.sell_that");
            harmony.PatchAll();
        }
    }
}

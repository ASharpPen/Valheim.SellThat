﻿using HarmonyLib;
using Valheim.SellThat.ConfigurationCore;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat.Patches
{
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.OnWorldStart))]
    public static class WorldStartPatch
    {
        private static void Postfix()
        {
            //If singleplayer, ZNet will not be initialized here.
            if (ZNet.instance == null)
            {
                //Quick'n'dirty cleanup for now.
                SellableItemsPatch.Buying = null;
                SellItemPatch.Buying = null;

                Log.LogDebug("World startet. Loading trader configurations.");
                ConfigurationManager.LoadAllConfigurations();
            }
        }
    }
}

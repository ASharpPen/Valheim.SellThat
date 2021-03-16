using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Valheim.SellThat.ConfigurationCore;
using Valheim.SellThat.Configurations;

namespace Valheim.SellThat.TraderFeatures.TraderBuy
{
    [HarmonyPatch(typeof(StoreGui))]
    public static class GetSellableItemsPatch
    {
        private static MethodInfo GetValuableItemsAnchor = AccessTools.Method(typeof(Inventory), "GetValuableItems", new Type[] { typeof(List<ItemDrop.ItemData>)});
        private static MethodInfo CustomSellables = AccessTools.Method(typeof(GetSellableItemsPatch), nameof(SellableItemsDetour), new Type[] { typeof(Inventory), typeof(List<ItemDrop.ItemData>), typeof(StoreGui) });

        public static Dictionary<string, TraderBuyingConfig> Buying = null;

        [HarmonyPatch("GetSellableItem")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> RerouteSellableItemSearch(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Callvirt, GetValuableItemsAnchor))
                .SetInstruction(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Callvirt, CustomSellables))
                .InstructionEnumeration();
        }

        private static void SellableItemsDetour(Inventory inventory, List<ItemDrop.ItemData> sellableItems, StoreGui storeGui)
        {
            if (Buying is null)
            {
                Buying = ConfigurationManager.TraderBuyConfig.ToDictionary(x => x.ItemName.Value.Trim().ToUpperInvariant());

                foreach(var key in Buying.Keys)
                {
                    Log.LogTrace($"Enabling selling of: {key}");
                }
            }

            //Future importance.
            //var trader = (Trader)AccessTools.Field(typeof(StoreGui), "m_trader").GetValue(storeGui);

            foreach(var item in inventory.GetAllItems())
            {
                string name = item.m_dropPrefab.name;

                try
                {
                    string cleanedName = name.Trim().ToUpperInvariant();

                    if (Buying.TryGetValue(cleanedName, out TraderBuyingConfig config) && config.Price.Value > 0)
                    {
                        sellableItems.Add(item);
                    }
                    else if(item.m_shared.m_value > 0)
                    {
                        sellableItems.Add(item);
                    }
                }
                catch(Exception e)
                {
                    Log.LogError("Error while checking modded buyers list.", e);
                }
            }
        }
    }
}

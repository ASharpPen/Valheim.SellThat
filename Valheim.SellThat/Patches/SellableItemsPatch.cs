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

namespace Valheim.SellThat.Patches
{
    [HarmonyPatch(typeof(StoreGui), "GetSellableItem")]
    public static class SellableItemsPatch
    {
        private static MethodInfo GetValuableItemsAnchor = AccessTools.Method(typeof(Inventory), "GetValuableItems", new Type[] { typeof(List<ItemDrop.ItemData>)});
        private static MethodInfo CustomSellables = AccessTools.Method(typeof(SellableItemsPatch), nameof(SellableItemsDetour), new Type[] { typeof(Inventory), typeof(List<ItemDrop.ItemData>), typeof(StoreGui) });

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var resultInstructions = new List<CodeInstruction>();
            var codes = instructions.ToList();

            for(int i = 0; i < codes.Count; ++i)
            {
                var curCode = codes[i];

                if(curCode.opcode == OpCodes.Callvirt && curCode.OperandIs(GetValuableItemsAnchor))
                {
                    Log.LogDebug($"Identified anchor {curCode}");
                    Log.LogDebug($"Replacing with custom detour for adding sellable items.");

                    resultInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    resultInstructions.Add(new CodeInstruction(OpCodes.Callvirt, CustomSellables));
                }
                else
                {
                    resultInstructions.Add(curCode);
                }
            }
            
            return resultInstructions;
        }

        public static Dictionary<string, TraderBuyingConfig> Buying = null;

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

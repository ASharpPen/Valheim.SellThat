using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public static class SetItemValuePatch
    {
        private static FieldInfo ItemValueAnchor = AccessTools.Field(typeof(ItemDrop.ItemData), "m_shared");

        private static MethodInfo CustomItemValue = AccessTools.Method(typeof(SetItemValuePatch), nameof(ItemValueDetour), new Type[] { typeof(ItemDrop.ItemData), typeof(StoreGui) });

        [HarmonyPatch("SellItem")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> RouteItemValueToDetour(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Ldfld, ItemValueAnchor))
                .SetInstruction(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Callvirt, CustomItemValue))
                .InstructionEnumeration();
        }

        public static Dictionary<string, TraderBuyingConfig> Buying = null;

        private static int ItemValueDetour(ItemDrop.ItemData item, StoreGui store)
        {
            if (Buying is null)
            {
                Buying = ConfigurationManager.TraderBuyConfig.ToDictionary(x => x.ItemName.Value.Trim().ToUpperInvariant());
            }

            string name = item.m_dropPrefab.name;

            string cleanedName = name.Trim().ToUpperInvariant();

            if (Buying.TryGetValue(cleanedName, out TraderBuyingConfig config))
            {
                Log.LogTrace($"Setting value for sold item '{name}:{config.Price.Value}'");

                store.m_sellButton.GetComponent<UITooltip>().Set(item.m_shared.m_name, config.Price.Value.ToString());

                return config.Price.Value;
            }
            else if(item.m_shared.m_value == 0)
            {
                Log.LogTrace($"Unable to find value for sold item '{name}'");
            }


            return item.m_shared.m_value;
        }
    }
}

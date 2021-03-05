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
    [HarmonyPatch(typeof(StoreGui), "SellItem")]
    public static class SellItemPatch
    {
        private static FieldInfo ItemValueAnchor = AccessTools.Field(typeof(ItemDrop.ItemData), "m_shared");

        private static MethodInfo CustomItemValue = AccessTools.Method(typeof(SellItemPatch), nameof(ItemValueDetour), new Type[] { typeof(ItemDrop.ItemData), typeof(StoreGui) });

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> resultInstructions = new List<CodeInstruction>();

            var codes = instructions.ToList();

            bool foundAnchor = false;

            for(int i = 0; i < codes.Count; ++i)
            {
                var code = codes[i];

                if(!foundAnchor && (foundAnchor = (code.opcode == OpCodes.Ldfld && code.OperandIs(ItemValueAnchor))))
                {
                    Log.LogDebug($"Identified anchor {code}");
                    Log.LogDebug($"Replacing anchor with custom detour for item value calculation.");

                    //Add in detour
                    resultInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    resultInstructions.Add(new CodeInstruction(OpCodes.Callvirt, CustomItemValue));

                    //Skip actual value extraction which comes right after anchor
                    ++i;
                }
                else
                {
                    resultInstructions.Add(code);
                }
            }

            foreach(var code in resultInstructions)
            {
                Log.LogTrace(code.ToString());
            }

            return resultInstructions;
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

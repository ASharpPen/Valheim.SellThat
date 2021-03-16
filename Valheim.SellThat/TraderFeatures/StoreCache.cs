using System.Runtime.CompilerServices;

namespace Valheim.SellThat.TraderFeatures
{
    public static class StoreCache
    {
        private static ConditionalWeakTable<StoreGui, StoreState> StoreStateTable = new ConditionalWeakTable<StoreGui, StoreState>();

        public static StoreState GetState(StoreGui store)
        {
            return StoreStateTable.GetOrCreateValue(store);
        }
    }
}

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SellThat.TraderFeatures
{
    public class TraderCache
    {
        private static TraderCache instance = null;

        private ConditionalWeakTable<Trader, Character> CharacterComponentTable = new ConditionalWeakTable<Trader, Character>();
        private ConditionalWeakTable<Character, ZNetView> CharacterViewTable = new ConditionalWeakTable<Character, ZNetView>();
        private ConditionalWeakTable<Trader, List<Trader.TradeItem>> TraderDefaultItemsTable = new ConditionalWeakTable<Trader, List<Trader.TradeItem>>();

        private TraderCache()
        { }

        public static TraderCache Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new TraderCache();
                }
                return instance;
            }
        }

        public Character GetCharacter(Trader trader)
        {
            Character character;

            if (CharacterComponentTable.TryGetValue(trader, out Character ch))
            {
                character = ch;
            }
            else
            {
                character = trader.gameObject.GetComponent<Character>();
                CharacterComponentTable.Add(trader, character);
            }

            return character;
        }

        public ZNetView GetZNetView(Trader trader)
        {
            var character = GetCharacter(trader);

            ZNetView view;

            if (CharacterViewTable.TryGetValue(character, out ZNetView characterView))
            {
                view = characterView;
            }
            else
            {
                view = (ZNetView)AccessTools.Field(typeof(Character), "m_nview").GetValue(character);
                CharacterViewTable.Add(character, view);
            }

            return view;
        }

        public List<Trader.TradeItem> GetDefaultItems(Trader trader)
        {
            if(TraderDefaultItemsTable.TryGetValue(trader, out List<Trader.TradeItem> cachedDefaults))
            {
                return cachedDefaults;
            }
            else
            {
                var defaultItems = trader.m_items.ToList();
                TraderDefaultItemsTable.Add(trader, defaultItems);

                return defaultItems;
            }
        }
    }
}

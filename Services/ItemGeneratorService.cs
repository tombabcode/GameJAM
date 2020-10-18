using GameJAM.Gameplay;
using GameJAM.Models;
using GameJAM.Types;
using System.Collections.Generic;
using System.Linq;
using TBEngine.Utils;

namespace GameJAM.Services {
    public static class ItemGeneratorService {

        public static List<Item> Get(Player player, ContentService content) {
            List<Item> result = new List<Item>( );

            float foodChance = player.Hunger * 0.7f + 0.1f;
            float fluidChange = player.Thirst * 0.7f + 0.1f;

            Dictionary<string, ItemObjectData> definitions = content.ItemsDefinitions;

            int foodItems = RandomService.GetRandomInt(15, 5);
            int fluidItems = RandomService.GetRandomInt(8, 3);

            List<ItemObjectData> food = definitions.Where(i => i.Value.Type == ItemType.Food).OrderBy(i => i.Value.Rarity).Select(i => i.Value).ToList( );
            List<ItemObjectData> fluid = definitions.Where(i => i.Value.Type == ItemType.Fluid).OrderBy(i => i.Value.Rarity).Select(i => i.Value).ToList( );

            result.AddRange(GetItems(food, foodItems, content));
            result.AddRange(GetItems(fluid, fluidItems, content));

            return result;
        }

        private static List<Item> GetItems(List<ItemObjectData> items, int amount, ContentService content) {
            List<Item> result = new List<Item>( );

            for (int i = 0; i < amount; i++) {
                int minimumRarity = items.Min(f => f.Rarity);
                int maximumRarity = items.Max(f => f.Rarity);
                int rarity = RandomService.GetRandomInt(maximumRarity + 1, minimumRarity);

                ItemObjectData item = items.FirstOrDefault(f => f.Rarity >= rarity);
                if (item != null)
                    result.Add(content.SpawnItem(item.ID));
            }

            return result;
        }

    }
}

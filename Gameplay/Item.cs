using System.Collections.Generic;
using GameJAM.Models;
using GameJAM.Types;

namespace GameJAM.Gameplay {
    public sealed class Item {

        public string ID { get; private set; }
        public ItemType Type { get; private set; }
        public float Weight { get; private set; }
        public int Amount { get; private set; }
        public int Rarity { get; private set; }

        public List<ItemEffectData> Effects { get; private set; } = new List<ItemEffectData>( );

        public Item(ItemObjectData model, int amount) {
            ID = model.ID;
            Type = model.Type;
            Weight = model.Weight < 0 ? .1f : model.Weight;
            Amount = amount < 1 ? 1 : amount;
            Rarity = model.Rarity < 1 ? 1 : model.Rarity;
            Effects = model.Effects ?? new List<ItemEffectData>( );;
        }

        public void Add(Item item) {
            Amount += item.Amount;
        }
        
        public void Use(Player player) {
            Effects.ForEach(effect => {
                if (effect == null) return;

                if (effect.Type == ItemEffectType.AttributeSingleUse && effect.Attribute.HasValue && effect.Value.HasValue) {
                    player.ChangeAttribute(effect.Attribute.Value, effect.Value.Value);
                    player.Inventory.Remove(this);
                }
            });
        }

    }
}
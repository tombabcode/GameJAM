using GameJAM.Models;
using GameJAM.Types;
using System;
using System.Collections.Generic;

namespace GameJAM.Gameplay {
    public sealed class Item {

        public string ID { get; private set; }
        public ItemType Type { get; private set; }
        public float Weight { get; private set; }
        public int Amount { get; private set; }

        public List<ItemEffectData> Effects { get; private set; } = new List<ItemEffectData>( );

        public Item(string id, ItemType type, List<ItemEffectData> effects, float weight = 1, int amount = 1) {
            ID = id;
            Type = type;
            Weight = weight < 0 ? .1f : weight;
            Amount = amount < 1 ? 1 : amount;
            Effects = effects ?? new List<ItemEffectData>( );;
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
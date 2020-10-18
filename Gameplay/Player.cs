using System.Linq;
using System.Collections.Generic;
using GameJAM.Types;

namespace GameJAM.Gameplay {
    public sealed class Player {

        public float MaxWeight { get; private set; }

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Tiredness { get; set; }

        public List<Item> Inventory { get; private set; } = new List<Item>( );

        public Player(float maxWeight) {
            MaxWeight = maxWeight;
        }

        public void AddItems(List<Item> items) {
            items.ForEach(item => {
                Item inv = Inventory.FirstOrDefault(i => i.ID == item.ID);
                if (inv != null)
                    inv.Add(item);
                else
                    Inventory.Add(item);
            });
        }

        public void ChangeAttribute(AttributeType type, float value) {
            switch (type) {
                case AttributeType.Hunger: Hunger = Hunger + value > 1 ? Hunger = 1 : Hunger + value < 0 ? Hunger = 0 : Hunger + value; break;
                case AttributeType.Thirst: Thirst = Thirst + value > 1 ? Thirst = 1 : Thirst + value < 0 ? Thirst = 0 : Thirst + value; break;
                case AttributeType.Tiredness: Tiredness = Tiredness + value > 1 ? Tiredness = 1 : Tiredness + value < 0 ? Tiredness = 0 : Tiredness + value; break;
            }
        }

    }
}
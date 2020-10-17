using System.Collections.Generic;
using System.Linq;

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
                Item inv = Inventory.FirstOrDefault(i => i.Name == item.Name);
                if (inv != null)
                    inv.Add(item);
                else
                    Inventory.Add(item);
            });
        }

    }
}
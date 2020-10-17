using GameJAM.Types;

namespace GameJAM.Gameplay {
    public sealed class Item {

        public string Name { get; private set; }
        public ItemType Type { get; private set; }
        public float Weight { get; private set; }
        public int Amount { get; private set; }

        public Item(string name, ItemType type, float weight = 1, int amount = 1) {
            Name = name;
            Type = type;
            Weight = weight < 0 ? .1f : weight;
            Amount = amount < 1 ? 1 : amount;
        }

        public void Add(Item item) {
            Amount += item.Amount;
        }
        

    }
}
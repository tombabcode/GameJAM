using GameJAM.Types;

namespace GameJAM.Gameplay {
    public sealed class Item {

        public string Name { get; private set; }
        public ItemType Type { get; private set; }
        public int SkinID { get; private set; }
        public int Amount { get; private set; }

        public Item(string name, ItemType type, int skinID, int amount = 1) {
            Name = name;
            Type = type;
            SkinID = skinID;
            Amount = amount < 1 ? 1 : amount;
        }
        

    }
}
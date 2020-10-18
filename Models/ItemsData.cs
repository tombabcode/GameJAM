using GameJAM.Types;
using System.Collections.Generic;

namespace GameJAM.Models {
    public sealed class ItemsData {
        public List<ItemObjectData> Items { get; set; } = new List<ItemObjectData>( );
    }

    public sealed class ItemObjectData {
        public string ID { get; set; }
        public ItemType Type { get; set; }
        public float Weight { get; set; }
        public int Rarity { get; set; }
        public List<ItemEffectData> Effects { get; set; } = new List<ItemEffectData>( );
    }

    public sealed class ItemEffectData {
        public ItemEffectType? Type { get; set; }
        public AttributeType? Attribute { get; set; }
        public float? Value { get; set; }
    }

}

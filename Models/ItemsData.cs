using GameJAM.Types;
using System.Collections.Generic;

namespace GameJAM.Models {
    public sealed class ItemsData {
        public List<ItemObjectData> Items { get; set; } = new List<ItemObjectData>( );
    }

    public sealed class ItemObjectData {
        public string ID { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public float Weight { get; set; }
        public int SkinID { get; set; }
    }

}

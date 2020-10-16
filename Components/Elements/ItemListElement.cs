using GameJAM.Gameplay;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components.Elements {
    public sealed class ItemListElement : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentDataService _content;
        private InputService _input;

        private List<Item> _items = new List<Item>( );

        private RenderTarget2D _resultScene;

        private Item _hoveredItem;

        public float ItemsWeight => _items.Sum(item => item.Weight * item.Amount);

        public ItemListElement(ContentDataService content, InputService input, List<Item> items, int width, int height) {
            _items = items ?? new List<Item>( );
            _content = content;
            _input = input;

            _resultScene = new RenderTarget2D(content.Device, width, height);
        }

        public void Update( ) {
            _hoveredItem = null;

            for (int i = 0; i < _items.Count; i++)
                if (_input.IsOver(AbsoluteX, AbsoluteY + i * 40, _content.TexGUIItemOutline.Width, _content.TexGUIItemOutline.Height)) {
                    _hoveredItem = _items[i];
                    break;
                }
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                if (_items.Count == 0) {
                    DH.Text(_content.FontRegular, "Inventory is empty", _resultScene.Width / 2, _resultScene.Height / 2, align: AlignType.CM);
                    return;
                }

                for (int i = 0; i < _items.Count; i++) {
                    Item item = _items[i];
                    DH.Raw(_content.TexGUIItemOutline, 0, i * 40, color: Color.White * (_hoveredItem == item ? .75f : .25f));
                    DH.Text(_content.FontSmall, item.Name + (item.Amount > 1 ? $" ({item.Amount}x)" : ""), 52, i * 40 + 20, align: AlignType.LM);
                }
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}

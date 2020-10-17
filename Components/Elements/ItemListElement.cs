using GameJAM.Gameplay;
using GameJAM.Services;
using GameJAM.Types;
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

        private ContentService _content;
        private InputService _input;

        private List<Item> _items = new List<Item>( );
        public List<Item> SelectedItems { get; private set; } = new List<Item>( );

        private RenderTarget2D _resultScene;

        private Item _hoveredItem;

        public float SelectedItemsWeight => SelectedItems.Sum(item => item.Weight * item.Amount);

        public ItemListElement(ContentService content, InputService input, List<Item> items, int width, int height) {
            _items = items ?? new List<Item>( );
            SelectedItems = new List<Item>( );
            _content = content;
            _input = input;

            _resultScene = new RenderTarget2D(content.Device, width, height);
        }

        public void Update( ) {
            _hoveredItem = null;

            for (int i = 0; i < _items.Count; i++)
                if (_input.IsOver(AbsoluteX, AbsoluteY + i * 24, _resultScene.Width, 24)) {
                    _hoveredItem = _items[i];
                    break;
                }

            if (_hoveredItem != null && _input.IsLMBPressedOnce( ))
                if (SelectedItems.Contains(_hoveredItem))
                    SelectedItems.Remove(_hoveredItem);
                else
                    SelectedItems.Add(_hoveredItem);
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                if (_items.Count == 0) {
                    DH.Text(_content.FontRegular, "Inventory is empty", _resultScene.Width / 2, _resultScene.Height / 2, align: AlignType.CM);
                    return;
                }

                for (int i = 0; i < _items.Count; i++) {
                    Item item = _items[i];
                    Color outlineColor;
                    bool isSelected = SelectedItems.Contains(item);
                    bool isHovered = _hoveredItem == item;

                    if (isHovered && isSelected) outlineColor = Color.LightGreen;
                    else if (!isHovered && isSelected) outlineColor = Color.Green;
                    else if (isHovered && !isSelected) outlineColor = Color.White * .75f; 
                    else outlineColor = Color.White * .4f;

                    string amount = item.Type == ItemType.Fluid ? $" ({(item.Amount / 10f):0.0} l)" : (item.Amount > 1 ? $" ({item.Amount}x)" : "");
                    string weight = $"{(item.Weight * item.Amount):0.0}" + (item.Type == ItemType.Fluid ? " l" : " kg");

                    DH.Text(_content.FontSmall, item.Name + amount, 12, i * 24 + 12, outlineColor, AlignType.LM);
                    DH.Text(_content.FontTiny, weight, _resultScene.Width - 12, i * 24 + 12, outlineColor, AlignType.RM);
                }
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);


    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using GameJAM.Types;
using GameJAM.Services;
using GameJAM.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;
using LANG = TBEngine.Utils.TranslationService;

namespace GameJAM.Components.Elements {
    public sealed class ItemListElement : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }
        public bool IsSelectingAvailable { get; set; } = true;

        private ContentService _content;
        private InputService _input;

        private List<Item> _items = new List<Item>( );
        public List<Item> SelectedItems { get; private set; } = new List<Item>( );

        private RenderTarget2D _resultScene;

        private Item _hoveredItem;
        private Item _lastHovered;

        public Action<Item> OnHover { private get; set; }
        public Action<Item> OnRMBClick { private get; set; }

        private bool _hasScrollbar => _totalSize > _resultScene.Height;
        private int _totalSize => _items.Count * 24;
        private float _scrollOffset = 0;
        private int _viewableElements => (int)Math.Floor(_resultScene.Height / 24f);

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

            for (int i = 0; i < _items.Count; i++) {
                if (_input.IsOver(AbsoluteX, AbsoluteY + i * 24 - _scrollOffset, _resultScene.Width, 24)) {
                    _hoveredItem = _items[i];
                    break;
                }
            }

            if (_hoveredItem != null) {
                if (_input.IsRMBPressedOnce( )) OnRMBClick?.Invoke(_hoveredItem);

                if (IsSelectingAvailable && _input.IsLMBPressedOnce( )) {
                    if (SelectedItems.Contains(_hoveredItem))
                        SelectedItems.Remove(_hoveredItem);
                    else
                        SelectedItems.Add(_hoveredItem);
                }
            }

            if (_lastHovered != _hoveredItem) {
                OnHover?.Invoke(_hoveredItem);
                _lastHovered = _hoveredItem;
            }

            if (_totalSize > _resultScene.Height) {
                if (_input.HasScrolledUp( )) _scrollOffset -= 24;
                if (_input.HasScrolledDown( )) _scrollOffset += 24;
                if (_scrollOffset < 0) _scrollOffset = 0;
                if (_scrollOffset + _resultScene.Height > _totalSize) _scrollOffset = _totalSize - _resultScene.Height;
            }
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                if (_items.Count == 0) {
                    DH.Text(_content.FontRegular, "inventory_empty", _resultScene.Width / 2, _resultScene.Height / 2, align: AlignType.CM);
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
                    string weight = $"{(item.Weight * item.Amount):0.00}" + (item.Type == ItemType.Fluid ? " l" : " kg");

                    DH.Text(_content.FontSmall, LANG.Get(item.ID) + amount, 8, i * 24 + 12 - _scrollOffset, false, outlineColor, AlignType.LM);
                    DH.Text(_content.FontTiny, weight, _resultScene.Width - (_hasScrollbar ? 16 : 8), i * 24 + 12 - _scrollOffset, false, outlineColor, AlignType.RM);

                    if (_hasScrollbar) {
                        float scrollbarHeight = (_viewableElements / (float)_items.Count) * _resultScene.Height;
                        DH.Box(_resultScene.Width - 8, (int)((_scrollOffset / (_totalSize - _resultScene.Height)) * (_resultScene.Height - scrollbarHeight)), 2, (int)scrollbarHeight, color: Color.Gray);
                    }
                }
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);


    }
}

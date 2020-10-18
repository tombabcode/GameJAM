using System;
using System.Collections.Generic;
using GameJAM.Gameplay;
using GameJAM.Services;
using GameJAM.Components.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components {
    public sealed class JourneyComponent : IComponent {
        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private ItemListElement _itemListElement;

        private Action _onClose;
        private Player _player;

        private Button _backButton;

        private bool _isOverweight;

        public JourneyComponent(ContentService content, InputService input, ConfigurationService config, Action onClose, Action<Item> onItemHover, Player player) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;
            _player = player;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            List<Item> itemsFound = new List<Item>( );
            itemsFound.Add(content.SpawnItem("blackberry"));
            itemsFound.Add(content.SpawnItem("mushroom_leccinum"));
            itemsFound.Add(content.SpawnItem("water", 10));

            _itemListElement = new ItemListElement(_content, _input, itemsFound, _config.ViewWidth - 16, _config.ViewHeight - 196) {
                AbsoluteX = AbsoluteX + 8,
                AbsoluteY = AbsoluteY + 108,
                OnHover = (Item hovered) => onItemHover?.Invoke(hovered)
            };

            _backButton = new Button( ) {
                Text = "back_home",
                X = _resultScene.Width / 2,
                Y = _resultScene.Height - 8,
                Width = _resultScene.Width - 16,
                Height = 32,
                ButtonAlign = AlignType.CB,
                OnClick = ( ) => {
                    if (_itemListElement.SelectedItemsWeight > _player.MaxWeight)
                        return;
                    _player.AddItems(_itemListElement.SelectedItems); 
                    onClose?.Invoke( ); 
                }
            };
        }

        public void Update( ) {
            if (_input.IsKeyPressedOnce(Keys.Escape) && _itemListElement.SelectedItemsWeight <= _player.MaxWeight) { 
                _player.AddItems(_itemListElement.SelectedItems);
                _onClose?.Invoke( );
            }

            _itemListElement.Update( );
            _backButton.Update(_input);

            _isOverweight = _itemListElement.SelectedItemsWeight > _player.MaxWeight;
            _backButton.TextColor = _isOverweight ? Color.Red : Color.White;
        }

        public void Render( ) {
            _itemListElement.Render( );

            DH.RenderScene(_resultScene, () => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);
                DH.Text(_content.FontSmall, "journey_1", _resultScene.Width / 2, 52, align: AlignType.CT);
                DH.Text(_content.FontSmall, "journey_2", _resultScene.Width / 2, 74, align: AlignType.CT);

                _itemListElement.Display( );

                DH.Text(_content.FontSmall, $"{_itemListElement.SelectedItemsWeight:0.0}kg / {_player.MaxWeight:0.0}kg", _resultScene.Width / 2, _resultScene.Height - 48,
                    false, _isOverweight ? Color.Red : Color.DarkGray, AlignType.CB);

                _backButton.Display(_content);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
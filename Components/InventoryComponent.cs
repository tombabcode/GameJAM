using System;
using System.Collections.Generic;
using GameJAM.Components.Elements;
using GameJAM.Gameplay;
using GameJAM.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;
using CFG = GameJAM.Types.ConfigType;

namespace GameJAM.Components {
    public sealed class InventoryComponent : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private ItemListElement _itemListElement;

        private Button _closeButton;

        public InventoryComponent(ContentService content, InputService input, ConfigurationService config, Action onClose, Action<Item> onItemHover, Player player) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            _itemListElement = new ItemListElement(_content, _input, player.Inventory, _config.ViewWidth - 16, _config.ViewHeight - 96) {
                AbsoluteX = AbsoluteX + 8,
                AbsoluteY = AbsoluteY + 88,
                IsSelectingAvailable = false,
                OnRMBClick = (Item actionItem) => actionItem.Use(player),
                OnHover = (Item item) => onItemHover?.Invoke(item)
            };

            _closeButton = new Button( ) {
                Text = "X",
                TextTranslate = false,
                X = _config.ViewWidth - 4,
                Y = 4,
                ButtonAlign = AlignType.RT,
                Width = 24,
                Height = 32,
                OnClick = onClose
            };
        }

        public void Update( ) {
            if (_input.IsKeyPressedOnce(Keys.Escape) || _input.IsKeyPressedOnce(_config.KEY_Inventory))
                _onClose?.Invoke( );

            _itemListElement.Update( );
            _closeButton.Update(_input);
        }

        public void Render( ) {
            _itemListElement.Render( );

            DH.RenderScene(_resultScene, ( ) => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);
                DH.Text(_content.FontSmall, "inventory", _resultScene.Width / 2, 64, align: AlignType.CM);

                _closeButton.Display(_content);
                _itemListElement.Display( );
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
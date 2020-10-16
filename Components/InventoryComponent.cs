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

namespace GameJAM.Components {
    public sealed class InventoryComponent : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentDataService _content;
        private InputService _input;
        private ConfigurationDataService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private ItemListElement _itemListElement;

        private Button _closeButton;

        public InventoryComponent(ContentDataService content, InputService input, ConfigurationDataService config, Action onClose, List<Item> items) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);

            _itemListElement = new ItemListElement(_content, _input, items, _config.WindowWidth - 16, _config.WindowHeight - 56) {
                AbsoluteX = AbsoluteX + 16,
                AbsoluteY = AbsoluteY + 48
            };

            _closeButton = new Button( ) {
                Text = "X",
                X = _config.WindowWidth - 4,
                Y = 4,
                ButtonAlign = AlignType.RT,
                Width = 24,
                Height = 32,
                OnClick = onClose
            };
        }

        public void Update( ) {
            if (_input.IsKeyPressedOnce(Keys.Escape))
                _onClose?.Invoke( );

            _itemListElement.Update( );
            _closeButton.Update(_input);
        }

        public void Render( ) {
            _itemListElement.Render( );

            DH.RenderScene(_resultScene, ( ) => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);
                DH.Text(_content.FontSmall, "Inventory", _resultScene.Width / 2, 20, align: AlignType.CM);

                _closeButton.Display(_content);
                _itemListElement.Display( );
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
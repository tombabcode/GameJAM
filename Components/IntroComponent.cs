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
    public sealed class IntroComponent : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private int _state = 0;

        public IntroComponent(ContentService content, InputService input, ConfigurationService config, Action onClose) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);
        }

        public void Update( ) {
            if (_input.IsKeyPressedOnce(Keys.Space))
                _state++;

            if (_state == 1)
                _onClose?.Invoke( );
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                if (_state >= 0)
                    DH.Text(_content.FontSmall, "intro_0", _resultScene.Width / 2, _resultScene.Height - 16, align: AlignType.CB);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
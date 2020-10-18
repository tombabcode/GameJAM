using System;
using GameJAM.Services;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;

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
            if (_input.IsAnyKeyPressedOnce( ) && _state == 0)
                _state++;

            if (_state == 1)
                _onClose?.Invoke( );
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                if (_state >= 0)
                    DH.Text(_content.FontSmall, "intro_0", _resultScene.Width / 2, _resultScene.Height /2, align: AlignType.CM);

                DH.Text(_content.FontTiny, "continue", _resultScene.Width / 2, _resultScene.Height - 8, align: AlignType.CB);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
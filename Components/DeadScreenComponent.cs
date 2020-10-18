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
using Microsoft.Xna.Framework;

namespace GameJAM.Components {
    public sealed class DeadScreenComponent : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;
        private long _score;

        public DeadScreenComponent(ContentService content, InputService input, ConfigurationService config, Action onClose, long score) {
            _content = content;
            _input = input;
            _config = config;
            _score = score;
            _onClose = onClose;
            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);
        }

        public void Update( ) {
            if (_input.IsAnyKeyPressedOnce( ))
                _onClose?.Invoke( );
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                DH.Box(0, 0, _resultScene.Width, _resultScene.Height, color: Color.Black);
                DH.Text(_content.FontTiny, "continue", _resultScene.Width / 2, _resultScene.Height - 8, align: AlignType.CB);

                DH.Text(_content.FontBig, "dead_0", _resultScene.Width / 2, _resultScene.Height / 2, align: AlignType.CM);
                DH.Text(_content.FontSmall, $"{_score:00000000}", _resultScene.Width / 2, _resultScene.Height / 2 + 48, translate: false, align: AlignType.CM);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
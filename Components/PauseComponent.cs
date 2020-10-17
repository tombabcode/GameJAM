using System;
using System.Collections.Generic;
using GameJAM.Components.Elements;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components {
    public sealed class PauseComponent : IComponent {
        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private List<Button> _buttons;

        private Action _onResume;

        public PauseComponent(ContentService content, InputService input, ConfigurationService config, Action onClose, Action onSettings, Action onTutorial, Action onResume) {
            _content = content;
            _input = input;
            _config = config;
            _onResume = onResume;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            _buttons = new List<Button>( ) {
                new Button( ) { Text = "End game", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 + 160,
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onClose },
                new Button( ) { Text = "Settings", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 + 56,
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onSettings },
                new Button( ) { Text = "How to play", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2,
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onTutorial },
                new Button( ) { Text = "Resume", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 - 56,
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onResume }
            };
        }

        public void Update( ) {
            if (_input.IsKeyPressedOnce(Keys.Escape))
                _onResume?.Invoke( );

            foreach (Button btn in _buttons)
                btn.Update(_input);
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, () => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);

                foreach (Button btn in _buttons)
                    btn.Display(_content);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
using System;
using System.Collections.Generic;
using GameJAM.Components.Elements;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components {
    public sealed class MainMenuComponent : IComponent {
        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentDataService _content;
        private InputService _input;
        private ConfigurationDataService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private List<Button> _buttons;

        public MainMenuComponent(ContentDataService content, InputService input, ConfigurationDataService config, Action onClose, Action onPlay, Action onLeave) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);

            _buttons = new List<Button>( ) {
                new Button( ) { Text = "Leave", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 + 160, 
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onLeave },
                new Button( ) { Text = "Settings", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 + 56,
                    Width = _resultScene.Width - 64, Height = 48 },
                new Button( ) { Text = "How to play", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2,
                    Width = _resultScene.Width - 64, Height = 48 },
                new Button( ) { Text = "Play", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 - 56,
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onPlay }
            };
        }

        public void Update( ) {
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
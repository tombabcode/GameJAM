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

        private ContentDataService _content;
        private InputService _input;
        private ConfigurationDataService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private List<Button> _buttons;

        public MainMenuComponent(ContentDataService content, InputService input, ConfigurationDataService config, Action onClose) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);

            _buttons = new List<Button>( ) {
                new Button( ) {
                    Text = "Leave",
                    ButtonAlign = AlignType.CB,
                    X = _resultScene.Width / 2,
                    Y = _resultScene.Height / 2 + 32,
                    Width = _resultScene.Width - 64,
                    Height = 48,
                    BackgroundColor = Color.Red
                },
                new Button( ) {
                    Text = "Settings",
                    ButtonAlign = AlignType.CM,
                    X = _resultScene.Width / 2,
                    Y = _resultScene.Height / 2 - 24,
                    Width = _resultScene.Width - 64,
                    Height = 48,
                    BackgroundColor = Color.Red
                },
                new Button( ) {
                    Text = "Play",
                    ButtonAlign = AlignType.CB,
                    X = _resultScene.Width / 2,
                    Y = _resultScene.Height / 2 - 32,
                    Width = _resultScene.Width - 64,
                    Height = 48,
                    BackgroundColor = Color.Red
                }
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

        public void Display(int x, int y) => DH.Scene(_resultScene, x, y);

    }
}
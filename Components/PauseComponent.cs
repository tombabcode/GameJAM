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
    public sealed class PauseComponent : IComponent {
        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentDataService _content;
        private InputService _input;
        private ConfigurationDataService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private List<Button> _buttons;

        public PauseComponent(ContentDataService content, InputService input, ConfigurationDataService config, Action onClose, Action onLeaveButton) {
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
                    Y = _resultScene.Height - 64,
                    Width = _resultScene.Width - 64,
                    Height = 32,
                    OnClick = onLeaveButton,
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

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
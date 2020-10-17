using System;
using System.Collections.Generic;
using GameJAM.Components.Elements;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;
using LANG = TBEngine.Utils.TranslationService;

namespace GameJAM.Components {
    public sealed class MainMenuComponent : IComponent {
        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private List<Button> _buttons;

        public MainMenuComponent(ContentService content, InputService input, ConfigurationService config, Action onClose, Action onSettings, Action onPlay, Action onLeave) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            _buttons = new List<Button>( ) {
                new Button( ) { Text = "leave", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 + 160, 
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onLeave },
                new Button( ) { Text = "settings", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 + 56,
                    Width = _resultScene.Width - 64, Height = 48, OnClick = onSettings },
                new Button( ) { Text = "tutorial", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2,
                    Width = _resultScene.Width - 64, Height = 48 },
                new Button( ) { Text = "play", ButtonAlign = AlignType.CT, X = _resultScene.Width / 2, Y = _resultScene.Height / 2 - 56,
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

                DH.Text(_content.FontTiny, "copyright", _resultScene.Width / 2, _resultScene.Height - 4, color: new Color(60, 60, 60), align: AlignType.CB);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
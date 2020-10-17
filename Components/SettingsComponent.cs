using System;
using GameJAM.Components.Elements;
using GameJAM.Gameplay;
using GameJAM.Services;
using GameJAM.Types;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components {
    public sealed class SettingsComponent : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private Button _backButton;

        private SliderElement _slider;

        public SettingsComponent(ContentService content, InputService input, ConfigurationService config, Action onBack) {
            _content = content;
            _input = input;
            _config = config;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            _backButton = new Button( ) { Text = "back", X = 16, Y = _resultScene.Height - 16, ButtonAlign = AlignType.LB, Width = 96, Height = 32, OnClick = onBack };

            _slider = new SliderElement(_content, _input, (float output) => ChangeWindowSize(output)) {
                AbsoluteX = _resultScene.Width / 2,
                AbsoluteY = _resultScene.Height / 2,
                Name = "scale",
                Minimum = 1,
                Maximum = _config.MaxScale
            };
        }

        public void Update( ) {
            _slider.Update( );
            _backButton.Update(_input);
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);

                _slider.Display( );

                _backButton.Display(_content);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

        public void ChangeWindowSize(float scale) {
            _config.Configuration[ConfigType.WindowScale.ToString( ).ToLower( )] = $"{scale:0.0}";
            _content.Graphics.PreferredBackBufferWidth = _config.WindowWidth;
            _content.Graphics.PreferredBackBufferHeight = _config.WindowHeight;
            _content.Graphics.ApplyChanges( );
        }

    }
}
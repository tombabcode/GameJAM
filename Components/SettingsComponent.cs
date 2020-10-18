using System;
using GameJAM.Components.Elements;
using GameJAM.Gameplay;
using GameJAM.Services;
using GameJAM.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Types;
using TBEngine.Utils;
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
        private Button _langPLButton;
        private Button _langENButton;

        private SliderElement _slider;

        public SettingsComponent(ContentService content, InputService input, ConfigurationService config, Action onBack) {
            _content = content;
            _input = input;
            _config = config;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            _backButton = new Button( ) { Text = "back", X = 16, Y = _resultScene.Height - 16, ButtonAlign = AlignType.LB, Width = 96, Height = 32, OnClick = onBack };
            _langENButton = new Button( ) { Text = "english", X = 90, Y = _resultScene.Height / 2 + 64, ButtonAlign = AlignType.CM, Width = 160, Height = 32, OnClick = ( ) => {
                TranslationService.LoadTranslations("en");
                _langENButton.TextColor = Color.White;
                _langPLButton.TextColor = new Color(60, 60, 60);
                _config.Configuration[ConfigType.Language.ToString( ).ToLower( )] = "en";
                _config.SaveConfiguration( );
            }, TextColor = TranslationService.CurrentLanguage == "en" ? Color.White : new Color(60, 60, 60) };
            _langPLButton = new Button( ) { Text = "polish", X = 270, Y = _resultScene.Height / 2 + 64, ButtonAlign = AlignType.CM, Width = 160, Height = 32, OnClick = ( ) => {
                TranslationService.LoadTranslations("pl");
                _langENButton.TextColor = new Color(60, 60, 60);
                _langPLButton.TextColor = Color.White;
                _config.Configuration[ConfigType.Language.ToString( ).ToLower( )] = "pl";
                _config.SaveConfiguration( );
            }, TextColor = TranslationService.CurrentLanguage == "pl" ? Color.White : new Color(60, 60, 60) };

            _slider = new SliderElement(_content, _input, (_config.Scale - 1) / (_config.MaxScale - 1), (float output) => ChangeWindowSize(output)) {
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
            _langENButton.Update(_input);
            _langPLButton.Update(_input);
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);

                _slider.Display( );

                _backButton.Display(_content);
                _langPLButton.Display(_content);
                _langENButton.Display(_content);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

        public void ChangeWindowSize(float scale) {
            scale = scale * (_config.MaxScale - 1) + 1;
            scale = scale < 1 ? 1 : scale > _config.MaxScale ? _config.MaxScale : scale;
            _config.Configuration[ConfigType.WindowScale.ToString( ).ToLower( )] = $"{scale:0.0}";
            _config.CheckScale(_content.Device);
            _config.SaveConfiguration( );
            _content.Graphics.PreferredBackBufferWidth = _config.WindowWidth;
            _content.Graphics.PreferredBackBufferHeight = _config.WindowHeight;
            _content.Graphics.ApplyChanges( );
        }

    }
}
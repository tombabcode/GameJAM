using System;
using GameJAM.Components.Elements;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components {
    public sealed class TutorialComponent : IComponent {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private RenderTarget2D _resultScene;

        private Action _onClose;

        private int _state = 0;

        private Button _restButton;
        private Button _wanderButton;
        private Button _inventoryButton;

        public TutorialComponent(ContentService content, InputService input, ConfigurationService config, Action onClose) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            _restButton = new Button( ) { Text = "rest", X = 300, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB };
            _wanderButton = new Button( ) { Text = "wander", X = 180, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB };
            _inventoryButton = new Button( ) { Text = "inventory", X = 60, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB };
        }

        public void Update( ) {
            if (_input.IsAnyKeyPressedOnce( ))
                _state++;

            if (_state == 4)
                _onClose?.Invoke( );

            if (_state == 1) _restButton.Update(_input);
            if (_state == 2) _wanderButton.Update(_input);
            if (_state == 3) _inventoryButton.Update(_input);
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);

                if (_state == 0) {
                    DH.Text(_content.FontSmall, "continue", _resultScene.Width / 2, _resultScene.Height - 16, color: Color.Gray, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_0", _resultScene.Width / 2, _resultScene.Height / 2 - 28, align: AlignType.CB);
                    DH.Text(_content.FontRegular, "tutorial_1", _resultScene.Width / 2, _resultScene.Height / 2 + 6, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_2", _resultScene.Width / 2, _resultScene.Height / 2 + 28, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_3", _resultScene.Width / 2, _resultScene.Height / 2 + 56, align: AlignType.CB);
                    DH.Text(_content.FontRegular, "tutorial_4", _resultScene.Width / 2, _resultScene.Height / 2 + 84, align: AlignType.CB);

                    DH.Box(8, 8, _resultScene.Width - 16, 48, color: Color.White * .25f);
                    DH.Text(_content.FontTiny, "thirst", 100, 12, align: AlignType.CT);
                    DH.Text(_content.FontTiny, "hunger", 180, 12, align: AlignType.CT);
                    DH.Text(_content.FontTiny, "tiredness", 260, 12, align: AlignType.CT);
                    DH.Text(_content.FontSmall, "12.8%", 100, 26, translate: false, align: AlignType.CT);
                    DH.Text(_content.FontSmall, "60.2%", 180, 26, translate: false, align: AlignType.CT);
                    DH.Text(_content.FontSmall, "100.0%", 260, 26, translate: false, color: Color.Red, align: AlignType.CT);
                } else if (_state == 1) {
                    DH.Text(_content.FontSmall, "tutorial_5", _resultScene.Width / 2, _resultScene.Height / 2 - 28, align: AlignType.CB);
                    DH.Text(_content.FontTiny, "tutorial_6", _resultScene.Width / 2, _resultScene.Height / 2 + 6, align: AlignType.CB);

                    _restButton.Display(_content);
                } else if (_state == 2) {
                    DH.Text(_content.FontSmall, "tutorial_7", _resultScene.Width / 2, _resultScene.Height / 2 - 28, align: AlignType.CB);
                    DH.Text(_content.FontTiny, "tutorial_8", _resultScene.Width / 2, _resultScene.Height / 2 + 6, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_13", _resultScene.Width / 2, _resultScene.Height / 2 + 28, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_14", _resultScene.Width / 2, _resultScene.Height / 2 + 56, align: AlignType.CB);
                    DH.Text(_content.FontRegular, "tutorial_15", _resultScene.Width / 2, _resultScene.Height / 2 + 84, align: AlignType.CB);

                    _wanderButton.Display(_content);
                } else if (_state >= 3) {
                    DH.Text(_content.FontSmall, "tutorial_9", _resultScene.Width / 2, _resultScene.Height / 2 - 28, align: AlignType.CB);
                    DH.Text(_content.FontRegular, "tutorial_10", _resultScene.Width / 2, _resultScene.Height / 2 + 6, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_11", _resultScene.Width / 2, _resultScene.Height / 2 + 28, align: AlignType.CB);
                    DH.Text(_content.FontSmall, "tutorial_12", _resultScene.Width / 2, _resultScene.Height / 2 + 56, align: AlignType.CB);

                    _inventoryButton.Display(_content);
                }
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
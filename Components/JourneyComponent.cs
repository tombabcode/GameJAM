using System;
using GameJAM.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components {
    public sealed class JourneyComponent : IComponent {

        private ContentDataService _content;
        private InputService _input;
        private ConfigurationDataService _config;

        private RenderTarget2D _resultScene;
        private RenderTarget2D _itemListScene;

        private Action _onClose;

        public JourneyComponent(ContentDataService content, InputService input, ConfigurationDataService config, Action onClose) {
            _content = content;
            _input = input;
            _config = config;
            _onClose = onClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);
            _itemListScene = new RenderTarget2D(_content.Device, _config.WindowWidth - 16, _config.WindowHeight - 48);
        }

        public void Update( ) {
            if (_input.IsKeyPressedOnce(Keys.Escape))
                _onClose?.Invoke( );
        }

        public void Render( ) {
            DH.RenderScene(_itemListScene, () => {
            });

            DH.RenderScene(_resultScene, () => {
                DH.TransparentBox(0, 0, _resultScene.Width, _resultScene.Height, .75f);
                DH.Text(_content.FontSmall, "During my wanderings I found...", _resultScene.Width / 2, 16, align: AlignType.CT);

                DH.Scene(_itemListScene, 8, 40);
            });
        }

        public void Display(int x, int y) => DH.Scene(_resultScene, x, y);

    }
}
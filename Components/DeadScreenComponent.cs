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

        public DeadScreenComponent(ContentService content, InputService input, ConfigurationService config, Action onClose) {
            _content = content;
            _input = input;
            _config = config;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);
        }

        public void Update( ) {
        }

        public void Render( ) {
            DH.RenderScene(_resultScene, ( ) => {
                DH.Box(0, 0, _resultScene.Width, _resultScene.Height, color: Color.Black);
            });
        }

        public void Display( ) => DH.Scene(_resultScene, AbsoluteX, AbsoluteY);

    }
}
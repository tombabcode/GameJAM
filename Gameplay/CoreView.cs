using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TBEngine.Services;
using GameJAM.Services;
using GameJAM.Types;
using GameJAM.Components;

using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Gameplay {
    public sealed class CoreView {

        private InputService _input;
        private ContentDataService _content;
        private ConfigurationDataService _config;
        private Action _onExit;

        private GameStateType _state;

        private Player _player;

        private IComponent _component;

        public CoreView(InputService input, ContentDataService content, ConfigurationDataService config, Action onExit) {
            _input = input;
            _content = content;
            _config = config;

            _onExit = onExit;

            _state = GameStateType.MainMenu;
            _component = new MainMenuComponent(_content, _input, _config, ComponentClosure);
        }

        public void NewGame( ) {
            _player = new Player(40);
            _state = GameStateType.Gameplay;
        }

        public void Update(GameTime gameTime) {

            if (_component == null) {
                if (_input.IsKeyPressedOnce(Keys.Escape)) _component = new PauseComponent(_content, _input, _config, ComponentClosure, () => _onExit?.Invoke( ));
                if (_input.IsKeyPressedOnce(Keys.Space)) _component = new JourneyComponent(_content, _input, _config, ComponentClosure);
            } else
                _component.Update( );
            
            _input.Update( );
        }

        public void Display( ) {
            if (_component != null)
                _component.Render( );

            DH.RenderScene(( ) => {
                DH.Raw(_content.Background);

                if (_component != null)
                    _component.Display(0, 0);
            });
        }

        private void ComponentClosure( ) => _component = null;

    }
}
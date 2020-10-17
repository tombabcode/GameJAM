using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TBEngine.Services;
using GameJAM.Services;
using GameJAM.Components;

using DH = TBEngine.Utils.DisplayHelper;
using System.Collections.Generic;
using GameJAM.Components.Elements;
using TBEngine.Types;
using Microsoft.Xna.Framework.Graphics;

namespace GameJAM.Gameplay {
    public sealed class CoreView {

        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;
        private Action _onExit;

        private Player _player;

        private IComponent _component;
        private List<Button> _buttons;

        private RenderTarget2D _resultScene;

        private Action _onAppClose;

        public CoreView(InputService input, ContentService content, ConfigurationService config, Action onAppClose) {
            _input = input;
            _content = content;
            _config = config;

            _onAppClose = onAppClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            OpenMainMenu( );
            _buttons = new List<Button>( ) {
                new Button( ) { Text = "Stuff", X = 60, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB, OnClick = OpenInventory },
                new Button( ) { Text = "Wander", X = 180, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB },
                new Button( ) { Text = "Rest", X = 300, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB }
            };
        }

        public void NewGame( ) {
            _component = null;
            _player = new Player(40);
        }

        public void Update(GameTime gameTime) {
            if (_component == null) {
                if (_input.IsKeyPressedOnce(Keys.Escape)) Pause( );
                if (_input.IsKeyPressedOnce(Keys.Space)) Journey( );
                if (_input.IsKeyPressedOnce(_config.KEY_Inventory)) OpenInventory( );

                _buttons.ForEach(btn => btn.Update(_input));
            } else
                _component.Update( );
            
            _input.Update( );
        }

        public void Display( ) {
            if (_component != null)
                _component.Render( );

            DH.RenderScene(_resultScene, ( ) => {
                DH.Raw(_content.Background);

                if (_component != null)
                    _component.Display( );
                else
                    _buttons.ForEach(btn => btn.Display(_content));
            });

            // Display everything
            DH.RenderScene(( ) => DH.Scene(_resultScene, 0, 0, _config.WindowWidth, _config.WindowHeight));
        }

        private void ComponentClosure( ) => _component = null;
        private void OpenMainMenu( ) => _component = new MainMenuComponent(_content, _input, _config, ComponentClosure, NewGame, _onAppClose);
        private void OpenInventory( ) => _component = new InventoryComponent(_content, _input, _config, ComponentClosure, _player.Inventory);
        private void Pause( ) => _component = new PauseComponent(_content, _input, _config, OpenMainMenu, null, null, ComponentClosure);
        private void Journey( ) => _component = new JourneyComponent(_content, _input, _config, ComponentClosure, _player);

    }
}
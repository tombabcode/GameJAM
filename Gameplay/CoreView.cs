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

namespace GameJAM.Gameplay {
    public sealed class CoreView {

        private InputService _input;
        private ContentDataService _content;
        private ConfigurationDataService _config;
        private Action _onExit;

        private Player _player;
        private List<Item> _inventoryItems;

        private IComponent _component;
        private List<Button> _buttons;

        public CoreView(InputService input, ContentDataService content, ConfigurationDataService config, Action onExit) {
            _input = input;
            _content = content;
            _config = config;

            _onExit = onExit;

            _component = new MainMenuComponent(_content, _input, _config, ComponentClosure, NewGame, onExit);
            _buttons = new List<Button>( ) {
                new Button( ) {
                    Text = "Stuff",
                    X = 60, Y = _config.WindowHeight,
                    Width = 120, Height = 64,
                    ButtonAlign = AlignType.CB,
                    OnClick = OpenInventory
                },
                new Button( ) {
                    Text = "Wander",
                    X = 180, Y = _config.WindowHeight,
                    Width = 120, Height = 64,
                    ButtonAlign = AlignType.CB
                },
                new Button( ) {
                    Text = "Rest",
                    X = 300, Y = _config.WindowHeight,
                    Width = 120, Height = 64,
                    ButtonAlign = AlignType.CB
                }
            };
        }

        public void NewGame( ) {
            _component = null;
            _player = new Player(40);

            _inventoryItems = new List<Item>( );
        }

        public void Update(GameTime gameTime) {
            if (_component == null) {
                if (_input.IsKeyPressedOnce(Keys.Escape)) _component = new PauseComponent(_content, _input, _config, ComponentClosure, () => _onExit?.Invoke( ));
                if (_input.IsKeyPressedOnce(Keys.Space)) _component = new JourneyComponent(_content, _input, _config, ComponentClosure);
                if (_input.IsKeyPressedOnce(Keys.I)) OpenInventory( );

                _buttons.ForEach(btn => btn.Update(_input));
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
                    _component.Display( );
                else
                    _buttons.ForEach(btn => btn.Display(_content));
            });
        }

        private void ComponentClosure( ) => _component = null;
        private void OpenInventory( ) => _component = new InventoryComponent(_content, _input, _config, ComponentClosure, _inventoryItems);

    }
}
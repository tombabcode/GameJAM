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
using TBEngine.Utils;
using GameJAM.Models;
using GameJAM.Types;

namespace GameJAM.Gameplay {
    public sealed class CoreView {

        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;

        private Player _player;

        private IComponent _component;
        private List<Button> _buttons;

        private RenderTarget2D _resultScene;

        private Action _onAppClose;

        private bool _animRun => _componentSwitchDirection != 0;
        private int _componentSwitchDirection;
        private float _componentSwitchValue;
        private Action _onComponentSwitch;

        private Item _itemHovered;

        private long _score;

        public CoreView(InputService input, ContentService content, ConfigurationService config, Action onAppClose) {
            _input = input;
            _content = content;
            _config = config;

            _onAppClose = onAppClose;

            _resultScene = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            OpenMainMenu( );
            _buttons = new List<Button>( ) {
                new Button( ) { Text = "inventory", X = 60, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB, OnClick = OpenInventory },
                new Button( ) { Text = "wander", X = 180, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB, OnClick = Journey },
                new Button( ) { Text = "rest", X = 300, Y = _config.ViewHeight, Width = 120, Height = 64, ButtonAlign = AlignType.CB, OnClick = Sleep }
            };
        }

        public void NewGame( ) {
            _component = null;
            _player = new Player(40);
            _score = 0;
            _itemHovered = null;
        }

        public void Update(GameTime gameTime) {
            if (_component == null) {
                if (_input.IsKeyPressedOnce(Keys.Escape)) StartAnim(Pause);
                if (_input.IsKeyPressedOnce(Keys.Space)) Journey( );
                if (_input.IsKeyPressedOnce(_config.KEY_Inventory)) OpenInventory( );

                _buttons.ForEach(btn => btn.Update(_input));
            } else if (!_animRun)
                _component.Update( );

            if (_animRun) {
                _componentSwitchValue += _componentSwitchDirection * (float)(2 * gameTime.ElapsedGameTime.TotalSeconds);
                if (_componentSwitchValue > 1) {
                    _componentSwitchValue = _componentSwitchDirection;
                    _onComponentSwitch?.Invoke( );
                    _componentSwitchDirection *= -1;
                } else if (_componentSwitchValue < 0) {
                    _componentSwitchValue = 0;
                    _componentSwitchDirection = 0;
                    _onComponentSwitch = null;
                }
            }
            
            _input.Update( );
        }

        public void Display( ) {
            if (_component != null)
                _component.Render( );

            DH.RenderScene(_resultScene, BlendState.NonPremultiplied, ( ) => {
                DH.Raw(_content.TexBackgroundSky);
                DH.Raw(_content.TexBackgroundStars);
                DH.Raw(_content.TexBackground);

                if (_component != null)
                    _component.Display( );
                else 
                    _buttons.ForEach(btn => btn.Display(_content));

                int third = _resultScene.Width / 3;

                if (_player != null && (_component is InventoryComponent || _component is JourneyComponent || _component is null)) {
                    DH.Text(_content.FontTiny, "thirst", 100, 12, align: AlignType.CT);
                    DH.Text(_content.FontTiny, "hunger", 180, 12, align: AlignType.CT);
                    DH.Text(_content.FontTiny, "tiredness", 260, 12, align: AlignType.CT);

                    if (_itemHovered != null) {
                        float newThirst = _player.Thirst;
                        float newHunger = _player.Hunger;
                        float newTiredness = _player.Tiredness;
                        foreach (ItemEffectData effect in _itemHovered.Effects) {
                            if (effect.Attribute == AttributeType.Hunger) newHunger += effect.Value.Value;
                            if (effect.Attribute == AttributeType.Thirst) newThirst += effect.Value.Value;
                            if (effect.Attribute == AttributeType.Tiredness) newTiredness += effect.Value.Value;
                        }
                        if (newThirst < 0) newThirst = 0; else if (newThirst > 1) newThirst = 1;
                        if (newHunger < 0) newHunger = 0; else if (newHunger > 1) newHunger = 1;
                        if (newTiredness < 0) newTiredness = 0; else if (newTiredness > 1) newTiredness = 1;
                        DH.Text(_content.FontSmall, $"{(newThirst * 100):0.0}%", 100, 26, false, newThirst > _player.Thirst ? Color.Red : newThirst < _player.Thirst ? Color.Green : Color.White, AlignType.CT);
                        DH.Text(_content.FontSmall, $"{(newHunger * 100):0.0}%", 180, 26, false, newHunger > _player.Hunger ? Color.Red : newHunger < _player.Hunger ? Color.Green : Color.White, AlignType.CT);
                        DH.Text(_content.FontSmall, $"{(newTiredness * 100):0.0}%", 260, 26, false, newTiredness > _player.Tiredness ? Color.Red : newTiredness < _player.Tiredness ? Color.Green : Color.White, AlignType.CT);
                    } else {
                        DH.Text(_content.FontSmall, $"{(_player.Thirst * 100):0.0}%", 100, 26, translate: false, align: AlignType.CT);
                        DH.Text(_content.FontSmall, $"{(_player.Hunger * 100):0.0}%", 180, 26, translate: false, align: AlignType.CT);
                        DH.Text(_content.FontSmall, $"{(_player.Tiredness * 100):0.0}%", 260, 26, translate: false, align: AlignType.CT);
                    }
                }

                if (_animRun)
                    DH.Box(0, 0, _config.WindowWidth, _config.WindowHeight, Color.Black * (float)Math.Pow(_componentSwitchValue, 2));

                DH.Raw(_content.TexGUIBorder);
                DH.Raw(_content.Cursor, _input.MouseX, _input.MouseY);
            });

            // Display everything
            DH.RenderScene(( ) => DH.Scene(_resultScene, 0, 0, _config.WindowWidth, _config.WindowHeight));
        }

        private void StartAnim(Action onSwitch) {
            _componentSwitchDirection = 1;
            _onComponentSwitch = onSwitch;
        }

        private void ComponentClosure( ) => _component = null;
        private void OpenMainMenu( ) => _component = new MainMenuComponent(_content, _input, _config, ComponentClosure, SettingsMainMenu, TutorialMainMenu, Intro, _onAppClose);
        private void OpenInventory( ) => _component = new InventoryComponent(_content, _input, _config, ( ) => { _itemHovered = null; ComponentClosure( ); }, (Item hovered) => _itemHovered = hovered, _player);
        private void Intro( ) => StartAnim(( ) => _component = new IntroComponent(_content, _input, _config, () => StartAnim(NewGame)));
        private void Pause( ) => _component = new PauseComponent(_content, _input, _config, OpenMainMenu, SettingsGameplay, TutorialGameplay, ComponentClosure);
        private void TutorialMainMenu( ) => _component = new TutorialComponent(_content, _input, _config, OpenMainMenu);
        private void TutorialGameplay( ) => _component = new TutorialComponent(_content, _input, _config, Pause);
        private void Journey( ) => StartAnim(( ) => _component = new JourneyComponent(_content, _input, _config, 
            ( ) => { 
                ComponentClosure( );
                _itemHovered = null;
                float tiredness = _player.Tiredness;
                _player.Tiredness += RandomService.GetRandomFloat(.1f, .5f); 
                if (_player.Tiredness > 1) _player.Tiredness = 1;

                _score += (long)((_player.Tiredness - tiredness) * 1000);
            }, 
            (Item hovered) => _itemHovered = hovered, _player));
        private void Sleep( ) => StartAnim(( ) => {
            if (_player.Tiredness == 1 || _player.Hunger == 1 || _player.Thirst == 1) {
                Die( );
                return;
            }

            float thirst = _player.Thirst;
            float hunger = _player.Hunger;

            _player.Tiredness -= RandomService.GetRandomFloat(.5f, .25f);
            _player.Thirst += RandomService.GetRandomFloat(.1f, .25f);
            _player.Hunger += RandomService.GetRandomFloat(.1f, .35f);

            if (_player.Tiredness < 0) _player.Tiredness = 0;
            if (_player.Hunger > 1) _player.Hunger = 1;
            if (_player.Thirst > 1) _player.Thirst = 1;

            _score += (long)((_player.Thirst - thirst) * 1000);
            _score += (long)((_player.Hunger - hunger) * 1000);
            Console.WriteLine(_score);
        });
        private void SettingsGameplay( ) => _component = new SettingsComponent(_content, _input, _config, Pause);
        private void SettingsMainMenu( ) => _component = new SettingsComponent(_content, _input, _config, OpenMainMenu);
        private void Die( ) => _component = new DeadScreenComponent(_content, _input, _config, () => StartAnim(OpenMainMenu), _score);

    }
}
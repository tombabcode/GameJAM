using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using GameJAM.Gameplay;
using TBEngine.Utils;

using CFG = GameJAM.Types.ConfigType;
using GameJAM.Services;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameJAM {
    public sealed class Core : Game {
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _canvas;

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;

        private CoreView _gameplay;
        
        public Core( ) {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Assets";
            IsMouseVisible = false;
            Window.Title = "TBCODE | Mini Jam 65 itch.io";
            Window.AllowUserResizing = false;
            Window.AllowAltF4 = false;
        }

        protected override void Initialize( ) {
            base.Initialize( );

            _canvas = new SpriteBatch(GraphicsDevice);

            _config = new ConfigurationService( );
            _content = new ContentService(_graphics, Content, GraphicsDevice, _canvas);
            _input = new InputService(_config);

            _config.Add(CFG.WindowScale, 1.0f.ToString( ));
            _config.Add(CFG.KEY_Inventory, Keys.I.ToString( ));
            _config.Add(CFG.Language, "en");
            _config.LoadConfiguration( );

            _config.CheckScale(GraphicsDevice);

            TranslationService.LoadTranslations(_config.Language);
            RandomService.Random = new Random( );
            DisplayHelper.Content = _content;

            _graphics.PreferredBackBufferWidth = _config.WindowWidth;
            _graphics.PreferredBackBufferHeight = _config.WindowHeight;
            _graphics.ApplyChanges( );

            _content.LoadContent( );

            _gameplay = new CoreView(_input, _content, _config, Exit);
        }

        protected override void Update(GameTime gameTime) {
            _gameplay.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _gameplay.Display( );
            base.Draw(gameTime);
        }

    }
}
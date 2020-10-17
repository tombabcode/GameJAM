using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using GameJAM.Gameplay;
using TBEngine.Utils;

using CFG = GameJAM.Types.ConfigType;
using GameJAM.Services;
using Microsoft.Xna.Framework.Input;

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
            IsMouseVisible = true;
        }

        protected override void Initialize( ) {
            base.Initialize( );

            _canvas = new SpriteBatch(GraphicsDevice);

            _config = new ConfigurationService( );
            _content = new ContentService(Content, GraphicsDevice, _canvas);
            _input = new InputService(_config);

            _config.Add(CFG.WindowScale, 1.5f.ToString( ));
            _config.Add(CFG.KEY_Inventory, Keys.I.ToString( ));
            _config.LoadConfiguration( );

            _config.CheckScale(GraphicsDevice);

            _gameplay = new CoreView(_input, _content, _config, Exit);

            DisplayHelper.Content = _content;

            _graphics.PreferredBackBufferWidth = _config.WindowWidth;
            _graphics.PreferredBackBufferHeight = _config.WindowHeight;
            _graphics.ApplyChanges( );

            _content.LoadContent( );
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
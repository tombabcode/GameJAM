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

        private ContentDataService _content;
        private InputService _input;
        private ConfigurationDataService _config;

        private CoreView _gameplay;
        
        public Core( ) {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Assets";
            IsMouseVisible = true;
        }

        protected override void Initialize( ) {
            base.Initialize( );

            _canvas = new SpriteBatch(GraphicsDevice);

            _config = new ConfigurationDataService( );
            _content = new ContentDataService(Content, GraphicsDevice, _canvas);
            _input = new InputService( );

            _config.Add(CFG.WindowWidth, "360");
            _config.Add(CFG.WindowHeight, "640");
            _config.Add(CFG.KEY_Inventory, Keys.I.ToString( ));
            _config.LoadConfiguration( );

            _gameplay = new CoreView(_input, _content, _config, Exit);

            DisplayHelper.Content = _content;

            _graphics.PreferredBackBufferWidth = _config.GetInt(CFG.WindowWidth);
            _graphics.PreferredBackBufferHeight = _config.GetInt(CFG.WindowHeight);
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
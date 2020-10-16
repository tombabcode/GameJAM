using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;

namespace GameJAM {
    public sealed class Core : Game {
        
        private SpriteBatch _canvas;

        private ContentService _content;
        private InputService _input;
        private ConfigurationService _config;
        
        public Core( ) {
            _ = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
        }

        protected override void Initialize( ) {
            base.Initialize( );

            _canvas = new SpriteBatch(GraphicsDevice);

            _config = new ConfigurationService( );
            _content = new ContentService(Content, GraphicsDevice, _canvas);
            _input = new InputService( );
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }

    }
}
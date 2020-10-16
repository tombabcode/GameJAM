using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using TBEngine.Textures;

namespace GameJAM.Services {
    public sealed class ContentDataService : ContentService {

        public SpriteFont FontRegular { get; private set; }
        public SpriteFont FontSmall { get; private set; }

        public Texture2D Background { get; private set; }
        public TextureTileset TexHungerStatus { get; private set; }
        
        public ContentDataService(ContentManager content, GraphicsDevice device, SpriteBatch canvas) : base(content, device, canvas) { }

        public override void LoadContent( ) {
            FontRegular = _content.Load<SpriteFont>(Path.Combine("Fonts", "regular"));
            FontSmall = _content.Load<SpriteFont>(Path.Combine("Fonts", "small"));
            
            Background = Texture2D.FromFile(Device, Path.Combine("Assets", "background.png"));

            TexHungerStatus = new TextureTileset(Texture2D.FromFile(Device, Path.Combine("Assets", "hunger.png")), 8, 1);
        }

    }
}
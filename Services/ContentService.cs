using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using GameJAM.Gameplay;
using GameJAM.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using TBEngine.Textures;

namespace GameJAM.Services {
    public sealed class ContentService : ContentServiceBase {

        public GraphicsDeviceManager Graphics { get; private set; }

        public SpriteFont FontBig { get; private set; }
        public SpriteFont FontRegular { get; private set; }
        public SpriteFont FontSmall { get; private set; }
        public SpriteFont FontTiny { get; private set; }

        public Texture2D TexBackground { get; private set; }
        public Texture2D TexBackgroundSky { get; private set; }
        public Texture2D TexBackgroundStars { get; private set; }
        public TextureTileset TexHungerStatus { get; private set; }

        public Texture2D Cursor { get; private set; }

        public Texture2D TexGUIBorder { get; private set; }

        public Dictionary<string, ItemObjectData> ItemsDefinitions { get; private set; }
        
        public ContentService(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device, SpriteBatch canvas) : base(content, device, canvas) {
            Graphics = graphics;
        }

        public override void LoadContent( ) {
            FontBig = _content.Load<SpriteFont>(Path.Combine("Fonts", "big"));
            FontRegular = _content.Load<SpriteFont>(Path.Combine("Fonts", "regular"));
            FontSmall = _content.Load<SpriteFont>(Path.Combine("Fonts", "small"));
            FontTiny = _content.Load<SpriteFont>(Path.Combine("Fonts", "tiny"));

            Cursor = Texture2D.FromFile(Device, Path.Combine("Assets", "cursor.png"));

            TexBackground = Texture2D.FromFile(Device, Path.Combine("Assets", "background.png"));
            TexBackgroundSky = Texture2D.FromFile(Device, Path.Combine("Assets", "background_sky.png"));
            TexBackgroundStars = Texture2D.FromFile(Device, Path.Combine("Assets", "background_stars.png"));

            TexGUIBorder = Texture2D.FromFile(Device, Path.Combine("Assets", "GUI", "border.png"));

            TexHungerStatus = new TextureTileset(Texture2D.FromFile(Device, Path.Combine("Assets", "hunger.png")), 8, 1);

            ItemsDefinitions = new Dictionary<string, ItemObjectData>( );
            var options = new JsonSerializerOptions( );
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            ItemsData itemsDefs = JsonSerializer.Deserialize<ItemsData>(File.ReadAllText(Path.Combine("Assets", "items_data.json")), options);
            if (itemsDefs != null && itemsDefs.Items != null && itemsDefs.Items.Count > 0)
                foreach (ItemObjectData data in itemsDefs.Items)
                    ItemsDefinitions.Add(data.ID, data);
        }

        public Item SpawnItem(string name, int amount = 1) {
            if (ItemsDefinitions.ContainsKey(name.ToLower( ))) {
                ItemObjectData data = ItemsDefinitions[name];
                return new Item(data.ID, data.Type, data.Effects, data.Weight, amount);
            }

            return null;
        }

    }
}
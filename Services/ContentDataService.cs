using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameJAM.Gameplay;
using GameJAM.Models;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;
using TBEngine.Textures;

namespace GameJAM.Services {
    public sealed class ContentDataService : ContentService {

        public SpriteFont FontBig { get; private set; }
        public SpriteFont FontRegular { get; private set; }
        public SpriteFont FontSmall { get; private set; }

        public Texture2D Background { get; private set; }
        public TextureTileset TexHungerStatus { get; private set; }

        public Texture2D TexGUIItemOutline { get; private set; }

        public Dictionary<string, ItemObjectData> ItemsDefinitions { get; private set; }
        
        public ContentDataService(ContentManager content, GraphicsDevice device, SpriteBatch canvas) : base(content, device, canvas) { }

        public override void LoadContent( ) {
            FontBig = _content.Load<SpriteFont>(Path.Combine("Fonts", "big"));
            FontRegular = _content.Load<SpriteFont>(Path.Combine("Fonts", "regular"));
            FontSmall = _content.Load<SpriteFont>(Path.Combine("Fonts", "small"));
            
            Background = Texture2D.FromFile(Device, Path.Combine("Assets", "background.png"));

            TexGUIItemOutline = Texture2D.FromFile(Device, Path.Combine("Assets", "GUI", "item_border.png"));

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

        public Item SpawnItem(string name) {
            if (ItemsDefinitions.ContainsKey(name.ToLower( ))) {
                ItemObjectData data = ItemsDefinitions[name];
                return new Item(data.Name, data.Type, 1);
            }

            return null;
        }

    }
}
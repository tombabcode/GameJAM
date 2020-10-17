using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TBEngine.Services;
using CFG = GameJAM.Types.ConfigType;

namespace GameJAM {
    public sealed class ConfigurationService : ConfigurationServiceBase {

        private const int WIDTH = 360;
        private const int HEIGHT = 640;

        public int ViewWidth => WIDTH;
        public int ViewHeight => HEIGHT;
        public int WindowWidth => (int)(WIDTH * Scale);
        public int WindowHeight => (int)(HEIGHT * Scale);
        public float Scale => GetFloat(CFG.WindowScale);
        public float MaxScale;

        public string Language => Get(CFG.Language);

        public Keys KEY_Inventory => Enum.TryParse(Get(CFG.KEY_Inventory), out Keys result) ? result : default;

        public void CheckScale(GraphicsDevice device) {
            if (device == null || device.Adapter == null || device.Adapter.CurrentDisplayMode == null)
                Configuration[CFG.WindowScale.ToString( )] = 1.0f.ToString( );

            float maxWidthScale = (device.Adapter.CurrentDisplayMode.Width / (float)WIDTH);
            float maxHeightScale = (device.Adapter.CurrentDisplayMode.Height / (float)HEIGHT);
            float scale = GetFloat(CFG.WindowScale);

            MaxScale = maxWidthScale >= maxHeightScale ? maxWidthScale : maxHeightScale;
        
            if (scale < 1) Configuration[CFG.WindowScale.ToString( )] = 1.0f.ToString( ); else
            if (maxWidthScale >= maxHeightScale && scale > maxWidthScale) Configuration[CFG.WindowScale.ToString( )] = maxWidthScale.ToString( ); else
            if (maxHeightScale > maxWidthScale && scale > maxHeightScale) Configuration[CFG.WindowScale.ToString( )] = maxHeightScale.ToString( );
        }

    }
}
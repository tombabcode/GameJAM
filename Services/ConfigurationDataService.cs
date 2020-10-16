using Microsoft.Xna.Framework.Input;
using System;
using TBEngine.Services;
using CFG = GameJAM.Types.ConfigType;

namespace GameJAM {
    public sealed class ConfigurationDataService : ConfigurationService {

        public int WindowWidth => GetInt(CFG.WindowWidth);
        public int WindowHeight => GetInt(CFG.WindowHeight);

        public Keys KEY_Inventory => Enum.TryParse(Get(CFG.KEY_Inventory), out Keys result) ? result : default;

    }
}
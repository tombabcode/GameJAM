using TBEngine.Services;
using CFG = GameJAM.Types.ConfigType;

namespace GameJAM {
    public sealed class ConfigurationDataService : ConfigurationService {

        public int WindowWidth => GetInt(CFG.WindowWidth);
        public int WindowHeight => GetInt(CFG.WindowHeight);

    }
}
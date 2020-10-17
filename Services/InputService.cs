using TBEngine.Services;

namespace GameJAM.Services {
    public sealed class InputService : InputServiceBase {

        private ConfigurationService _config;

        public InputService(ConfigurationService config) {
            _config = config;
        }

        public override int MouseX => (int)(_currentMouse.X / _config.Scale);
        public override int MouseY => (int)(_currentMouse.Y / _config.Scale);

    }
}

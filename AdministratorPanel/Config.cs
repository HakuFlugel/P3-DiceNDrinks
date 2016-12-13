using System.Collections.Generic;
using Shared;

namespace AdministratorPanel
{
    public class Config : ControllerBase
    {
        private Dictionary<string, object> config;

        private static Config _config;
        public static Config getConfig()
        {
            if (_config == null)
            {
                _config = new Config();
            }

            return _config;
        }

        private Config()
        {
            load();
        }

        public override void save()
        {
            saveFile("config", config);
        }

        public override void load()
        {
            config = loadFile<string, object>("config");
        }
    }
}
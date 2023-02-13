using Microsoft.AspNetCore.Identity;
using PowerTradingApplication.Modal;

namespace PowerTradingApplication.Config
{
    public static class ConfigurationSetup
    {
        public const string Config = "MyConfig";
        public static MyConfig myConfig { get; set; } = new MyConfig();
    }
}

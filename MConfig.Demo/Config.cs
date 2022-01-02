using MConfig.Deserialization;
using MConfig.Serialization;

namespace MConfig.Demo
{
    public class Config : ConfigBase
    {
        public int MagicNumber { get => _magicNumber.Value; set => _magicNumber.SetValue(value); }
        public string AppName { get => _appName.Value; set => _appName.SetValue(value); }
        public DateTime DateTime { get => _dateTime.Value; set => _dateTime.SetValue(value); }

        private readonly ConfigProperty<int> _magicNumber;
        private readonly ConfigProperty<string> _appName;
        private readonly ConfigProperty<DateTime> _dateTime;

        // Constructor needs to be parameterless for deserialization to work.
        public Config()
        {
            _magicNumber = new ConfigProperty<int>(nameof(MagicNumber), 42, (n) => n >= 42, this);
            _appName = new ConfigProperty<string>(nameof(AppName), "TestApp", (s) => !string.IsNullOrWhiteSpace(s), this);
            _dateTime = RegisterProperty("DateTime", DateTime.Now);
        }

        //Example factory method with dependency injection.
        public static Config Deserialize(/*ILogger logger*/)
        {
            string json = "{}"; //Serialized json
            var newConfig = Deserialize<Config>(new JsonConfigDeserializer(), json) ?? new Config();
            // Set DI field:
            //newConfig._logger = logger;
            return newConfig;
        }

        //This method is called by ConfigBase.Save() when 'SaveOnPropertyChanged' is true and config property is changed.
        //Can also be called manually.
        //Input string is created by ConfigBase.Serializer which is null by default.
        public override void Save(string? serializedConfig)
        {
            //Leave empty if not required.
        }
    }
}

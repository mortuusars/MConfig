using MConfig.Deserialization;

namespace MConfig.Demo
{
    internal class Config : ConfigBase
    {
        public int MagicNumber { get => _magicNumber.Value; set => _magicNumber.SetValue(value); }
        public string AppName { get => _appName.Value; set => _appName.SetValue(value); }
        public DateTime DateTime { get => _dateTime.Value; set => _dateTime.SetValue(value); }

        private readonly ConfigProperty<int> _magicNumber;
        private readonly ConfigProperty<string> _appName;
        private readonly ConfigProperty<DateTime> _dateTime;

        public Config()
        {
            _magicNumber = new ConfigProperty<int>(nameof(MagicNumber), 42, (n) => n >= 42, this);
            _appName = new ConfigProperty<string>(nameof(AppName), "TestApp", (s) => !string.IsNullOrWhiteSpace(s), this);
            _dateTime = RegisterProperty("DateTime", DateTime.Now);
        }

        public override void Save(string? serializedConfig)
        {
            if (serializedConfig is not null)
                Console.WriteLine("Saved config:\n" + serializedConfig);
        }

        public static Config Deserialize()
        {
            var deserializer = new JsonByPropertyDeserializer(msg => Console.WriteLine(msg));
            return Deserialize<Config>(deserializer, "") ?? new Config();
        }
    }
}

using System;

namespace Mortuus.Config.Tests
{
    public class ConfigForTesting : ConfigBase
    {
        public int Number { get => _number.Value; set => _number.SetValue(value); }
        private ConfigProperty<int> _number;

        public string Str { get => _str.Value; set => _str.SetValue(value); }
        private ConfigProperty<string> _str;

        public ConfigForTesting()
        {
            _number = RegisterProperty<int>("Number", 10);
            _str = RegisterProperty<string>("Str", "Testing");
        }

        public override void Save(string? serializedConfig)
        {
            throw new NotImplementedException();
        }
    }
}

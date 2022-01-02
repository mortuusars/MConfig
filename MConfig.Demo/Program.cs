using MConfig;
using MConfig.Demo;
using MConfig.Deserialization;
using MConfig.Serialization;
using System.Text.Json;

var config = new Config();
config.MagicNumber = 999;
var options = new JsonSerializerOptions() { WriteIndented = true };
config.Serializer = new JsonConfigSerializer(msg => Console.WriteLine($"[Serialization] {msg}")) { JsonSerializerOptions = options };
string? json = config.Serialize();

if (json is null)
    Console.WriteLine("Json");
else
{
    json = json.Replace("\"MagicNumber\": 999,", "");
    var newCfg = Config.Deserialize<Config>(new JsonByPropertyDeserializer(msg => Console.WriteLine($"[Deserialization] {msg}")), json);
}

Console.ReadLine();
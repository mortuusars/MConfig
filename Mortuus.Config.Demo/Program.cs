using Mortuus.Config.Deserialization;
using Mortuus.Config.Serialization;

namespace Mortuus.Config.Demo;

public class Program
{
    public static void Main(string[] args)
    {
        //Create default:
        Config defaultConfig = new Config();
        defaultConfig.PropertyChanged += (s, e) => Console.WriteLine(e.PropertyName); //Any client can subscribe to Config.PropertyChanged to receive updates.

        //Deserialize from string:
        string json = "{ \"MagicNumber\":9999,\"AppName\":\"Example\",\"DateTime\":\"2046-01-02T13:48:46.4464316+02:00\"}";
        Config deserializedConfig = Config.Deserialize<Config>(new JsonConfigDeserializer(), json) ?? new Config(); //Result of deserialization can be null if something fails.

        //Serialize to string:
        deserializedConfig.Serializer = new JsonConfigSerializer(); //Specify a serializer (it's null be default).
        var serializedString = deserializedConfig.Serialize();

        Console.ReadLine();
    }
}

using MConfig.Deserialization;
using MConfig.Serialization;
using Xunit;

namespace MConfig.Tests;

public class DeserializerTests
{
    [Fact]
    public void DefaultJsonDeserializerShouldDeserializeProperly()
    {
        var config = new ConfigForTesting();
        config.Serializer = new JsonConfigSerializer();
        config.Number = 99;
        string json = config.Serialize() ?? "";

        var newCfg = ConfigForTesting.Deserialize<ConfigForTesting>(new JsonConfigDeserializer(), json);

        Assert.Equal(99, newCfg!.Number);
    }

    [Fact]
    public void JsonByPropertyDeserializerShouldDeserializeProperly()
    {
        var config = new ConfigForTesting();
        config.Serializer = new JsonConfigSerializer();
        config.Number = 99;
        config.Str = "Changed";
        string json = config.Serialize() ?? "";

        var newCfg = ConfigForTesting.Deserialize<ConfigForTesting>(new JsonConfigDeserializer(), json);

        Assert.Equal(99, newCfg!.Number);
        Assert.Equal("Changed", newCfg!.Str);
    }

    [Fact]
    public void JsonByPropertyDeserializerShouldDeserializeWhatsAvailable()
    {
        var config = new ConfigForTesting();
        config.Serializer = new JsonConfigSerializer();
        config.Number = 99;
        config.Str = "Changed";
        string json = config.Serialize() ?? "";

        // Remove one property from json.
        json = json.Replace("\"Number\":99,", ""); 

        var newCfg = ConfigBase.Deserialize<ConfigForTesting>(new JsonConfigDeserializer(), json);

        Assert.Equal(10, newCfg!.Number);
        Assert.Equal("Changed", newCfg!.Str);
    }
}
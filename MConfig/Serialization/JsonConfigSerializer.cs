using System.Text.Json;

namespace MConfig.Serialization;

public class JsonConfigSerializer : IConfigSerializer
{
    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    private Action<string> _errorMessageCallback;

    public JsonConfigSerializer(Action<string> errorMessageCallback)
    {
        _errorMessageCallback = errorMessageCallback;
        JsonSerializerOptions = new JsonSerializerOptions();
    }

    public JsonConfigSerializer() : this((s) => { }) { }

    public string? Serialize<T>(T config) where T : ConfigBase
    {
        try
        {
            return JsonSerializer.Serialize(config, config.GetType(), JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            _errorMessageCallback($"Failed to serialize '{typeof(T)}': {ex.Message}");
            return null;
        }
    }
}
using System.Text.Json;

namespace MConfig.Serialization;

/// <summary>
/// Allows serialization of configs.
/// </summary>
public class JsonConfigSerializer : IConfigSerializer
{
    /// <summary>
    /// Options that serializer will use.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    private Action<string> _errorMessageCallback;

    /// <summary>
    /// Creates instance of serializer with callback action for error messages.
    /// </summary>
    /// <param name="errorMessageCallback"></param>
    public JsonConfigSerializer(Action<string> errorMessageCallback)
    {
        _errorMessageCallback = errorMessageCallback;
        JsonSerializerOptions = new JsonSerializerOptions();
    }

    /// <summary>
    /// Creates instance of serializer.
    /// </summary>
    public JsonConfigSerializer() : this((s) => { }) { }

    /// <summary>
    /// Serializes config instance to json string. Uses json options defined in <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <typeparam name="T">Type of config.</typeparam>
    /// <param name="config">Config instance.</param>
    /// <returns>Serialized json string.</returns>
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
using System.Text.Json;

namespace Mortuus.Config.Deserialization;

/// <summary>
/// Allows deserailization of a json string to instance of config type (derived from <see cref="ConfigBase"/>).
/// </summary>
public class JsonConfigDeserializer : IConfigDeserializer
{
    private readonly Action<string> _errorMessageCallback;

    /// <summary>
    /// Creates an instance of Deserializer.
    /// </summary>
    public JsonConfigDeserializer() : this((s) => { }) { }
    /// <summary>
    /// Creates an instance of Deserializer with a string callback action for error messages.
    /// </summary>
    public JsonConfigDeserializer(Action<string> errorMessageCallback) => _errorMessageCallback = errorMessageCallback;

    /// <summary>
    /// Deserializes config from serialized json input string.
    /// </summary>
    /// <typeparam name="T">Type of config.</typeparam>
    /// <param name="input">Serialized json string.</param>
    /// <returns>Deserialized config or null if failed.</returns>
    public T? Deserialize<T>(string input)
    {        
        try
        {
            return JsonSerializer.Deserialize<T>(input);
        }
        catch (Exception ex)
        {
            _errorMessageCallback(ex.ToString());
            return default(T);
        }
    }
}
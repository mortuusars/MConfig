namespace Mortuus.Config.Deserialization;

/// <summary>
/// Provides functionality to Deserialized config.
/// </summary>
public interface IConfigDeserializer
{
    /// <summary>
    /// Deserializes input string to a TConfig instance.
    /// </summary>
    /// <typeparam name="T">Type of config (derived from <see cref="ConfigBase"/>).</typeparam>
    /// <param name="input">String to deserialize.</param>
    /// <returns>Deserialized instance of type T.</returns>
    T? Deserialize<T>(string input);
}

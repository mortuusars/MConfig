namespace MConfig.Serialization;

/// <summary>
/// Allows serialization of config class.
/// </summary>
public interface IConfigSerializer
{
    /// <summary>
    /// Serializes TConfig to a string.
    /// </summary>
    /// <typeparam name="T">Type of config.</typeparam>
    /// <param name="config">Config instance to serialize.</param>
    /// <returns>Serialized string.</returns>
    string? Serialize<T>(T config) where T : ConfigBase;
}
using System.Reflection;
using System.Text.Json;

namespace MConfig.Deserialization;

public class JsonByPropertyDeserializer : IConfigDeserializer
{
    private readonly Action<string> _errorMessageCallback;

    /// <summary>
    /// Creates an instance of Deserializer.
    /// </summary>
    public JsonByPropertyDeserializer() : this((s) => { }) { }
    /// <summary>
    /// Creates an instance of Deserializer with a string callback action for error messages.
    /// </summary>
    public JsonByPropertyDeserializer(Action<string> errorMessageCallback) => _errorMessageCallback = errorMessageCallback;

    public T? Deserialize<T>(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            _errorMessageCallback("Input string was null or empty.");
            return default(T);
        }

        PropertyInfo[] configPropertiesInfo;

        try { configPropertiesInfo = (typeof(T)).GetProperties(); }
        catch (Exception ex)
        {
            _errorMessageCallback($"Getting properties info of type '{typeof(T)}' failed: {ex.Message}");
            return default(T);
        }

        Dictionary<string, object>? loadedProperties = DeserializeProperties(input, configPropertiesInfo);

        if (loadedProperties is null) return default(T);
        else if (loadedProperties.Count == 0)
        {
            _errorMessageCallback($"No properties has been loaded.");
            return default(T);
        }

        T configInstance = Activator.CreateInstance<T>();

        string[] configBasePropertyNames = Array.Empty<string>();
        try { configBasePropertyNames = typeof(ConfigBase).GetProperties().Select(p => p.Name).ToArray(); }
        catch (Exception) { }

        foreach (var propertyInfo in configPropertiesInfo)
        {
            try
            {
                if (configBasePropertyNames.Contains(propertyInfo.Name))
                    continue; // Skip ConfigBase properties.

                propertyInfo.SetValue(configInstance, loadedProperties[propertyInfo.Name]);
            }
            catch (KeyNotFoundException)
            {
                _errorMessageCallback($"Failed to set value of '{propertyInfo.Name}': Property with that name was not found.");
            }
            catch (Exception ex)
            {
                _errorMessageCallback($"Failed to set value of '{propertyInfo.Name}': {ex.Message}");
            }
        }

        return configInstance;
    }

    private Dictionary<string, object>? DeserializeProperties(string jsonInput, PropertyInfo[] configProperties)
    {
        JsonDocument document;

        try { document = JsonDocument.Parse(jsonInput); }
        catch (Exception ex)
        {
            _errorMessageCallback("Failed to parse input string to JsonDocument: " + ex.Message);
            return null;
        }

        JsonElement.ObjectEnumerator jsonEnumerator;

        try { jsonEnumerator = document.RootElement.EnumerateObject(); }
        catch (Exception ex)
        {
            _errorMessageCallback("Failed to enumerate JsonDocument properties: " + ex.Message);
            return null;
        }

        Dictionary<string, object> loadedProperties = new();

        foreach (var jsonElement in jsonEnumerator)
        {
            try
            {
                Type propertyType = configProperties.First(p => p.Name.Equals(jsonElement.Name)).PropertyType;
                object? deserializedProperty = JsonSerializer.Deserialize(jsonElement.Value.GetRawText(), propertyType);

                if (deserializedProperty is not null)
                    loadedProperties.Add(jsonElement.Name, deserializedProperty);
                else
                    _errorMessageCallback($"Failed to deserialize property '{jsonElement.Name}'.");
            }
            catch (Exception ex)
            {
                _errorMessageCallback($"Failed to load property '{jsonElement.Name}': {ex.Message}");
            }
        }

        return loadedProperties;
    }
}
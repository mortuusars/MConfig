using MConfig.Deserialization;
using MConfig.Serialization;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MConfig;

/// <summary>
/// Base class for config.
/// </summary>
public abstract class ConfigBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Indicates whether config should save on every property changed.
    /// </summary>
    [JsonIgnore]
    public bool SaveOnPropertyChanged { get; set; }

    /// <summary>
    /// Serializer that will be used when <see cref="Serialize"/> function is called.
    /// </summary>
    [JsonIgnore]
    public IConfigSerializer? Serializer { get; set; }

    internal Dictionary<string, object> Properties { get; }

    public ConfigBase()
    {
        Properties = new Dictionary<string, object>();
    }

    /// <summary>
    /// Deserializes input string using the provided deserializer to specified config type.
    /// </summary>
    /// <typeparam name="T">Type (inherited from <see cref="ConfigBase"/>) to deserialize to.</typeparam>
    /// <param name="configDeserializer">Deserializer that will be used to deserialize input.</param>
    /// <param name="serializedInput">Serialized string.</param>
    /// <returns>Deserialized instance.</returns>
    public static T? Deserialize<T>(IConfigDeserializer configDeserializer, string serializedInput) where T : ConfigBase
    {
        return configDeserializer.Deserialize<T>(serializedInput);
    }

    /// <summary>
    /// This method is called when <see cref="SaveOnPropertyChanged"/> is <see langword="true"/> and config property is changed.
    /// </summary>
    /// <param name="serializedConfig">Result of the serialization. Can be <see langword="null"/>.</param>
    public abstract void Save(string? serializedConfig);

    /// <summary>
    /// Serialize (using <see cref="Serializer"/>) and save config.
    /// </summary>
    public virtual void Save()
    {
        string? serializedConfig = Serialize();
        if (serializedConfig is not null)
            Save(serializedConfig);
    }

    /// <summary>
    /// Serializes this config instance using <see cref="Serializer"/>.
    /// </summary>
    /// <returns></returns>
    public string? Serialize() => Serializer?.Serialize(this);

    /// <summary>
    /// Registers config property.
    /// </summary>
    /// <typeparam name="T">Type of property.</typeparam>
    /// <param name="name">Name (key) of the property.</param>
    /// <param name="defaultValue">Value that a property will have after creation.</param>
    /// <param name="isValid">Function to validate when setting a new value on the property. Called on every SetValue.</param>
    /// <returns>Created property.</returns>
    /// <exception cref="InvalidOperationException">When property with same name is already registered.</exception>
    protected ConfigProperty<T> RegisterProperty<T>(string name, T defaultValue, Func<T, bool> isValid)
    {
        if (Properties.ContainsKey(name))
            throw new InvalidOperationException($"Property with name '{name}' is already registered.");

        return new ConfigProperty<T>(name, defaultValue, isValid, this);
    }

    /// <summary>
    /// Registers config property.
    /// </summary>
    /// <typeparam name="T">Type of property.</typeparam>
    /// <param name="name">Name (key) of the property.</param>
    /// <param name="defaultValue">Value that a property will have after creation.</param>
    /// <param name="isValid">Function to validate when setting a new value on the property. Called on every SetValue.</param>
    /// <returns>Created property.</returns>
    /// <exception cref="InvalidOperationException">When property with same name is already registered.</exception>
    protected ConfigProperty<T> RegisterProperty<T>(string name, T defaultValue)
    {
        return RegisterProperty(name, defaultValue, (p) => true);
    }

    /// <summary>
    /// Raises PropertyChanged event for specified property. Saves config
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configProperty"></param>
    internal void InvokePropertyChanged<T>(ConfigProperty<T> configProperty)
    {
        var args = new PropertyChangedEventArgs(configProperty.Name);
        PropertyChanged?.Invoke(this, args);

        if (SaveOnPropertyChanged)
            Save(Serialize());
    }
}
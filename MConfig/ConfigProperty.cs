namespace MConfig;

/// <summary>
/// Represents a config property
/// </summary>
/// <typeparam name="T"></typeparam>
public class ConfigProperty<T> : IEquatable<ConfigProperty<T>?>, IDisposable
{
    /// <summary>
    /// Name (key) of the property.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Value of the property.
    /// </summary>
    public T Value { get; private set; }

    private readonly Func<T, bool> _isValid;
    private readonly ConfigBase _configBase;

    /// <summary>
    /// Creates instance of config property. And sets required fields an properties.
    /// </summary>
    /// <param name="name">Name (key) of th property.</param>
    /// <param name="defaultValue">Initial (default) value.</param>
    /// <param name="isValid">Function that will be will be used to verify that new value (set through<br>
    /// </br><see cref="SetValue(T)"/> is valid for this property.(not null, for example).</param>
    /// <param name="configBase">Instance of config class for proper registering.</param>
    public ConfigProperty(string name, T defaultValue, Func<T, bool> isValid, ConfigBase configBase)
    {
        Name = name;
        Value = defaultValue;
        _isValid = isValid;

        _configBase = configBase;

        _configBase.Properties.Add(name, this);
    }

    /// <summary>
    /// Sets the value of a property to a new value if new value is valid and not same as the old one. 
    /// </summary>
    /// <param name="newValue">New value for the property.</param>
    /// <returns><see langword="true"/> if set successfully.</returns>
    public bool SetValue(T newValue)
    {
        if (!Value!.Equals(newValue) && _isValid(newValue))
        {
            Value = newValue;
            _configBase.InvokePropertyChanged(this);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns string representation of a property.
    /// </summary>
    public override string ToString()
    {
        return $"{Name} = {Value}";
    }

    /// <summary>
    /// Checks if two property objects are equal.<br></br>Config properties are considered equal if they have equal Name and Value.
    /// </summary>
    /// <param name="obj">Other object to compare.</param>
    /// <returns>True if equal.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ConfigProperty<T>);
    }

    /// <summary>
    /// Checks if two properties are equal.<br></br>Config properties are considered equal if they have equal Name and Value.
    /// </summary>
    /// <param name="other">Other property to compare.</param>
    /// <returns>True if equal.</returns>
    public bool Equals(ConfigProperty<T>? other)
    {
        return other != null &&
               Name == other.Name &&
               EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    /// <summary>
    /// Returns HashCode of the property.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }

    /// <summary>
    /// Unregisters property from Config.
    /// </summary>
    public void Dispose()
    {
        _configBase.Properties.Remove(Name);
        GC.SuppressFinalize(this);
    }
}
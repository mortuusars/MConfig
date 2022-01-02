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

    public override string ToString()
    {
        return $"{Name} = {Value}";
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ConfigProperty<T>);
    }

    public bool Equals(ConfigProperty<T>? other)
    {
        return other != null &&
               Name == other.Name &&
               EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }

    public void Dispose()
    {
        _configBase.Properties.Remove(Name);
        GC.SuppressFinalize(this);
    }
}
Simple library for implementing runtime configuration.

- Implements INotifyPropertyChanged
- Allows serialization and deserialization.
- Validation of the new values.

---

- Provides Json serializer and two Json deserializers. 
- Custom serialization can be used by implementing IConfigSerializer and IConfigDeserializer.

---
Example of config class

```csharp
    public class Config : ConfigBase //Inherit from ConfigBase
    {
        public int MagicNumber { get => _magicNumber.Value; set => _magicNumber.SetValue(value); }
        private readonly ConfigProperty<int> _magicNumber;
        
        public Config()
        {
            //Registering property example:
            _magicNumber = RegisterProperty((nameof(MagicNumber), 42);
        }
        
        // Implement this abstract method:
        public override void Save(string? serializedConfig)
        {
            //Leave empty if not needed.
        }
    }
```

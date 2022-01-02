using System;
using System.Text.Json;
using Xunit;

namespace Mortuus.Config.Tests;

public class ConfigBaseTests : ConfigBase
{
    public override void Save(string? serializedConfig) { }

    [Fact]
    public void RegisterPropertyShouldReturnProperProperty()
    {
        var property = RegisterProperty<int>("Prop", 42);

        Assert.Equal("Prop", property.Name);
        Assert.Equal(42, property.Value);
    }

    [Fact]
    public void RegisterPropertyThrowIfRegisteredTwice()
    {
        var property = RegisterProperty<int>("Prop", 42);
        var property2 = 

        Assert.Throws<InvalidOperationException>(() => RegisterProperty<int>("Prop", 42));
    }

    [Fact]
    public void ChangedPropertyShouldRaizePropertyChanged()
    {
        var property = RegisterProperty<int>("Prop", 42);

        bool isRaised = false;
        PropertyChanged += (s, e) => { if ("Prop".Equals(e.PropertyName)) isRaised = true; };
        property.SetValue(1354);

        Assert.True(isRaised);
    }

    [Fact]
    public void ChangedPropertyShouldNOTRaizePropertyChangedIfNewValueIsSame()
    {
        var property = RegisterProperty<int>("Prop", 42);

        bool isRaised = false;
        PropertyChanged += (s, e) => { if ("Prop".Equals(e.PropertyName)) isRaised = true; };
        property.SetValue(42);

        Assert.False(isRaised);
    }
}
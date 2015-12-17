# Oxygenize
This is a new 2.0 branch with completely rewritten syntax. The plan is to make it easier to write and read + allow defining how particular properties should vary.

Populating your classes and structures with totally randomized(or not at all!) data.

Oxygenize is a small library, that helps you populate data to your classes and structures. Its main purpose is to generate multiple test cases, so you don't have to bother thinking about them. It supports three strategies of data generation:
* `GenerationStrategy.Random`
* `GenerationStrategy.Custom`
* `GenerationStrategy.Mixed`

## Installation
Oxygenize can be installed using NuGet Packages Manager:

```Install-Package Oxygenize```

## How it works?
Oxygenize takes type you want to be generated as a parameter and - depending on the selected generation strategy - either tries to generate totally random or fully customized data.
It returns strongly typed object so no additional casts are required.

## Basic usage
The very basic usage takes nothing more than configuring your type with selected strategy:

```
Oxygenize.Configure<YourTypeToBegenerated>(configurator =>
{
    configurator.WithStrategy(GenerationStrategy.Random);
});
```
Then you can fetch it using:

```var instance = Oxygenize.For<YourTypeToBegenerated>();```

Note that each time you call ```Oxygenize.For<T>()``` method, you will fetch different instance.

## Supported types
Currently Oxygenize supports following types natively:
* primitives(`int`, `decimal` etc.)
* nullable primitives(`int?`, `decimal?` etc.)
* arrays of primitives/nullable primitives
* value types(`DateTime`, `Guid`, `TimeSpan`)
* decimals/decimal arrays
* enums
* generic collections `IEnumerable<T>`, `ICollection<T>`, `IList<T>` and `IDictionary<TKey, TValue>`
* strings

Note: Generics parameter types are limited to the types natively supported by Oxygenize. If the parameter type is your custom type(or is not supported) you has to explicitely register it as shown below.

## Custom types support
By default Oxygenize supports only few structs which can be generated using `GenerationStrategy.Random` e.g. `DateTime` or `Guid`. You can add support for all desired types using `Configurator.Concrete<TType>(Func<object> value)` method:

```
Oxygenize.Configure<YourTypeToBegenerated>(configurator =>
{
    configurator.WithStrategy(GenerationStrategy.Random);
    configurator.Concrete<CustomStruct>(() => new CustomStruct { Id = 1 });
});
```

Note that the previous version of ```Oxygenize``` stored all supported types in an internal static dictionary, thus it was possible to reuse them. Now implementation has changed so you have to either configure type in one place or reconfigure it each time it is used.

## Mask support
Currently unavailable

## Example usage
```
Oxygenize.Configure<MyType>(configurator =>
{
    configurator.WithStrategy(GenerationStrategy.Mixed);
    configurator.WithMaximumCapacity(100);
    configurator.WithMaxStringLength(100);
    configurator.WithMinStringLength(50);
    configurator.WithNullableReferenceTypes(true);
    configurator.WithValues((_) =>
    {
        _.Name = "NameShouldBeFixed;
        _.Surname = "SurnameShouldBeFixedAlso";
        
        return _;
    });
});

var instance = Oxygenize.For<StringArray>();
```
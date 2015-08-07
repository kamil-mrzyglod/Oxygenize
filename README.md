# Oxygenize
Populating your POCOs with totally randomized(or not at all!) data.

Oxygenize is a small library, that helps you populate data to your POCOs. It supports three strategies of data generation:
* `GenerationStrategy.Random`
* `GenerationStrategy.Custom`
* `GenerationStrategy.Mixed` (in development)

## Installation
Oxygenize can be installed using NuGet Packages Manager:

```Install-Package Oxygenize```

## How it works?
Oxygenize takes type you want to be generated as a parameter and - depending on the selected generation strategy - either tries to generate totally random or fully customized data.
It returns strongly typed object so no additional casts are required.

## Basic usage
The very basic usage takes nothing more than:

```var instance = Oxygenize.For<YourTypeToBegenerated>().Instance;```

You can explicitely select generation strategy using `WithStrategy(GenerationStrategy strategy)` method:

```
var instance = Oxygenize.For<YourTypeToBegenerated>()
                        .WithStrategy(GenerationStrategy.Random)
                        .Instance;
```
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
By default Oxygenize supports only few structs which can be generated using `GenerationStrategy.Random` e.g. `DateTime` or `Guid`. You can add support for all desired types using `Oxygenize.AddSupport(string typeName, Func<object> valueToObtain)` method:

```
Oxygenize.AddSupport("Oxygenize.Test.CustomStruct", () => new CustomStruct
{
    Id = 1
});
```

this method takes two arguments:
* `string typeName` which is nothing more than string representation of an instance of the given type - implementation of the `ToString()` method
* `Func<object> valueToObtain` delegate which will be invoked when generating random type instance value

All supported types are stored using internal `ConcurrentDictionary` static field so once registered, they stay there until `AppDomain` is unloaded.

## Example usage
```
[Test]
public void Should_Generate_An_Instance_With_Reference_Types()
{
    Oxygenize.AddSupport(typeof(PrimitiveTypes).ToString(), () => Oxygenize.For<PrimitiveTypes>().Instance);
    Oxygenize.AddSupport(typeof(Collections).ToString(), () => Oxygenize.For<Collections>().Instance);

    var instance = Oxygenize.For<InstanceTypes>()
                            .WithStrategy(GenerationStrategy.Random)
                            .UpperBound(500)
                            .NullableReferenceTypes(true)
                            .Instance;

    ...
}

class InstanceTypes
{
    public PrimitiveTypes PrimitiveTypes { get; set; }

    public Collections Collections { get; set; }
}
```

## API reference
All available API methods are listed below:

### Oxygenize
* `void AddSupport(string typeName, Func<object> valueToObtain)` - adds support for given custom type. Type name is a Type.ToString() representation.
* `Oxygenize<T> For<T>` - returns `Oxygenize<T>` object, which can be customized to populate necessary data for given parameter type. Parameter is constrained with `new()`

### Oxygenize\<T\>
* `PropertyConfigurator<T> Configure()` - returns a PropertyConfigurator, which can be used to configure properties using CustomGenerationStrategy
* `T Instance` - returns an instance of constructed type. Used at the end of the methods chain.
* `Oxygenize<T> MaxStringLength(int maxLength)` - sets maximum length for all generated strings
* `Oxygenize<T> MinStringLength(int maxLength)` - sets minimum length for all generated strings
* `Oxygenize<T> NullableReferenceTypes(bool areNullable)` - tells the generator whether reference types can be generated as `null`
* `Oxygenize<T> UpperBound(int upperBound)` - sets upper bound of array elements count.
* `Oxygenize<T> WithConstructor(Type[] types, object[] values)` - specifies constructor which should be used for an instance generation
* `Oxygenize<T> WithStrategy(GenerationStrategy strategy = GenerationStrategy.Random)` - sets a strategy used for an instance generation

### PropertyConfigurator<T>
* `PropertyConfigurator<T> Set<TProp>(Expression<Func<T, TProp>> expression, TProp value)` - sets a given property value
* `Oxygenize<T> Compile()` - finishes configuration
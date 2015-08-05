# Oxygenize
Populating your POCOs with totally randomized(or not at all!) data.

Oxygenize is a small library, that helps you populate data to your POCOs. It supports three strategies of data generation:
* `GenerationStrategy.Random`
* `GenerationStrategy.Custom` (in development)
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
var instance = Oxygenize.For<YourTypeToBegenerated>().WithStrategy(GenerationStrategy.Random).Instance;```

## Custom types support
By default Oxygenize supports only few structs which can be generated using `GenerationStrategy.Random` e.g. `DateTime` or `Guid`. You can add support for all desired types using `Oxygenize.AddSupport(string typeName, Func<object> valueToObtain)` method:

```
Oxygenize.AddSupport("Oxygenize.Test.CustomStruct", () => new CustomStruct
            {
                Id = 1
            });```

this method take two arguments:
* `string typeName` which is nothing more than string representation of an instance of the given type - implementation of the `ToString()` method
* `Func<object> valueToObtain` delegate which will be invoked when generating random type instance value

All supported types are stored using internal `ConcurrentDictionary` static field so once registered, they stay there until `AppDomain` is unloaded.
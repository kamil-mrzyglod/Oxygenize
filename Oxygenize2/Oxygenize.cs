using System;
using System.Collections.Concurrent;
using Oxygenize2.Generators;

namespace Oxygenize2
{
    public class Oxygenize
    {
        private static readonly ConcurrentDictionary<Type, Delegate> Configurations = new ConcurrentDictionary<Type, Delegate>();

        public static void Configure<T>(Action<Configurator<T>> configuration) where T : new()
        {
            if(Configurations.TryAdd(typeof(T), configuration) == false)
                throw new ArgumentException();
        }

        public static T For<T>() where T : new()
        {
            Delegate configuration;
            if(Configurations.TryGetValue(typeof(T), out configuration) == false)
                throw new ArgumentException();

            var configurator = new Configurator<T>();
            ((Action<Configurator<T>>)configuration)(configurator);

            var instance = PopulateData<T>(configurator.Configuration);
            return instance;
        }

        private static T PopulateData<T>(Configuration configuration) where T : new()
        {
            T instance;
            switch (configuration.Strategy)
            {
                case GenerationStrategy.Mixed:
                case GenerationStrategy.Custom:
                case GenerationStrategy.Random:
                    instance = new RandomStrategyGenerator<T>(configuration).GetData();
                    break;
                default:
                    instance = new RandomStrategyGenerator<T>(configuration).GetData();
                    break;
            }

            return instance;
        }

        public class Configurator<T> where T : new()
        {
            internal readonly Configuration Configuration;

            internal Configurator()
            {
                Configuration = new Configuration(typeof(T));
            }

            public void WithStrategy(GenerationStrategy strategy)
            {
                Configuration.Strategy = strategy;
            }

            public void WithMaximumCapacity(int capacity)
            {
                Configuration.MaxCapacity = capacity;
            }

            public void WithNullableReferenceTypes(bool nullableReferenceTypes)
            {
                Configuration.NullableReferenceTypes = nullableReferenceTypes;
            }

            public void WithMaxStringLength(int length)
            {
                Configuration.MaximumStringLength = length;
            }

            public void WithMinStringLength(int length)
            {
                Configuration.MinStringLength = length;
            }

            public void WithCtorParameters(Tuple<Type[], object[]> @params)
            {
                Configuration.ConstructorParameters = @params;
            }
        }

        internal class Configuration
        {
            private readonly Type _type;

            public GenerationStrategy Strategy;
            public int MaxCapacity;
            public bool NullableReferenceTypes;
            public int MaximumStringLength;
            public int MinStringLength;
            public Tuple<Type[], object[]> ConstructorParameters;

            internal Configuration(Type type)
            {
                _type = type;
            }
        }
    }

    public enum GenerationStrategy
    {
        Random,
        Custom,
        Mixed
    }
}
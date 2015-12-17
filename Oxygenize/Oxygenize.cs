namespace Oxygenize
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Generators;
    using System.Linq.Expressions;

    public class Oxygenize
    {
        private static readonly ConcurrentDictionary<Type, Delegate> Configurations = new ConcurrentDictionary<Type, Delegate>();

        /// <summary>
        /// Pass configuration which should be used when populating data
        /// </summary>
        public static void Configure<T>(Action<Configurator<T>> configuration) where T : new()
        {
            Configurations.AddOrUpdate(typeof(T), configuration, (type, @delegate) => configuration);
        }
        
        /// <summary>
        /// Get an instance with populated data
        /// </summary>
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
                    instance = new MixedStrategyGenerator<T>(configuration).GetData();
                    break;
                case GenerationStrategy.Custom:
                    instance = new CustomStrategyGenerator<T>(configuration).GetData();
                    break;
                case GenerationStrategy.Random:
                    instance = new RandomStrategyGenerator<T>(configuration).GetData();
                    break;
                default:
                    instance = new RandomStrategyGenerator<T>(configuration).GetData();
                    break;
            }

            return instance;
        }   
    }

    internal class Configuration
    {
        private readonly Type _type;

        internal readonly IDictionary<string, Func<object>> Concretes = new Dictionary<string, Func<object>>();

        public GenerationStrategy Strategy;
        public int MaxCapacity = 1000;
        public bool NullableReferenceTypes;
        public int MaximumStringLength = 1000;
        public int MinStringLength;
        public Tuple<Type[], object[]> ConstructorParameters;
        public Expression Value;
        public Delegate ValueGetter; 

        internal Configuration(Type type)
        {
            _type = type;
        }
    }

    public enum GenerationStrategy
    {
        Random,
        Custom,
        Mixed
    }
}
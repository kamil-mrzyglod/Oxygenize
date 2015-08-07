using System;
using System.Collections.Concurrent;
using Oxygenize.Generators;

namespace Oxygenize
{
    public class Oxygenize
    {
        private static readonly ConcurrentDictionary<string, Func<object>> SupportedTypes = new ConcurrentDictionary<string, Func<object>>(); 

        public static Oxygenize<T> For<T>() where T : new()
        {
            return new Oxygenize<T>();
        }

        /// <summary>
        /// Adds support for given custom type, which enables
        /// RandomStrategyGenerator to generate given value
        /// </summary>
        /// <param name="typeName">Type ToString() representation</param>
        /// <param name="valueToObtain">Function which will be used to obtain random value</param>
        public static void AddSupport(string typeName, Func<object> valueToObtain)
        {
            SupportedTypes.AddOrUpdate(typeName, valueToObtain, (s, func) => valueToObtain);
        }

        internal static object ObtainValue(string typeName)
        {
            Func<object> valueToObtain;
            if (!SupportedTypes.TryGetValue(typeName, out valueToObtain))
            {
                throw new ArgumentException("Cannot obtain a value of unregistered type.", "typeName");
            }

            return valueToObtain.Invoke();
        }
    }

    public class Oxygenize<T> where T : new()
    {
        private GenerationStrategy _strategy = GenerationStrategy.Random;
        private int _arrayUpperBound = 1000;
        private bool _nullableReferenceTypes;
        private int _maxStringLength = 4000;
        private int _minStringLength;
        private Tuple<Type[], object[]> _constructorParameters;

        /// <summary>
        /// Returns an instance of the given type
        /// </summary>
        public T Instance
        {
            get { return PopulateData(); }
        }

        private T PopulateData()
        {
            var configuration = new Configuration(_arrayUpperBound, _nullableReferenceTypes, _maxStringLength, _minStringLength, _constructorParameters);

            T instance;
            switch (_strategy)
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

        /// <summary>
        /// Sets the strategy used for data generation
        /// </summary>
        public Oxygenize<T> WithStrategy(GenerationStrategy strategy = GenerationStrategy.Random)
        {
            _strategy = strategy;
            return this;
        }

        /// <summary>
        /// Sets upper bound for arrays generation
        /// </summary>
        public Oxygenize<T> UpperBound(int upperBound)
        {
            _arrayUpperBound = upperBound;
            return this;
        }

        /// <summary>
        /// Determines whether reference types can be generated as null
        /// </summary>
        public Oxygenize<T> NullableReferenceTypes(bool areNullable)
        {
            _nullableReferenceTypes = areNullable;
            return this;
        }

        /// <summary>
        /// Sets maximum length of a generated string
        /// </summary>
        public Oxygenize<T> MaxStringLength(int maxLength)
        {
            _maxStringLength = maxLength;
            return this;
        }

        /// <summary>
        /// Sets minimal length of a generated string
        /// </summary>
        public Oxygenize<T> MinStringLength(int minLength)
        {
            _minStringLength = minLength;
            return this;
        }

        /// <summary>
        /// Sets a constructor which should be called when creating an instance
        /// </summary>
        /// <param name="types">Parameters types</param>
        /// <param name="values">Parameters values</param>
        public Oxygenize<T> WithConstructor(Type[] types, object[] values)
        {
            _constructorParameters = new Tuple<Type[], object[]>(types, values);
            return this;
        } 
    }

    public enum GenerationStrategy
    {
        Random,
        Custom,
        Mixed
    }
}
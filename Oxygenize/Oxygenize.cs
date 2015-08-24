using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
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
        private readonly PropertyConfigurator<T> _configurator; 

        private GenerationStrategy _strategy = GenerationStrategy.Random;
        private int _arrayUpperBound = 1000;
        private bool _nullableReferenceTypes;
        private int _maxStringLength = 4000;
        private int _minStringLength;
        private Tuple<Type[], object[]> _constructorParameters;

        internal Oxygenize()
        {
            _configurator = new PropertyConfigurator<T>(this);
        }   

        /// <summary>
        /// Returns an instance of the given type
        /// </summary>
        public T Instance
        {
            get { return PopulateData(); }
        }

        private T PopulateData()
        {
            var configuration = new Configuration(_arrayUpperBound, _nullableReferenceTypes, _maxStringLength, _minStringLength, _constructorParameters, _configurator.PropertyConfigurations);

            T instance;
            switch (_strategy)
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

        /// <summary>
        /// Returns a PropertyConfigurator, which can be used to configure
        /// properties using CustomGenerationStrategy
        /// </summary>
        public PropertyConfigurator<T> Configure()
        {
            if(_strategy == GenerationStrategy.Random)
                throw new InvalidOperationException("You cannot configure an instance for RandomGenerationStrategy.");

            return _configurator;
        }  
    }

    public class PropertyConfigurator<T> where T : new()
    {
        private readonly Oxygenize<T> _oxygenize;
        internal readonly ConcurrentDictionary<string, PropertyConfiguration> PropertyConfigurations = new ConcurrentDictionary<string, PropertyConfiguration>(); 

        internal PropertyConfigurator(Oxygenize<T> oxygenize)
        {
            _oxygenize = oxygenize;
        }

        /// <summary>
        /// Sets the specific property configurator
        /// </summary>
        public SpecificPropertyConfigurator<T, TProp> Prop<TProp>(Expression<Func<T, TProp>> expression)
        {
            MemberExpression me;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    me = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expression.Body as MemberExpression;
                    break;

            }

            return new SpecificPropertyConfigurator<T, TProp>(this, me);
        }

        /// <summary>
        /// Finishes configuration
        /// </summary>
        public Oxygenize<T> Compile()
        {
            return _oxygenize;;
        } 
    }

    internal class PropertyConfiguration
    {
        public readonly object Value;

        public readonly string Mask;

        public PropertyConfiguration(object value, string mask)
        {
            Value = value;
            Mask = mask;
        }
    }

    public class SpecificPropertyConfigurator<T, TProp> where T : new()
    {
        private readonly PropertyConfigurator<T> _propertyConfigurator;
        private readonly MemberExpression _expression;

        private object _value;
        private string _mask;

        internal SpecificPropertyConfigurator(PropertyConfigurator<T> propertyConfigurator, MemberExpression expression)
        {
            _propertyConfigurator = propertyConfigurator;
            _expression = expression;
        }

        /// <summary>
        /// Sets a property value
        /// </summary>
        public SpecificPropertyConfigurator<T, TProp> WithValue(TProp value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        /// Sets a property mask
        /// </summary>
        public SpecificPropertyConfigurator<T, TProp> Mask(string mask)
        {
            if (typeof (TProp) != typeof(string))
            {
                throw new InvalidOperationException("Cannot set mask for the type other than string.");
            }

            _mask = mask;
            return this;
        }

        /// <summary>
        /// Saves the property configuration and allows for further method chain
        /// </summary>
        public PropertyConfigurator<T> Set()
        {
            _propertyConfigurator.PropertyConfigurations.AddOrUpdate(_expression.Member.Name, s => new PropertyConfiguration(_value, _mask), (info, val) => val);
            return _propertyConfigurator;
        } 
    }

    public enum GenerationStrategy
    {
        Random,
        Custom,
        Mixed
    }
}
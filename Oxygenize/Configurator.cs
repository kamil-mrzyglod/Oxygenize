namespace Oxygenize
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class Configurator<T> where T : new()
    {
        internal readonly Configuration Configuration;

        internal Configurator()
        {
            Configuration = new Configuration(typeof(T));
        }

        /// <summary>
        /// Sets strategy which should be used when populating data
        /// </summary>
        public void WithStrategy(GenerationStrategy strategy)
        {
            Configuration.Strategy = strategy;
        }

        /// <summary>
        /// Defines maximum collection(list, array) capacity
        /// </summary>
        public void WithMaximumCapacity(int capacity)
        {
            Configuration.MaxCapacity = capacity;
        }

        /// <summary>
        /// Defines whether any reference type can be generated as null
        /// </summary>
        public void WithNullableReferenceTypes(bool nullableReferenceTypes)
        {
            Configuration.NullableReferenceTypes = nullableReferenceTypes;
        }

        /// <summary>
        /// Defines maximum strings' length
        /// </summary>
        /// <param name="length"></param>
        public void WithMaxStringLength(int length)
        {
            Configuration.MaximumStringLength = length;
        }

        /// <summary>
        /// Defines minimum strings' length
        /// </summary>
        /// <param name="length"></param>
        public void WithMinStringLength(int length)
        {
            Configuration.MinStringLength = length;
        }

        /// <summary>
        /// Allows to pass concrete ctor params, what enables
        /// using specific ctor when creating an instance of type
        /// </summary>
        /// <param name="params"></param>
        public void WithCtorParameters(Type[] types, object[] objects)
        {
            Configuration.ConstructorParameters = new Tuple<Type[], object[]>(types, objects);
        }

        /// <summary>
        /// Defines what factory function should be used to obtain
        /// a value for a type, which cannot be generated
        /// using in-built methods
        /// </summary>
        public void Concrete<TType>(Func<object> value)
        {
            Configuration.Concretes.Add(typeof(TType).ToString(), value);
        }

        /// <summary>
        /// Defines an implicitly assumed factory function,
        /// which can be used to get type's instance
        /// </summary>
        public void Concrete<TType>() where TType : new()
        {
            Configuration.Concretes.Add(typeof(TType).ToString(), () => Oxygenize.For<TType>());
        }

        /// <summary>
        /// Enables to manually declare returned value
        /// </summary>
        public void WithValue(Expression<Func<T>> expression)
        {
            if (Configuration.Strategy == GenerationStrategy.Custom)
            {
                Configuration.Value = expression;
                return;
            }
                
            throw new InvalidOperationException("You can declare value only for CustomGenerationStrategy.");
        }

        /// <summary>
        /// Declares specific values which should be used
        /// instead of generated ones
        /// </summary>
        public void WithValues(Func<T, T> func)
        {
            Configuration.ValueGetter = func;
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
}
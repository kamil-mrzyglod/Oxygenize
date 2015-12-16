namespace Oxygenize2
{
    using System;

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
        /// <param name="@params"></param>
        public void WithCtorParameters(Tuple<Type[], object[]> @params)
        {
            Configuration.ConstructorParameters = @params;
        }

        /// <summary>
        /// Defines what function should be used to obtain
        /// a value for a type, which cannot be generated
        /// using in-built methods
        /// </summary>
        public void Concrete<TType>(Func<object> value)
        {
            Configuration.Concretes.Add(typeof(TType).ToString(), value);
        }
    }
}
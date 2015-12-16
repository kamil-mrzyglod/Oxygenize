using System;
using System.Reflection;

namespace Oxygenize2.Generators
{
    abstract class GeneratorBase<T> where T : new()
    {
        protected const string Chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";

        protected Oxygenize.Configuration Configuration;

        protected Type Type;
        protected T Instance;

        protected GeneratorBase() { }

        protected GeneratorBase(Oxygenize.Configuration configuration)
        {
            Configuration = configuration;
        }

        public T GetData()
        {
            Type = typeof(T);

            if (Configuration.ConstructorParameters != null)
            {
                var type = typeof(T);
                var constructor = type.GetConstructor(Configuration.ConstructorParameters.Item1);
                if (constructor == null)
                {
                    throw new ArgumentException("Constructor with given types is not found.");
                }

                Instance = (T)constructor.Invoke(Configuration.ConstructorParameters.Item2);
            }
            else
            {
                Instance = new T();
            }

            return Generate();
        }

        protected abstract T Generate();
        protected abstract void SetProperties();

        protected void SetProperty(PropertyInfo property)
        {
            property.SetValue(Instance, RandomStrategyGenerator<T>.GetRandomPropertyValue(property.PropertyType), null);
        }
    }
}
using System;

namespace Oxygenize.Generators
{
    abstract class GeneratorBase<T>
    {
        protected Type Type;
        protected T Instance;

        public T GetData()
        {
            Type = typeof (T);
            Instance = Activator.CreateInstance<T>();

            return Generate();
        }

        protected abstract T Generate();
    }
}
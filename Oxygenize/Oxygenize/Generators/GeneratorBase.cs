using System;

namespace Oxygenize.Generators
{
    abstract class GeneratorBase<T> where T : new()
    {
        protected Type Type;
        protected T Instance;

        public T GetData()
        {
            Type = typeof (T);
            Instance = new T();

            return Generate();
        }

        protected abstract T Generate();
        protected abstract void SetProperties();
    }
}
namespace Oxygenize.Generators
{
    abstract class GeneratorBase
    {
        public T For<T>()
        {
            return Generate<T>();
        }

        protected abstract T Generate<T>();
    }
}
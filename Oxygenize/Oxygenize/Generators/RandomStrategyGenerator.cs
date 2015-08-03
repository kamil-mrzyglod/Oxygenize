namespace Oxygenize.Generators
{
    class RandomStrategyGenerator<T> : GeneratorBase<T>
    {
        protected override T Generate()
        {
            return Instance;
        }
    }
}
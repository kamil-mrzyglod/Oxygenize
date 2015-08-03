using System;

namespace Oxygenize.Generators
{
    class RandomStrategyGenerator : GeneratorBase
    {
        protected override T Generate<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
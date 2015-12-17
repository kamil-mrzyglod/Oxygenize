using System;
using System.Linq.Expressions;

namespace Oxygenize.Generators
{
    internal class MixedStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public MixedStrategyGenerator(Configuration configuration) : base(configuration)
        {
        }

        protected override T Generate()
        {
            SetProperties();
            if(Configuration.ValueGetter != null)
            {
                ((Func<T, T>)Configuration.ValueGetter).Invoke(Instance);
            }

            return Instance;
        }

        protected override void SetProperties()
        {
            foreach (var property in Type.GetProperties())
            {
                SetProperty(property);             
            }
        }
    }
}
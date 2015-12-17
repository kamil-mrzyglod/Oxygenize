using System;
using System.Linq.Expressions;

namespace Oxygenize.Generators
{
    internal class CustomStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public CustomStrategyGenerator(Configuration configuration) : base(configuration)
        {
        }

        protected override T Generate()
        {
            // This strategy supports populating data manually
            if(Configuration.Value != null)
            {
                var value = ((Expression<Func<T>>)Configuration.Value).Compile().Invoke();
                return value;
            }

            SetProperties();
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
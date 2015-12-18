namespace Oxygenize.Generators
{
    using System;

    internal class MixedStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public MixedStrategyGenerator(Configuration configuration) : base(configuration)
        {
        }

        protected override T Generate()
        {
            SetProperties();
            return base.Generate();
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
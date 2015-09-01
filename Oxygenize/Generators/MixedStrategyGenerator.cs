namespace Oxygenize.Generators
{
    class MixedStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public MixedStrategyGenerator(Configuration configuration) : base(configuration)
        {
        }

        protected override T Generate()
        {
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
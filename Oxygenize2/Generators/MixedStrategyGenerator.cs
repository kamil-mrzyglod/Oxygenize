namespace Oxygenize2.Generators
{
    class MixedStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public MixedStrategyGenerator(Oxygenize2.Oxygenize.Configuration configuration) : base(configuration)
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
namespace Oxygenize.Generators
{
    internal class CustomStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public CustomStrategyGenerator(Configuration configuration) : base(configuration)
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
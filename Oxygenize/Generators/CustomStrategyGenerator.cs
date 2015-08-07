namespace Oxygenize.Generators
{
    class CustomStrategyGenerator<T> : GeneratorBase<T> where T : new()
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
                object value;
                if (Configuration.ParametersConfiguration.TryGetValue(property.Name, out value))
                {
                    property.SetValue(Instance, value);
                }               
            }
        }
    }
}
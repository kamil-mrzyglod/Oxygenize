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
                PropertyConfiguration configuration;
                if (Configuration.ParametersConfigurations.TryGetValue(property.Name, out configuration))
                {
                    property.SetValue(Instance, configuration.Value);
                }               
            }
        }
    }
}
namespace Oxygenize.Generators
{
    class MixedStrategyGenerator<T> : RandomStrategyGenerator<T> where T : new()
    {
        public MixedStrategyGenerator(Configuration configuration) : base(configuration)
        {
        }

        protected override T Generate()
        {
            base.Generate();

            SetProperties();
            return Instance;
        }

        protected override void SetProperties()
        {
            base.SetProperties();
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
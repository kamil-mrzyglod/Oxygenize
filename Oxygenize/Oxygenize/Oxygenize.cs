using Oxygenize.Generators;

namespace Oxygenize
{
    public class Oxygenize
    {
        public static Oxygenize<T> For<T>()
        {
            return new Oxygenize<T>();
        } 
    }

    public class Oxygenize<T>
    {
        private GenerationStrategy _strategy = GenerationStrategy.Random;

        /// <summary>
        /// Returns an instance of the given type
        /// </summary>
        public T Instance
        {
            get { return PopulateData(); }
        }

        private T PopulateData()
        {
            T instance;
            switch (_strategy)
            {
                case GenerationStrategy.Mixed:
                case GenerationStrategy.Custom:
                case GenerationStrategy.Random:
                    instance = new RandomStrategyGenerator<T>().GetData();
                    break;
                default:
                    instance = new RandomStrategyGenerator<T>().GetData();
                    break;
            }

            return instance;
        }

        /// <summary>
        /// Sets the strategy used for data generation
        /// </summary>
        public Oxygenize<T> WithStrategy(GenerationStrategy strategy = GenerationStrategy.Random)
        {
            _strategy = strategy;
            return this;
        } 
    }

    public enum GenerationStrategy
    {
        Random,
        Custom,
        Mixed
    }
}
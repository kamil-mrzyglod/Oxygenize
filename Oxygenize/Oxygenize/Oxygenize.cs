using System;

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
            get { return Activator.CreateInstance<T>(); }
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
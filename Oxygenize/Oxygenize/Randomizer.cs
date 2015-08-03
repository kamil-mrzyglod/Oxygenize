using System;

namespace Oxygenize
{
    public class Randomizer
    {
        private static Random random;

        public Random Instance
        {
            get { return random ?? (random = new Random()); }
        }

        public static bool ShouldEnter()
        {
            return new Randomizer().Instance.NextDouble() < 0.5;
        }
    }
}
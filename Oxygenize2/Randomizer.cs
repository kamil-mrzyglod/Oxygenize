using System;

namespace Oxygenize
{
    internal class Randomizer
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

    /// <summary>
    /// Taken from http://stackoverflow.com/questions/609501/generating-a-random-decimal-in-c-sharp Jon Skeet's answer
    /// </summary>
    internal static class RandomizerExtensions
    {
        public static int NextInt32(this Random rng)
        {
            unchecked
            {
                var firstBits = rng.Next(0, 1 << 4) << 28;
                var lastBits = rng.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }

        public static decimal NextDecimal(this Random rng)
        {
            var scale = (byte)rng.Next(29);
            var sign = rng.Next(2) == 1;
            return new decimal(rng.NextInt32(),
                               rng.NextInt32(),
                               rng.NextInt32(),
                               sign,
                               scale);
        }
    }
}
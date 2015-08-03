using System;

namespace Oxygenize
{
    public class Randomizer
    {
        private static Random _random;

        public Random Instance
        {
            get { return _random ?? (_random = new Random()); }
        }
    }
}
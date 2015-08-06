namespace Oxygenize
{
    class Configuration
    {
        public readonly int ArrayUpperBound;
        public readonly bool NullableReferenceTypes;
        public readonly int MaximumStringLength;
        public readonly int MinStringLength;

        public Configuration(int arrayUpperBound, bool nullableReferenceTypes, int maximumStringLength, int minStringLength)
        {
            ArrayUpperBound = arrayUpperBound;
            NullableReferenceTypes = nullableReferenceTypes;
            MaximumStringLength = maximumStringLength;
            MinStringLength = minStringLength;
        }
    }
}
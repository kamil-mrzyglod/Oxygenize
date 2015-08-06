namespace Oxygenize
{
    class Configuration
    {
        public readonly int ArrayUpperBound;
        public readonly bool NullableReferenceTypes;
        public readonly int MaximumStringLength;

        public Configuration(int arrayUpperBound, bool nullableReferenceTypes, int maximumStringLength)
        {
            ArrayUpperBound = arrayUpperBound;
            NullableReferenceTypes = nullableReferenceTypes;
            MaximumStringLength = maximumStringLength;
        }
    }
}
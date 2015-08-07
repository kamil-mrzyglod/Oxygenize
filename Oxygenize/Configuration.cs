using System;
using System.Collections.Concurrent;

namespace Oxygenize
{
    class Configuration
    {
        public readonly int ArrayUpperBound;
        public readonly bool NullableReferenceTypes;
        public readonly int MaximumStringLength;
        public readonly int MinStringLength;
        public readonly Tuple<Type[], object[]> ConstructorParameters;
        public readonly ConcurrentDictionary<string, object> ParametersConfiguration;

        public Configuration(int arrayUpperBound, bool nullableReferenceTypes, int maximumStringLength, int minStringLength, Tuple<Type[], object[]> constructorParameters, ConcurrentDictionary<string, object> parametersConfiguration)
        {
            ArrayUpperBound = arrayUpperBound;
            NullableReferenceTypes = nullableReferenceTypes;
            MaximumStringLength = maximumStringLength;
            MinStringLength = minStringLength;
            ConstructorParameters = constructorParameters;
            ParametersConfiguration = parametersConfiguration;
        }
    }
}
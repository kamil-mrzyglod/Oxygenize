using System.Collections.Generic;

namespace Oxygenize.Test.TestClasses
{
    public class Collections
    {
        public IEnumerable<int> Ints { get; set; }

        public IEnumerable<int?> NullableInts { get; set; }

        public IEnumerable<decimal> Decimals { get; set; }

        public ICollection<int> CollectionInts { get; set; } 

        public ICollection<int?> CollectionNullableInts { get; set; }

        public IList<int> ListInts { get; set; } 

        public IList<int?> ListNullableInts { get; set; }

        public IDictionary<int, decimal> Dictionary { get; set; } 
    }
}
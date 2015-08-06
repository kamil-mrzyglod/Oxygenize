using System.Collections.Generic;

namespace Oxygenize.Test.TestClasses
{
    public class StringsClass
    {
        public string String { get; set; }

        public string[] Strings { get; set; }

        public IEnumerable<string> EnumerableStrings { get; set; }

        public ICollection<string> CollectionStrings { get; set; }

        public IList<string> ListStrings { get; set; }

        public IDictionary<string, string> DictionaryStrings { get; set; }
    }
}
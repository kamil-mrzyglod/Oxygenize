namespace Oxygenize.Test.TestClasses
{
    public class ConstructorsTest
    {
        public readonly int Int;
        public readonly string String;

        public ConstructorsTest()
        {
            Int = 1;
            String = "TEST";
        }

        public ConstructorsTest(int @int, string @string)
        {
            Int = @int;
            String = @string;
        }
    }
}
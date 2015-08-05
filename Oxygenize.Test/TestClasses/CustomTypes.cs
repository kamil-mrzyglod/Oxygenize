namespace Oxygenize.Test.TestClasses
{
    public class CustomTypes
    {
        public CustomStruct CustomStruct { get; set; }

        public override string ToString()
        {
            return "Oxygenize.Test.CustomTypes";
        }
    }

    public struct CustomStruct
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return "Oxygenize.Test.CustomStruct";
        }
    }
}
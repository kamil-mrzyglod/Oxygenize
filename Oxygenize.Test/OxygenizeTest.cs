using NUnit.Framework;

namespace Oxygenize.Test
{
    public class OxygenizeTest
    {
        public const string CustomValueTypeName = "System.SupportedStruct";

        [Test]
        public void Should_Create_Instance_Of_Given_Type()
        {
            var instance = Oxygenize.For<PrimitiveTypes>()
                            .WithStrategy(GenerationStrategy.Mixed)
                            .Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Bool);
            Assert.IsNotNull(instance.Byte);
            Assert.IsNotNull(instance.Char);
            Assert.IsNotNull(instance.Double);
            Assert.IsNotNull(instance.Float);
            Assert.IsNotNull(instance.Int);
            Assert.IsNotNull(instance.Long);
            Assert.IsNotNull(instance.Sbyte);
            Assert.IsNotNull(instance.Short);
            Assert.IsNotNull(instance.Uint);
            Assert.IsNotNull(instance.Ulong);
            Assert.IsNotNull(instance.Ushort);
            Assert.IsTrue(instance.GetType() == typeof(PrimitiveTypes));
        }

        [Test]
        public void Should_Create_Instance_Of_Given_Nullable_Type()
        {
            var instance = Oxygenize.For<NullablePrimitiveTypes>()
                            .WithStrategy(GenerationStrategy.Mixed)
                            .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(NullablePrimitiveTypes));
        }

        [Test]
        public void Should_Create_Instance_Of_Given_Value_Type()
        {
            var instance = Oxygenize.For<ValueTypes>()
                            .WithStrategy(GenerationStrategy.Mixed)
                            .Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.DateTime);
            Assert.IsNotNull(instance.Guid);
            Assert.IsNotNull(instance.TimeSpan);
            Assert.IsTrue(instance.GetType() == typeof(ValueTypes));
        }

        [Test]
        public void Should_Obtain_A_Valid_Value_From_Supported_Types()
        {
            Oxygenize.AddSupport(CustomValueTypeName, () => 1);
            var value = Oxygenize.ObtainValue(CustomValueTypeName);

            Assert.IsTrue((int)value == 1);
        }

        public struct SupportedStruct
        {
            public override string ToString()
            {
                return CustomValueTypeName;
            }
        }

        [Test]
        public void Should_Generate_An_Instance_For_Custom_Types()
        {
            Oxygenize.AddSupport("Oxygenize.Test.CustomStruct", () => new CustomStruct
            {
                Id = 1
            });

            var instance = Oxygenize.For<CustomTypes>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(CustomTypes));
        }

        [Test]
        public void Should_Generate_An_Instance_Of_Primitive_Arrays()
        {
            var instance = Oxygenize.For<PrimitiveTypesArrays>()
                            .UpperBound(500)
                            .Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Bools);
            Assert.IsNotNull(instance.Bytes);
            Assert.IsNotNull(instance.Chars);
            Assert.IsNotNull(instance.Doubles);
            Assert.IsNotNull(instance.Floats);
            Assert.IsNotNull(instance.Ints);
            Assert.IsNotNull(instance.Longs);
            Assert.IsNotNull(instance.Sbytes);
            Assert.IsNotNull(instance.Shorts);
            Assert.IsNotNull(instance.Uints);
            Assert.IsNotNull(instance.Ulongs);
            Assert.IsNotNull(instance.Ushorts);
            Assert.IsTrue(instance.GetType() == typeof(PrimitiveTypesArrays));
        }

        [Test]
        public void Should_Generate_An_Instance_With_Decimal()
        {
            var instance = Oxygenize.For<DecimalTest>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Decimal != 0);
            Assert.IsTrue(instance.GetType() == typeof(DecimalTest));
        }
    }

    public class DecimalTest
    {
        public decimal Decimal { get; set; }

        public decimal? NullableDecimal { get; set; }
    }
}
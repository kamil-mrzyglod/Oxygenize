using NUnit.Framework;

namespace Oxygenize.Test
{
    public class OxygenizeTest
    {
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
    }
}
using System;
using NUnit.Framework;

namespace Oxygenize.Test
{
    public class Oxygenize2Test
    {
        [Test]
        public void GivenPrimitiveType_ShouldGenerateIt()
        {
            Oxygenize2.Oxygenize.Configure<PrimitiveTypes>(configurator =>
            {
                configurator.WithStrategy(Oxygenize2.GenerationStrategy.Random);
            });

            var instance = Oxygenize2.Oxygenize.For<PrimitiveTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Byte != default(byte));
            Assert.IsTrue(instance.Char != default(char));
            Assert.IsTrue(Math.Abs(instance.Double - default(double)) > 0.000001);
            Assert.IsTrue(Math.Abs(instance.Float - default(float)) > float.MinValue);
            Assert.IsTrue(instance.Int != default(int));
            Assert.IsTrue(instance.Long != default(long));
            Assert.IsTrue(instance.Sbyte != default(sbyte));
            Assert.IsTrue(instance.Short != default(short));
            Assert.IsTrue(instance.Uint != default(uint));
            Assert.IsTrue(instance.Ulong != default(ulong));
            Assert.IsTrue(instance.Ushort != default(ushort));
            Assert.IsTrue(instance.GetType() == typeof(PrimitiveTypes));
        }

        [Test]
        public void Should_Create_Instance_Of_Given_Nullable_Type()
        {
            Oxygenize2.Oxygenize.Configure<NullablePrimitiveTypes>(configurator =>
            {
                configurator.WithStrategy(Oxygenize2.GenerationStrategy.Random);
            });

            var instance = Oxygenize2.Oxygenize.For<NullablePrimitiveTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(NullablePrimitiveTypes));
        }

        [Test]
        public void Should_Create_Instance_Of_Given_Value_Type()
        {
            Oxygenize2.Oxygenize.Configure<ValueTypes>(configurator =>
            {
                configurator.WithStrategy(Oxygenize2.GenerationStrategy.Random);
            });

            var instance = Oxygenize2.Oxygenize.For<ValueTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.DateTime != default(DateTime));
            Assert.IsTrue(instance.Guid != default(Guid));
            Assert.IsTrue(instance.TimeSpan != default(TimeSpan));
            Assert.IsTrue(instance.GetType() == typeof(ValueTypes));
        }

        [Test]
        public void Should_Generate_An_Instance_For_Custom_Types()
        {
            Oxygenize2.Oxygenize.Configure<CustomTypes>(configurator =>
            {
                configurator.WithStrategy(Oxygenize2.GenerationStrategy.Random);
            });

            var instance = Oxygenize2.Oxygenize.For<CustomTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(CustomTypes));
        }

        [Test]
        public void Should_Generate_An_Instance_For_Custom_Types_With_Random_Value()
        {
            Oxygenize2.Oxygenize.Configure<CustomTypes>(configurator =>
            {
                configurator.WithStrategy(Oxygenize2.GenerationStrategy.Random);
            });

            var instance = Oxygenize2.Oxygenize.For<CustomTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(CustomTypes));
            Assert.IsTrue(instance.CustomStruct.Id != default(int));
        }

        private class CustomTypes
        {
            public CustomStruct CustomStruct { get; set; }

            public override string ToString()
            {
                return "Oxygenize.Test.CustomTypes";
            }
        }

        private struct CustomStruct
        {
            public int Id { get; set; }

            public override string ToString()
            {
                return "Oxygenize.Test.CustomStruct";
            }
        }

        private class PrimitiveTypes
        {
            public bool Bool { get; set; }

            public byte Byte { get; set; }

            public char Char { get; set; }

            public double Double { get; set; }

            public int Int { get; set; }

            public long Long { get; set; }

            public sbyte Sbyte { get; set; }

            public short Short { get; set; }

            public float Float { get; set; }

            public ushort Ushort { get; set; }

            public uint Uint { get; set; }

            public ulong Ulong { get; set; }
        }

        private class NullablePrimitiveTypes
        {
            public bool? Bool { get; set; }

            public byte? Byte { get; set; }

            public char? Char { get; set; }

            public double? Double { get; set; }

            public int? Int { get; set; }

            public long? Long { get; set; }

            public sbyte? Sbyte { get; set; }

            public short? Short { get; set; }

            public float? Float { get; set; }

            public ushort? Ushort { get; set; }

            public uint? Uint { get; set; }

            public ulong? Ulong { get; set; }
        }

        private class ValueTypes
        {
            public DateTime DateTime { get; set; }

            public Guid Guid { get; set; }

            public TimeSpan TimeSpan { get; set; }

            //public TimeZone TimeZone { get; set; }
        }
    }
}
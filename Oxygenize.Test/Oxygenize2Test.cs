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
    }
}
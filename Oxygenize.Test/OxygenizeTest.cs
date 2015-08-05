﻿using System.Collections.Generic;
using NUnit.Framework;
using Oxygenize.Test.TestClasses;

namespace Oxygenize.Test
{
    using System;

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
            Assert.IsTrue(instance.DateTime != default(DateTime));
            Assert.IsTrue(instance.Guid != default(Guid));
            Assert.IsTrue(instance.TimeSpan != default(TimeSpan));
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
            Oxygenize.AddSupport("Oxygenize.Test.TestClasses.CustomStruct", () => new CustomStruct
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
            var instance = Oxygenize.For<DecimalTestClass>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Decimal != 0);
            Assert.IsNotNull(instance.Decimals);
            Assert.IsTrue(instance.GetType() == typeof(DecimalTestClass));
        }

        [Test]
        public void Should_Generate_An_Instance_With_Random_Enum_Value()
        {
            var instance = Oxygenize.For<ClassWithEnums>().Instance;

            TestEnum enumValue;
            Assert.IsNotNull(instance);
            Assert.IsTrue(Enum.TryParse(instance.Enum.ToString(), out enumValue));
            Assert.IsTrue(instance.GetType() == typeof(ClassWithEnums));
        }

        [Test]
        public void Should_Generate_An_Instance_With_Non_Empty_Collections()
        {
            var instance = Oxygenize.For<Collections>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Ints);
            Assert.IsNotEmpty(instance.Ints);
            Assert.IsTrue(instance.GetType() == typeof(Collections));
        }
    }

    public class Collections
    {
        public IEnumerable<int> Ints { get; set; }
    }
}
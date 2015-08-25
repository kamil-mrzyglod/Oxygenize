using System.Collections.Generic;
using System.Linq;
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
            var instance = Oxygenize.For<PrimitiveTypes>().Instance;

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
            var instance = Oxygenize.For<NullablePrimitiveTypes>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(NullablePrimitiveTypes));
        }

        [Test]
        public void Should_Create_Instance_Of_Given_Value_Type()
        {
            var instance = Oxygenize.For<ValueTypes>().Instance;

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
            Assert.IsInstanceOf<IEnumerable<int>>(instance.Ints);
            Assert.IsNotNull(instance.CollectionInts);
            Assert.IsNotEmpty(instance.CollectionInts);
            Assert.IsInstanceOf<ICollection<int>>(instance.CollectionInts);
            Assert.IsNotNull(instance.ListInts);
            Assert.IsNotEmpty(instance.ListInts);
            Assert.IsInstanceOf<IList<int>>(instance.ListInts);
            Assert.IsNotNull(instance.Dictionary);
            Assert.IsNotEmpty(instance.Dictionary);
            Assert.IsInstanceOf<IDictionary<int, decimal>>(instance.Dictionary);
            Assert.IsTrue(instance.GetType() == typeof(Collections));
        }

        [Test]
        public void Should_Generate_An_Instance_With_String()
        {
            var instance = Oxygenize.For<StringsClass>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.String);
            Assert.IsNotNull(instance.Strings);
            Assert.IsNotEmpty(instance.Strings);
            Assert.IsNotNull(instance.EnumerableStrings);
            Assert.IsNotEmpty(instance.EnumerableStrings);
            Assert.IsNotNull(instance.CollectionStrings);
            Assert.IsNotEmpty(instance.CollectionStrings);
            Assert.IsNotNull(instance.ListStrings);
            Assert.IsNotEmpty(instance.ListStrings);
            Assert.IsTrue(instance.GetType() == typeof(StringsClass));
        }

        [Test]
        public void Should_Generate_An_Instance_With_String_Which_Can_Be_Null()
        {
            var instance = Oxygenize.For<StringsClass>()
                                    .NullableReferenceTypes(true)
                                    .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(StringsClass));
        }

        [Test]
        public void Should_Generate_An_Instance_With_Reference_Types()
        {
            Oxygenize.AddSupport(typeof(PrimitiveTypes).ToString(), () => Oxygenize.For<PrimitiveTypes>().Instance);
            Oxygenize.AddSupport(typeof(Collections).ToString(), () => Oxygenize.For<Collections>().Instance);

            var instance = Oxygenize.For<InstanceTypes>().Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.PrimitiveTypes);
            Assert.IsNotNull(instance.Collections);
            Assert.IsTrue(instance.GetType() == typeof(InstanceTypes));
        }

        class InstanceTypes
        {
            public PrimitiveTypes PrimitiveTypes { get; set; }

            public Collections Collections { get; set; }
        }

        [Test]
        public void Should_Generate_Strings_Not_Exceeding_Given_Length()
        {
            var instance = Oxygenize.For<StringArray>()
                                    .UpperBound(100)
                                    .MaxStringLength(100)
                                    .MinStringLength(50)
                                    .NullableReferenceTypes(false)
                                    .Instance;

            Assert.IsTrue(instance.Strings.All(x => x.Length <= 100));
            Assert.IsTrue(instance.Strings.All(x => x.Length >= 50));
        }

        class StringArray
        {
            public string[] Strings { get; set; }
        }

        [Test]
        public void Should_Execute_Selected_Constructor()
        {
            var instance = Oxygenize.For<ConstructorsTest>()
                                    .WithConstructor(new []{typeof(int), typeof(string)}, new object[]{2, "TESTTEST"})
                                    .Instance;

            Assert.IsTrue(instance.Int == 2);
            Assert.IsTrue(instance.String == "TESTTEST");

            var baseInstance = Oxygenize.For<ConstructorsTest>().Instance;
            Assert.IsTrue(baseInstance.Int == 1);
            Assert.IsTrue(baseInstance.String == "TEST");
        }

        [Test]
        public void Should_Create_An_Instance_With_Custom_Strategy()
        {
            var instance = Oxygenize.For<PrimitiveTypes>()
                                    .WithStrategy(GenerationStrategy.Custom)
                                    .Configure()
                                        .Prop(x => x.Bool)
                                            .WithValue(true)
                                            .Set()
                                        .Prop(x => x.Int)
                                            .WithValue(123)
                                            .Set()
                                        .Compile()
                                    .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Bool);
            Assert.IsTrue(instance.Int == 123);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "You cannot configure an instance for RandomGenerationStrategy.")]
        public void Should_Throw_An_Exception_When_Trying_To_Configure_Properties_Using_Random_Strategy()
        {
            var instance = Oxygenize.For<PrimitiveTypes>()
                                    .Configure()
                                        .Prop(x => x.Bool)
                                            .WithValue(true)
                                            .Set()
                                        .Prop(x => x.Int)
                                            .WithValue(123)
                                            .Set()
                                        .Compile()
                                    .Instance;
        }

        [Test]
        public void Should_Create_An_Instance_With_Mixed_Strategy()
        {
            var instance = Oxygenize.For<PrimitiveTypes>()
                                    .WithStrategy(GenerationStrategy.Mixed)
                                    .Configure()
                                        .Prop(x => x.Bool)
                                            .WithValue(true)
                                            .Set()
                                        .Prop(x => x.Int)
                                            .WithValue(123)
                                            .Set()
                                        .Compile()
                                    .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Bool);
            Assert.IsTrue(instance.Int == 123);
            Assert.IsTrue(instance.Long != default(long));
            Assert.IsTrue(instance.Byte != default(byte));
            Assert.IsTrue(instance.Char != default(char));
            Assert.IsTrue(Math.Abs(instance.Double - default(double)) > 0.000001);
            Assert.IsTrue(Math.Abs(instance.Float - default(float)) > float.MinValue);
            Assert.IsTrue(instance.Sbyte != default(sbyte));
            Assert.IsTrue(instance.Short != default(short));
            Assert.IsTrue(instance.Uint != default(uint));
            Assert.IsTrue(instance.Ulong != default(ulong));
            Assert.IsTrue(instance.Ushort != default(ushort));
            Assert.IsTrue(instance.GetType() == typeof(PrimitiveTypes));
        }

        [Test]
        public void Should_Create_An_Instance_With_Properties_Masks()
        {
            var instance = Oxygenize.For<StringsClass>()
                                    .WithStrategy(GenerationStrategy.Mixed)
                                    .Configure()
                                        .Prop(x => x.String)
                                            .Mask("000")
                                            .Set()
                                        .Compile()
                                    .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.String.Length == 3);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Cannot set mask for the type other than string.")]
        public void Should_Throw_An_Exception_When_Trying_To_Set_Mask_For_Other_Type_Than_String()
        {
            var instance = Oxygenize.For<PrimitiveTypes>()
                                    .WithStrategy(GenerationStrategy.Mixed)
                                    .Configure()
                                        .Prop(x => x.Int)
                                            .Mask("000")
                                            .Set()
                                        .Compile()
                                    .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Int.ToString().Length == 3);
        }

        [Test]
        public void Should_Create_An_Instance_With_Properties_Masks_And_Custom_Placeholder()
        {
            var instance = Oxygenize.For<StringsClass>()
                                    .WithStrategy(GenerationStrategy.Mixed)
                                    .Configure()
                                        .Prop(x => x.String)
                                            .Mask("???-???", '?')
                                            .Set()
                                        .Compile()
                                    .Instance;

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.String.Length == 7);
            Assert.IsTrue(instance.String.Contains("-"));
        }
    }
}
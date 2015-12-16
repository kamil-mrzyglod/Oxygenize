namespace Oxygenize.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TestClasses;

    public class OxygenizeTest
    {
        [Test]
        public void GivenPrimitiveType_ShouldGenerateIt()
        {
            Oxygenize.Configure<PrimitiveTypes>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<PrimitiveTypes>();

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
            Oxygenize.Configure<NullablePrimitiveTypes>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<NullablePrimitiveTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(NullablePrimitiveTypes));
        }

        [Test]
        public void Should_Create_Instance_Of_Given_Value_Type()
        {
            Oxygenize.Configure<ValueTypes>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<ValueTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.DateTime != default(DateTime));
            Assert.IsTrue(instance.Guid != default(Guid));
            Assert.IsTrue(instance.TimeSpan != default(TimeSpan));
            Assert.IsTrue(instance.GetType() == typeof(ValueTypes));
        }

        [Test]
        public void Should_Generate_An_Instance_For_Custom_Types()
        {
            Oxygenize.Configure<CustomTypes>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<CustomTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(CustomTypes));
        }

        [Test]
        public void Should_Generate_An_Instance_For_Custom_Types_With_Concrete_Value()
        {
            Oxygenize.Configure<CustomTypes>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
                configurator.Concrete<CustomStruct>(() => new CustomStruct { Id = 1 });
            });

            var instance = Oxygenize.For<CustomTypes>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(CustomTypes));
            Assert.IsTrue(instance.CustomStruct.Id == 1);
        }

        private class CustomTypes
        {
            public CustomStruct CustomStruct { get; set; }

            public override string ToString()
            {
                return "Oxygenize.Test.CustomTypes";
            }
        }

        [Test]
        public void Should_Generate_An_Instance_Of_Primitive_Arrays()
        {
            Oxygenize.Configure<PrimitiveTypesArrays>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
                configurator.WithMaximumCapacity(500);
            });

            var instance = Oxygenize.For<PrimitiveTypesArrays>();

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
            Oxygenize.Configure<DecimalTestClass>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<DecimalTestClass>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.Decimal != 0);
            Assert.IsNotNull(instance.Decimals);
            Assert.IsTrue(instance.GetType() == typeof(DecimalTestClass));
        }

        [Test]
        public void Should_Generate_An_Instance_With_Random_Enum_Value()
        {
            Oxygenize.Configure<ClassWithEnums>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<ClassWithEnums>();

            TestEnum enumValue;
            Assert.IsNotNull(instance);
            Assert.IsTrue(Enum.TryParse(instance.Enum.ToString(), out enumValue));
            Assert.IsTrue(instance.GetType() == typeof(ClassWithEnums));
        }

        [Test]
        public void Should_Generate_An_Instance_With_Non_Empty_Collections()
        {
            Oxygenize.Configure<Collections>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<Collections>();

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
            Oxygenize.Configure<StringsClass>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<StringsClass>();

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
            Oxygenize.Configure<StringsClass>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
                configurator.WithNullableReferenceTypes(true);
            });

            var instance = Oxygenize.For<StringsClass>();

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.GetType() == typeof(StringsClass));
        }

        [Test]
        [Ignore("Need adding support for random generation of custom types")]
        public void Should_Generate_An_Instance_With_Reference_Types()
        {
            Oxygenize.Configure<InstanceTypes>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
            });

            var instance = Oxygenize.For<InstanceTypes>();

            /*Oxygenize.AddSupport(typeof(PrimitiveTypes).ToString(), () => Oxygenize.For<PrimitiveTypes>().Instance);
            Oxygenize.AddSupport(typeof(Collections).ToString(), () => Oxygenize.For<Collections>().Instance);*/

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
            Oxygenize.Configure<StringArray>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
                configurator.WithMaximumCapacity(100);
                configurator.WithMaxStringLength(100);
                configurator.WithMinStringLength(50);
            });

            var instance = Oxygenize.For<StringArray>();

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
            Oxygenize.Configure<ConstructorsTest>(configurator =>
            {
                configurator.WithStrategy(GenerationStrategy.Random);
                configurator.WithCtorParameters(new Tuple<Type[], object[]>(new[] { typeof(int), typeof(string) }, new object[] { 2, "TESTTEST" }));
            });

            var instance = Oxygenize.For<ConstructorsTest>();

            Assert.IsTrue(instance.Int == 2);
            Assert.IsTrue(instance.String == "TESTTEST");
        }

        /*[Test]
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

        [Test]
        public void Should_Create_An_Instance_Of_Poco()
        {
            var instance = Oxygenize.For<EfPocoTest>()
                                    .WithStrategy(GenerationStrategy.Mixed)
                                        .Configure()
                                            .Prop(x => x.Companies)
                                                .WithValue(Enumerable.Empty<Company>().ToList())
                                                .Set()
                                            .Compile()
                                    .Instance;

            Assert.IsNotNull(instance);
        }*/
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Oxygenize.Generators
{
    class RandomStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public RandomStrategyGenerator(int upperBound, bool nullableReferenceTypes)
            : base(upperBound, nullableReferenceTypes)
        {
        }

        protected override T Generate()
        {
            SetProperties();
            return Instance;
        }

        protected override void SetProperties()
        {
            foreach (var property in Type.GetProperties())
            {
                var value = GetRandomValue(property.PropertyType);
                property.SetValue(Instance, value);
            }
        }

        private object GetRandomValue(Type propertyType, bool cannotBeNull = false)
        {
            if (propertyType.IsPrimitive)
            {
                return SetPrimitiveValue(propertyType);
            }

            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                return SetNullableValue(propertyType);
            }

            if (propertyType.IsEnum)
            {
                return GetEnumValue(propertyType);
            }

            if (propertyType.IsGenericType)
            {
                return GetGenericTypeValue(propertyType);
            }

            if (propertyType.IsArray)
            {
                return GenerateArray(propertyType);
            }

            if (propertyType.IsValueType)
            {
                return GetValueType(propertyType);
            }

            return GetReferenceTypeValue(propertyType, cannotBeNull);
        }

        private object GetReferenceTypeValue(Type propertyType, bool cannotBeNull = false)
        {
            if (NullableReferenceTypes && !cannotBeNull)
            {
                return Randomizer.ShouldEnter() ? GetRandomReferenceTypeValue(propertyType) : null;
            }

            return GetRandomReferenceTypeValue(propertyType);
        }

        private static object GetRandomReferenceTypeValue(Type propertyType)
        {
            var randomizer = new Randomizer().Instance;

            switch (propertyType.ToString())
            {
                case "System.String":
                    return new string(Enumerable.Repeat(Chars, randomizer.Next(4000)).Select(s => s[randomizer.Next(s.Length)]).ToArray());
                default:
                    return Oxygenize.ObtainValue(propertyType.ToString());
            }
        }

        private object GetGenericTypeValue(Type propertyType)
        {
            var randomizer = new Randomizer().Instance;

            if (propertyType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
            {
                var genericType = propertyType.GetGenericArguments()[0];
                return GetRandomEnumerable(genericType, randomizer);
            }

            if (propertyType.GetGenericTypeDefinition() == typeof(ICollection<>) || propertyType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                var genericType = propertyType.GetGenericArguments()[0];
                var methodInfo = GetRandomEnumerable(genericType, randomizer);

                return typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(genericType).Invoke(null, new object[] { methodInfo });
            }

            if (propertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                var keyType = propertyType.GetGenericArguments()[0];
                var valueType = propertyType.GetGenericArguments()[1];

                var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                var dictionaryInstance = Activator.CreateInstance(dictionaryType);
                var addMethod = dictionaryType.GetMethod("Add");
                for (var i = 0; i <= randomizer.Next(UpperBound); i++)
                {
                    addMethod.Invoke(dictionaryInstance, new[] { GetRandomValue(keyType, true), GetRandomValue(valueType) });
                }

                return dictionaryInstance;
            }

            return new object();
        }

        private IEnumerable GetRandomEnumerable(Type genericType, Random randomizer)
        {
            var enumerable = Enumerable.Range(0, randomizer.Next(UpperBound)).Select(x => GetRandomValue(genericType));
            var methodInfo = typeof (Enumerable).GetMethod("Cast")
                .MakeGenericMethod(genericType)
                .Invoke(null, new object[] {enumerable}) as IEnumerable;
            return methodInfo;
        }

        private static Enum GetEnumValue(Type propertyType)
        {
            var values = Enum.GetValues(propertyType);
            var randomizer = new Randomizer().Instance;
            return (Enum)values.GetValue(randomizer.Next(values.Length));
        }

        private Array GenerateArray(Type propertyType)
        {
            var elementType = propertyType.GetElementType();
            var randomizer = new Randomizer().Instance;

            var array = Array.CreateInstance(elementType, randomizer.Next(1, UpperBound));
            var value = Enumerable.Range(0, array.Length - 1).Select(x => GetRandomValue(elementType)).ToArray();
            Array.Copy(value, array, value.Length);

            return array;
        }

        private object SetNullableValue(Type propertyType)
        {
            if (!Randomizer.ShouldEnter())
            {
                return null;
            }

            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (propertyType.IsValueType && !underlyingType.IsPrimitive)
            {
                return GetValueType(propertyType, underlyingType);
            }

            return SetPrimitiveValue(propertyType, underlyingType);
        }

        private static object SetPrimitiveValue(Type propertyType, Type underlyingType = null)
        {
            var type = underlyingType ?? propertyType;
            var value = GetRandomPrimitiveValue(type);

            return value;
        }

        private static object GetRandomPrimitiveValue(Type propertyType)
        {
            var randomizer = new Randomizer().Instance;

            object value;
            switch (propertyType.ToString())
            {
                case "System.Boolean":
                    value = Randomizer.ShouldEnter();
                    break;
                case "System.Byte":
                    var @byte = new byte[1];
                    randomizer.NextBytes(@byte);
                    value = @byte[0];
                    break;
                case "System.Char":
                    value = Chars[randomizer.Next(0, Chars.Length - 1)];
                    break;
                case "System.Double":
                    value = randomizer.NextDouble();
                    break;
                case "System.Int32":
                    value = randomizer.Next();
                    break;
                case "System.Int64":
                    value = (long) randomizer.Next();
                    break;
                case "System.SByte":
                    var @sbyte = new byte[1];
                    randomizer.NextBytes(@sbyte);
                    value = (sbyte) @sbyte[0];
                    break;
                case "System.Int16":
                    value = (short) randomizer.Next(1 << 16);
                    break;
                case "System.Single":
                    var singleBytes = new byte[8];
                    randomizer.NextBytes(singleBytes);
                    value = BitConverter.ToSingle(singleBytes, 0);
                    break;
                case "System.UInt16":
                    var shortBytes = new byte[2];
                    randomizer.NextBytes(shortBytes);
                    value = BitConverter.ToUInt16(shortBytes, 0);
                    break;
                case "System.UInt32":
                    var bytes = new byte[4];
                    randomizer.NextBytes(bytes);
                    value = BitConverter.ToUInt32(bytes, 0);
                    break;
                case "System.UInt64":
                    var longBytes = new byte[8];
                    randomizer.NextBytes(longBytes);
                    value = BitConverter.ToUInt64(longBytes, 0);
                    break;
                default:
                    value = new object();
                    break;
            }
            return value;
        }

        public object GetValueType(Type propertyType, Type underlyingType = null)
        {
            var type = underlyingType ?? propertyType;
            var randomizer = new Randomizer().Instance;

            object value;
            switch (type.ToString())
            {
                case "System.DateTime":
                    var range = DateTime.MaxValue - DateTime.MinValue;
                    var randTimeSpan = new TimeSpan((long)(randomizer.NextDouble() * range.Ticks));
                    value = DateTime.MinValue + randTimeSpan;
                    break;
                case "System.Guid":
                    value = Guid.NewGuid();
                    break;
                case "System.TimeSpan":
                    value = new TimeSpan(randomizer.Next());
                    break;
                case "System.Decimal":
                    var scale = (byte) randomizer.Next(29);
                    var sign = randomizer.Next(2) == 1;
                    value = new decimal(randomizer.NextInt32(), randomizer.NextInt32(), randomizer.NextInt32(), sign, scale);
                    break;
                default:
                    value = Oxygenize.ObtainValue(type.ToString());
                    break;
            }

            return value;
        }
    }
}
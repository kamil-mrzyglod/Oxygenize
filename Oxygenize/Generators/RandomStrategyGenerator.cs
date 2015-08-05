using System;
using System.Linq;
using System.Reflection;

namespace Oxygenize.Generators
{
    class RandomStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
        public RandomStrategyGenerator(int upperBound)
            : base(upperBound)
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
                object value;
                if (property.PropertyType.IsPrimitive)
                {
                    value = SetPrimitiveValue(property);
                    property.SetValue(Instance, value);
                    continue;
                }

                var nullableType = Nullable.GetUnderlyingType(property.PropertyType);
                if (nullableType != null)
                {
                    value = SetNullableValue(property);
                    property.SetValue(Instance, value);
                    continue;
                }

                if (property.PropertyType.IsValueType)
                {
                    value = SetValueType(property);
                    property.SetValue(Instance, value);
                }

                if (!property.PropertyType.IsArray)
                {
                    continue;
                }

                value = GenerateArray(property);
                property.SetValue(Instance, value);
            }
        }

        private Array GenerateArray(PropertyInfo property)
        {
            var elementType = property.PropertyType.GetElementType();
            var randomizer = new Randomizer().Instance;

            var array = Array.CreateInstance(elementType, randomizer.Next(1, UpperBound));
            var value = Enumerable.Range(0, array.Length - 1).Select(x => GetRandomValue(elementType)).ToArray();
            Array.Copy(value, array, value.Length);

            return array;
        }

        private object SetNullableValue(PropertyInfo property)
        {
            if (!Randomizer.ShouldEnter())
            {
                return null;
            }

            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
            if (property.PropertyType.IsValueType && !underlyingType.IsPrimitive)
            {
                return SetValueType(property, underlyingType);
            }

            return SetPrimitiveValue(property, underlyingType);
        }

        private static object SetPrimitiveValue(PropertyInfo property, Type type = null)
        {
            var propertyType = type ?? property.PropertyType;
            var value = GetRandomValue(propertyType);

            return value;
        }

        private static object GetRandomValue(Type propertyType)
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

        public object SetValueType(PropertyInfo property, Type type = null)
        {
            var propertyType = type ?? property.PropertyType;
            var randomizer = new Randomizer().Instance;

            object value;
            switch (propertyType.ToString())
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
                    value = Oxygenize.ObtainValue(property.PropertyType.ToString());
                    break;
            }

            return value;
        }
    }
}
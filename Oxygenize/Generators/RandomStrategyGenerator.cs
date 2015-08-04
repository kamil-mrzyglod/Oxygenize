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
                if (property.PropertyType.IsPrimitive)
                {
                    SetPrimitiveValue(property);
                }
                else
                {
                    var nullableType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (nullableType != null)
                    {
                        SetNullableValue(property);
                    }
                    else
                    {
                        if (property.PropertyType.IsValueType)
                        {
                            SetValueType(property);
                        }

                        if (property.PropertyType.IsArray)
                        {
                            var elementType = property.PropertyType.GetElementType();
                            var randomizer = new Randomizer().Instance;

                            var array = Array.CreateInstance(elementType, randomizer.Next(1, UpperBound));
                            var value = Enumerable.Range(0, array.Length - 1).Select(x => GetRandomValue(elementType)).ToArray();
                            Array.Copy(value, array, value.Length);

                            property.SetValue(Instance, array);
                        }
                    }
                }           
            }
        }

        private void SetNullableValue(PropertyInfo property)
        {
            if (Randomizer.ShouldEnter())
            {
                SetPrimitiveValue(property, Nullable.GetUnderlyingType(property.PropertyType));
            }
        }

        private void SetPrimitiveValue(PropertyInfo property, Type type = null)
        {
            var propertyType = type ?? property.PropertyType;
            var value = GetRandomValue(propertyType);

            property.SetValue(Instance, value);
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
                    value = (float) randomizer.Next(1 << 32);
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

        public void SetValueType(PropertyInfo property)
        {
            var randomizer = new Randomizer().Instance;

            object value;
            switch (property.PropertyType.ToString())
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
                default:
                    value = Oxygenize.ObtainValue(property.PropertyType.ToString());
                    break;
            }

            property.SetValue(Instance, value);
        }
    }
}
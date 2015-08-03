using System.Reflection;

namespace Oxygenize.Generators
{
    class RandomStrategyGenerator<T> : GeneratorBase<T> where T : new()
    {
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
            }
        }

        private void SetPrimitiveValue(PropertyInfo property)
        {
            object value;
            switch (property.PropertyType.ToString())
            {
                case "System.Boolean":
                    value = true;
                    break;
                case "System.Byte":
                    value = new byte();
                    break;
                case "System.Char":
                    value = new char();
                    break;
                case "System.Double":
                    value = 10d;
                    break;
                case "System.Int32":
                    value = 1;
                    break;
                case "System.Int64":
                    value = 10L;
                    break;
                case "System.SByte":
                    value = new sbyte();
                    break;
                case "System.Int16":
                    value = new short();
                    break;
                case "System.Single":
                    value = new float();
                    break;
                case "System.UInt32":
                    value = new uint();
                    break;
                case "System.UInt64":
                    value = new ulong();
                    break;
                default:
                    value = new object();
                    break;
            }

            property.SetValue(Instance, value);
        }
    }
}
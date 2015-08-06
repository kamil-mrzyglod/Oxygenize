using System;

namespace Oxygenize.Generators
{
    abstract class GeneratorBase<T> where T : new()
    {
        protected const string Chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";

        protected int UpperBound; 
        protected bool NullableReferenceTypes; 

        protected Type Type;
        protected T Instance;

        protected GeneratorBase(int upperBound, bool nullableReferenceTypes)
        {
            UpperBound = upperBound;
            NullableReferenceTypes = nullableReferenceTypes;
        }  

        public T GetData()
        {
            Type = typeof (T);
            Instance = new T();

            return Generate();
        }

        protected abstract T Generate();
        protected abstract void SetProperties();
    }
}
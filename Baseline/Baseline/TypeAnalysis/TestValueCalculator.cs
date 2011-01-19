using System;
using System.Collections.Generic;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TypeAnalysis
{
    public abstract class TestValueCalculator
    {

        public static readonly Type SBYTE_TYPE = typeof(SByte);
        public static readonly Type BYTE_TYPE = typeof(Byte);
        public static readonly Type SHORT_TYPE = typeof(Int16);
        public static readonly Type USHORT_TYPE = typeof(UInt16);
        public static readonly Type INT_TYPE = typeof(Int32);
        public static readonly Type UINT_TYPE = typeof(UInt32);
        public static readonly Type LONG_TYPE = typeof(Int64);
        public static readonly Type ULONG_TYPE = typeof(UInt64);
        public static readonly Type CHAR_TYPE = typeof(Char);
        public static readonly Type FLOAT_TYPE = typeof(Single);
        public static readonly Type DOUBLE_TYPE = typeof(Double);
        public static readonly Type BOOL_TYPE = typeof(bool);
        public static readonly Type DECIMAL_TYPE = typeof(Decimal);
        public static readonly Type OBJECT_TYPE = typeof(Object);
        public static readonly Type STRING_TYPE = typeof(String);
     


        public List<ObjectInstance> GetTestValues(Type t)
        {
            if (t.IsEnum)
                return GetEnumTestValues(t);
            if (t == SBYTE_TYPE)
                return GetSByteTestValues();
            if (t == BYTE_TYPE)
                return GetByteTestValues();
            if (t == SHORT_TYPE)
                return GetShortTestValues();
            if (t == USHORT_TYPE)
                return GetUShortTestValues();
            if (t == INT_TYPE)
                return GetIntTestValues();
            if (t == UINT_TYPE)
                return GetUIntTestValues();
            if (t == LONG_TYPE)
                return GetLongTestValues();
            if (t == ULONG_TYPE)
                return GetULongTestValues();
            if (t == CHAR_TYPE)
                return GetCharTestValues();
            if (t == FLOAT_TYPE)
                return GetFloatTestValues();
            if (t == DOUBLE_TYPE)
                return GetDoubleTestValues();
            if (t == BOOL_TYPE)
                return GetBoolTestValues();
            if (t == DECIMAL_TYPE)
                return GetDecimalTestValues();
            if (t == OBJECT_TYPE)
                return GetObjectTestValues();
            if (t == STRING_TYPE)
                return GetStringTestValues();
      
            
            return GetTestValuesFor(t);
        }

        protected abstract List<ObjectInstance> GetEnumTestValues(Type type);


        protected abstract List<ObjectInstance> GetTestValuesFor(Type type);

        protected abstract List<ObjectInstance> GetStringTestValues();

        protected abstract List<ObjectInstance> GetObjectTestValues();

        protected abstract List<ObjectInstance> GetDecimalTestValues();

        protected abstract List<ObjectInstance> GetBoolTestValues();

        protected abstract List<ObjectInstance> GetDoubleTestValues();

        protected abstract List<ObjectInstance> GetFloatTestValues();

        protected abstract List<ObjectInstance> GetCharTestValues();

        protected abstract List<ObjectInstance> GetULongTestValues();

        protected abstract List<ObjectInstance> GetLongTestValues();

        protected abstract List<ObjectInstance> GetUIntTestValues();

        protected abstract List<ObjectInstance> GetIntTestValues();

        protected abstract List<ObjectInstance> GetUShortTestValues();

        protected abstract List<ObjectInstance> GetShortTestValues();

        protected abstract List<ObjectInstance> GetByteTestValues();

        protected abstract List<ObjectInstance> GetSByteTestValues();
    }
}
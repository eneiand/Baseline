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



        public List<IObjectInstance> GetTestValues(Type t)
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

        protected abstract List<IObjectInstance> GetEnumTestValues(Type type);


        protected abstract List<IObjectInstance> GetTestValuesFor(Type type);

        protected abstract List<IObjectInstance> GetStringTestValues();

        protected abstract List<IObjectInstance> GetObjectTestValues();

        protected abstract List<IObjectInstance> GetDecimalTestValues();

        protected abstract List<IObjectInstance> GetBoolTestValues();

        protected abstract List<IObjectInstance> GetDoubleTestValues();

        protected abstract List<IObjectInstance> GetFloatTestValues();

        protected abstract List<IObjectInstance> GetCharTestValues();

        protected abstract List<IObjectInstance> GetULongTestValues();

        protected abstract List<IObjectInstance> GetLongTestValues();

        protected abstract List<IObjectInstance> GetUIntTestValues();

        protected abstract List<IObjectInstance> GetIntTestValues();

        protected abstract List<IObjectInstance> GetUShortTestValues();

        protected abstract List<IObjectInstance> GetShortTestValues();

        protected abstract List<IObjectInstance> GetByteTestValues();

        protected abstract List<IObjectInstance> GetSByteTestValues();
    }
}
using System;
using System.Collections.Generic;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TypeAnalysis
{
    public class DefaultTestValueCalculator : TestValueCalculator
    {
        protected override List<ObjectInstance> GetTestValuesFor(Type type)
        {
            throw new NotImplementedException();
        }

        protected override List<ObjectInstance> GetStringTestValues()
        {
            throw new NotImplementedException();
        }

        protected override List<ObjectInstance> GetObjectTestValues()
        {
            throw new NotImplementedException();
        }

        protected override List<ObjectInstance> GetDecimalTestValues()
        {
            var vals = GetMaxAndMin(DECIMAL_TYPE);
            vals.AddRange(new List<ObjectInstance>()
            {
                new ObjectInstance(Decimal.MinusOne),
                new ObjectInstance(Decimal.Zero),
                new ObjectInstance(Decimal.One)
            });

            return vals;
        }

        protected override List<ObjectInstance> GetBoolTestValues()
        {
            return new List<ObjectInstance>()
            {
                new ObjectInstance(true),
                new ObjectInstance(false)
            };
        }

        protected override List<ObjectInstance> GetDoubleTestValues()
        {
            return GetMaxAndMin(DOUBLE_TYPE);
        }

        protected override List<ObjectInstance> GetFloatTestValues()
        {
            return GetMaxAndMin(FLOAT_TYPE);
        }

        protected override List<ObjectInstance> GetCharTestValues()
        {
            return GetMaxAndMin(CHAR_TYPE);
        }

        protected override List<ObjectInstance> GetULongTestValues()
        {
            return GetMaxAndMin(ULONG_TYPE);
        }

        protected override List<ObjectInstance> GetLongTestValues()
        {
            return GetMaxAndMin(LONG_TYPE);
        }

        protected override List<ObjectInstance> GetUIntTestValues()
        {
            return GetMaxAndMin(UINT_TYPE);

        }

        protected override List<ObjectInstance> GetIntTestValues()
        {
            return GetMaxAndMin(INT_TYPE);
        }

        protected override List<ObjectInstance> GetUShortTestValues()
        {
            return GetMaxAndMin(USHORT_TYPE);

        }

        protected override List<ObjectInstance> GetShortTestValues()
        {
            return GetMaxAndMin(SHORT_TYPE);

        }

        protected override List<ObjectInstance> GetByteTestValues()
        {
            return GetMaxAndMin(BYTE_TYPE);

        }

        protected override List<ObjectInstance> GetSByteTestValues()
        {
            return GetMaxAndMin(SBYTE_TYPE);

        }

        private static List<ObjectInstance> GetMaxAndMin(Type t)
        {
            return new List<ObjectInstance>()
            {
                new ObjectInstance(t.GetField("MaxValue").GetValue(null)),
                new ObjectInstance(t.GetField("MinValue").GetValue(null))
            };
        }
    }
}
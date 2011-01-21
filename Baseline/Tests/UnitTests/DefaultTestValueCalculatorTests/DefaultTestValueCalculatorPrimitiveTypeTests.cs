using System;
using System.Collections.Generic;
using Baseline.TypeAnalysis;
using Baseline.TypeAnalysis.ObjectInstantiation;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.UnitTests
{
    [TestFixture]
    internal class DefaultTestValueCalculatorPrimitiveTypeTests
    {
        private readonly TestValueCalculator m_TestValueCalc = new DefaultTestValueCalculator();

        [Test]
        public void EnumTest()
        {
            List<Object> enumValues = new List<Object>
                ();

            foreach(var e in Enum.GetValues(typeof(StringComparison)))
            {
                enumValues.Add(e);
            }
            
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(typeof(StringComparison));
            Assert.That(vals.Count == enumValues.Count);
            for (int i = 0; i < vals.Count; ++i)
            {
                Assert.That(vals[i].Instance, Is.EqualTo(enumValues[i]));
            }
        }

        [Test]
        public void BoolTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.BOOL_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(vals[0].Instance, Is.True);
            Assert.That(vals[1].Instance, Is.False);
        }

        [Test]
        public void ByteTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.BYTE_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Byte.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Byte.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void CharTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.CHAR_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Char.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Char.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void DecimalTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.DECIMAL_TYPE);
            Assert.That(vals.Count == 5);
            Assert.That(Decimal.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Decimal.MinValue, Is.EqualTo(vals[1].Instance));
            Assert.That(Decimal.MinusOne, Is.EqualTo(vals[2].Instance));
            Assert.That(Decimal.Zero, Is.EqualTo(vals[3].Instance));
            Assert.That(Decimal.One, Is.EqualTo(vals[4].Instance));
        }

        [Test]
        public void DoubleTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.DOUBLE_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Double.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Double.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void FloatTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.FLOAT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Single.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Single.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void IntTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.INT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Int32.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Int32.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void LongTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.LONG_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Int64.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Int64.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void SByteTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.SBYTE_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(SByte.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(SByte.MinValue, Is.EqualTo(vals[1].Instance));
        }


        [Test]
        public void ShortTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.SHORT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Int16.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Int16.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void UIntTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.UINT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(UInt32.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(UInt32.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void ULongTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.ULONG_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(UInt64.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(UInt64.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void UShortTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.USHORT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(UInt16.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(UInt16.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void StringTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.STRING_TYPE);
            Assert.That(vals.Count == 4);
            Assert.That("", Is.EqualTo(vals[0].Instance));
            Assert.That(@" ", Is.EqualTo(vals[1].Instance));
            Assert.That("TEST_STRING", Is.EqualTo(vals[2].Instance));
            Assert.That("1234567890", Is.EqualTo(vals[3].Instance));

        }

        [Test]
        public void ObjectTest()
        {
            List<ObjectInstance> vals = m_TestValueCalc.GetTestValues(TestValueCalculator.OBJECT_TYPE);
            Assert.That(vals.Count == 11);
            Assert.That("", Is.EqualTo(vals[0].Instance));
            Assert.That(@" ", Is.EqualTo(vals[1].Instance));
            Assert.That("TEST_STRING", Is.EqualTo(vals[2].Instance));
            Assert.That("1234567890", Is.EqualTo(vals[3].Instance));
            Assert.That(Decimal.MaxValue, Is.EqualTo(vals[4].Instance));
            Assert.That(Decimal.MinValue, Is.EqualTo(vals[5].Instance));
            Assert.That(Decimal.MinusOne, Is.EqualTo(vals[6].Instance));
            Assert.That(Decimal.Zero, Is.EqualTo(vals[7].Instance));
            Assert.That(Decimal.One, Is.EqualTo(vals[8].Instance));
            Assert.That(vals[9].Instance, Is.True);
            Assert.That(vals[10].Instance, Is.False);

        }


    }
}
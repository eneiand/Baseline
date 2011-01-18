using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baseline.TypeAnalysis;
using Baseline.TypeAnalysis.ObjectInstantiation;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    class DefaultTestValueCalculatorTests
    {
        private readonly TestValueCalculator m_TestValueCalc = new DefaultTestValueCalculator();

        [Test]
        public void CharTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.CHAR_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Char.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Char.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void FloatTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.FLOAT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Single.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Single.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void DoubleTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.DOUBLE_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Double.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Double.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void DecimalTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.DECIMAL_TYPE);
            Assert.That(vals.Count == 5);
            Assert.That(Decimal.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Decimal.MinValue, Is.EqualTo(vals[1].Instance));
            Assert.That(Decimal.MinusOne, Is.EqualTo(vals[2].Instance));
            Assert.That(Decimal.Zero, Is.EqualTo(vals[3].Instance));
            Assert.That(Decimal.One, Is.EqualTo(vals[4].Instance));
        }

        [Test]
        public void BoolTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.BOOL_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(vals[0].Instance, Is.True);
            Assert.That(vals[1].Instance, Is.False);
        }

        [Test]
        public void ULongTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.ULONG_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(UInt64.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(UInt64.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void LongTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.LONG_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Int64.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Int64.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void IntTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.INT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Int32.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Int32.MinValue, Is.EqualTo(vals[1].Instance));
        }
        [Test]
        public void UIntTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.UINT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(UInt32.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(UInt32.MinValue, Is.EqualTo(vals[1].Instance));
        }


        [Test]
        public void ShortTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.SHORT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Int16.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Int16.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void UShortTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.USHORT_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(UInt16.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(UInt16.MinValue, Is.EqualTo(vals[1].Instance));
        }


        [Test]
        public void ByteTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.BYTE_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(Byte.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(Byte.MinValue, Is.EqualTo(vals[1].Instance));
        }

        [Test]
        public void SByteTest()
        {
            var vals = m_TestValueCalc.GetTestValues(TestValueCalculator.SBYTE_TYPE);
            Assert.That(vals.Count == 2);
            Assert.That(SByte.MaxValue, Is.EqualTo(vals[0].Instance));
            Assert.That(SByte.MinValue, Is.EqualTo(vals[1].Instance));
        }
    }
}

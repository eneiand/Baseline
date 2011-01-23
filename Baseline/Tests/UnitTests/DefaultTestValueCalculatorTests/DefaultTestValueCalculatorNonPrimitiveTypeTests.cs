using System;
using Baseline.TypeAnalysis;
using Baseline.TypeAnalysis.ObjectInstantiation;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.UnitTests.DefaultTestValueCalculatorTests
{
    [TestFixture]
    class DefaultTestValueCalculatorNonPrimitiveTypeTests
    {
        private readonly TestValueCalculator m_TestValueCalc = new DefaultTestValueCalculator();

        [Test]
        public void TypeWithDefaultConstructorTest()
        {
            Type d = typeof(DefaultConstructorTestType);
            var testVals = m_TestValueCalc.GetTestValues(d);
            Assert.That(testVals.Count, Is.EqualTo(1));
            Assert.That(testVals[0].Instance.GetType() == d);

        }
        [Test]
        public void TypeWithDefaultConstructorThrowsExceptionTest()
        {
            Type d = typeof(DefaultConstructorThrowsAnExceptionTestType);
            var testVals = m_TestValueCalc.GetTestValues(d);
            Assert.That(testVals.Count, Is.EqualTo(0));

        }
        [Test]
        public void TypeWithPrimitiveTypeConstructorTest()
        {
            Type t = typeof(PrimitiveTypeConstructorTestType);
            var testVals = m_TestValueCalc.GetTestValues(t);
            Assert.That(testVals.Count == 2);
            Assert.That((testVals[0].Instance as PrimitiveTypeConstructorTestType).I == Int32.MaxValue);
            Assert.That((Int32)(testVals[1].Instance as PrimitiveTypeConstructorTestType).I == Int32.MinValue);

            
        }

        [Test]
        public void TypeWithMultiplePrimitiveTypeConstructorTest()
        {
            Type t = typeof(MultiplePrimitiveTypeConstructorTestType);
            var testVals = m_TestValueCalc.GetTestValues(t);
            Assert.That(testVals.Count == 4);


        }

        [Test]
        public void TypeWithPrimitiveTypeConstructorThrowsConstructorTest()
        {
            Type t = typeof(PrimitiveTypeConstructorThrowsAnExceptionTestType);
            var testVals = m_TestValueCalc.GetTestValues(t);
            Assert.That(testVals.Count == 0);
        }
    }
}

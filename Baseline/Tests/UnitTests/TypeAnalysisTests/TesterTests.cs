using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baseline.TypeAnalysis;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.UnitTests.TypeAnalysisTests
{
    [TestFixture]
    class TesterTests
    {
        private Tester m_Tester;

        [SetUp]
        public void Init()
        {
            m_Tester = new Tester();
        }

        [Test]
        public void ConstructorTest()
        {

            Assert.Throws<ArgumentNullException>(() =>
                                                     {
                                                         Tester t = new Tester(null);
                                                     });

            Assert.DoesNotThrow(() =>
                                    {
                                        Tester t = new Tester(new DefaultTestValueCalculator());
                                    });
        }

        [Test]
        public void GenerateTestsForPrimitiveTypesTest()
        {
            Type intType = typeof (Int32);
            var testSuite = m_Tester.GenerateTests(intType);


        }

        [Test]
        public void GenerateTestsForNonPrimitiveTypeTest()
        {
            Type mockType = typeof(MockType);
            var testSuite = m_Tester.GenerateTests(mockType);

            foreach (var constructorInfo in mockType.GetConstructors())
            {
                Assert.That(testSuite.Tests.FindAll(t => constructorInfo.Name == t.Method.Name).Count >= 1);
            }

            foreach (var methodInfo in mockType.GetMethods())
            {
                Assert.That(testSuite.Tests.FindAll(t => methodInfo.Name == t.Method.Name).Count >= 1);
            }


        }
    }
}

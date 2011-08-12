using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Baseline.CodeGeneration;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TypeAnalysis;
using NUnit.Framework;
using TestAssembly.Mocks;

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
        public void GenerateTestsThrowsArgumentNullExceptionTest()
        {
            Type t = null;
            Assert.Throws<ArgumentNullException>(() => m_Tester.GenerateTests(t));
        }

        [Test]
        public void ConstructorTest()
        {

            Assert.Throws<ArgumentNullException>(() =>
                                                     {
                                                         Tester t = new Tester(null, null);
                                                     });

            Assert.DoesNotThrow(() =>
                                    {
                                        Tester t = new Tester();
                                    });
        }

        [Test, RequiresMTA]
        public void GenerateTestsForPrimitiveTypesTest()
        {
            Type intType = typeof (Int32);
            var testSuite = m_Tester.GenerateTests(intType);
            var exceptionthrowers = testSuite.Tests.FindAll(t => t.Name == "CompareToThrowsExceptionTest");


            Console.WriteLine(new TestSuiteGenerator(testSuite, new NUnitUnitTestCodeWriter()).TransformText());
        }

        [Test, RequiresMTA]
        public void GenerateTestsForNonPrimitiveTypeTest()
        {
            Type mockType = typeof(MockType);
            var testSuite = m_Tester.GenerateTests(mockType);

            //foreach (var constructorInfo in mockType.GetConstructors())
            //{
            //    Assert.That(testSuite.Tests.FindAll(t => constructorInfo.Name == t.Method.Name).Count >= 1);
            //}

            //foreach (var methodInfo in mockType.GetMethods())
            //{
            //    Assert.That(testSuite.Tests.FindAll(t => methodInfo.Name == t.Method.Name).Count >= 1);
            //}

            Console.WriteLine(new TestSuiteGenerator(testSuite, new NUnitUnitTestCodeWriter()).TransformText());

        }

        [Test]
        public void GenerateTestsAssembly()
        {
            var mockType = typeof (MockType);
            Assembly testAssembly = Assembly.GetAssembly(mockType);
            Console.WriteLine("Testing Assembly : " + testAssembly.FullName);

            var testSuites = m_Tester.GenerateTests(testAssembly);

            foreach (var type in testAssembly.GetTypes())
            {
                Assert.That(testSuites.Find(t => t.Type == type) != null);
            }

            
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baseline.CodeGeneration;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;
using NUnit.Framework;

namespace Tests.UnitTests.CodeGenerationTests
{
    public class DummyType
    {
        public DummyType()
        {}

        public void Method1()
        {
            
        }
    }

    [TestFixture]
    class NUnitTestSuiteGenerationTests
    {
        private TestSuiteGenerator m_TestSuiteGenerator;
        private TestSuite m_TestSuite;
        private List<UnitTest> m_Tests = new List<UnitTest>();
        private Type m_DummyType = typeof(DummyType);

        [SetUp]
        public void Init()
        {
            m_Tests.Add(new MethodTest(TimeSpan.FromMilliseconds(10), m_DummyType.GetMethod("Method1"), instance: new ObjectInstance(new DummyType(), new ObjectCreationData(m_DummyType.GetConstructor(Type.EmptyTypes)))));

            m_TestSuite = new TestSuite(m_DummyType, m_Tests);
            m_TestSuiteGenerator = new TestSuiteGenerator(m_TestSuite, new NUnitUnitTestCodeWriter(), "[TestFixture]");
    
        }

        [Test]
        public void ExceptionThrowingTestGenerationTest()
        {
            Console.WriteLine(m_TestSuiteGenerator.TransformText());
            Assert.That(m_TestSuiteGenerator.TransformText(), Is.EqualTo(String.Empty));
        }

    }
}

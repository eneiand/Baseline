using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Addin.CodeGeneration;
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

        public String Method2()
        {
            return "test";
        }

        public String Prop1
        {
            get { return "test"; }
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
            m_Tests.Add(new MethodTest(TimeSpan.FromMilliseconds(10), m_DummyType.GetMethod("Method2"), result:"test", instance: new ObjectInstance(new DummyType(), new ObjectCreationData(m_DummyType.GetConstructor(Type.EmptyTypes)))));
            m_Tests.Add(new ExceptionThrowingTest(TimeSpan.FromMilliseconds(10), m_DummyType.GetMethod("Method1"), new InvalidOperationException(),
                new ObjectInstance(new DummyType(), new ObjectCreationData(m_DummyType.GetConstructor(Type.EmptyTypes)))));
            
            m_Tests.Add(new ConstructorTest(TimeSpan.FromMilliseconds(10), m_DummyType.GetConstructor(Type.EmptyTypes),
                                            result:new DummyType()));

            m_TestSuite = new TestSuite(m_DummyType, m_Tests);
            m_TestSuiteGenerator = new TestSuiteGenerator(m_TestSuite, new NUnitUnitTestCodeWriter());
    
        }

        [Test]
        public void ExceptionThrowingTestGenerationTest()
        {
            Console.WriteLine(m_TestSuiteGenerator.TransformText());
            Assert.That(m_TestSuiteGenerator.TransformText(), Is.EqualTo(String.Empty));
        }

    }
}

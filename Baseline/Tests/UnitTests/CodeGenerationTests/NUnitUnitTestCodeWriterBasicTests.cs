using System;
using System.Reflection;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;
using Microsoft.CSharp;
using NUnit.Framework;

namespace Tests.UnitTests.CodeGenerationTests
{
    public class BasicTestType
    {
        public void ExceptionThrower()
        {
            throw new ArgumentOutOfRangeException();
        }

        public String Property1 { get { return "TEST_STRING"; } }

        public int Method1()
        {
            return 5;
        }
    }

    [TestFixture]
    class NUnitUnitTestCodeWriterBasicTests
    {
        private ExceptionThrowingTest m_ExceptionThrowingTest;
        private ConstructorTest m_ConstructorTest;
        private MethodTest m_MethodTest;
        private Type m_TestType;
        private ObjectInstance m_TestTypeInstance;
        private ConstructorInfo m_TestTypeDefaultConstructor;
        private NUnitUnitTestCodeWriter m_TestCodeWriter;

        [SetUp]
        public void Setup()
        {
            m_TestCodeWriter = new NUnitUnitTestCodeWriter();
            m_TestType = typeof(BasicTestType);
            m_TestTypeDefaultConstructor = m_TestType.GetConstructor(Type.EmptyTypes);
            m_TestTypeInstance = new ObjectInstance(new BasicTestType(), new ObjectCreationData(m_TestTypeDefaultConstructor));
            m_ExceptionThrowingTest = new ExceptionThrowingTest(TimeSpan.FromMilliseconds(1), m_TestType.GetMethod("ExceptionThrower"), new ArgumentOutOfRangeException(), m_TestTypeInstance);
            
            m_ConstructorTest = new ConstructorTest(TimeSpan.FromMilliseconds(1), m_TestTypeDefaultConstructor, new BasicTestType());

            m_MethodTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_TestType.GetMethod("Method1"), 5, instance:m_TestTypeInstance);
        }

        [Test]
        public void ExceptionThrowingTestTest()
        {
            var code = m_TestCodeWriter.GetCode(m_ExceptionThrowingTest);

            Assert.That(code,
                        Is.EqualTo(
                            @"[Test]
public void ExceptionThrowerThrowsExceptionTest()
{
    Assert.Throws<ArgumentOutOfRangeException>(()=>
    {
        var instance = new BasicTestType();
        instance.ExceptionThrower();
    });
}
"));
        }

        [Test]
        public void ConstructorTestTest()
        {
            var code = m_TestCodeWriter.GetCode(m_ConstructorTest);
            Assert.That(code, Is.EqualTo(
                @"[Test]
public void DefaultConstructorTest()
{
    var instance = new BasicTestType();
    Assert.That(instance, Is.Not.Null);
    Assert.That(instance.Property1, Is.EqualTo(""TEST_STRING""));
}
"));
        }

        [Test]
        public void MethodTestTest()
        {
            var code = m_TestCodeWriter.GetCode(m_MethodTest);
            Assert.That(code, Is.EqualTo(
                @"[Test]
public void Method1Test()
{
    var instance = new BasicTestType();
    var result = instance.Method1();
    Assert.That(instance.Property1, Is.EqualTo(""TEST_STRING""));
    Assert.That(result, Is.EqualTo(5));
}
"));
        }

    }
}
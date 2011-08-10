using System;
using System.Reflection;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.UnitTests.CodeGenerationTests
{
    public class ConstructorTestsType
    {
        //Constructor with simple parameters 
        public ConstructorTestsType(int x, int y)
        {
        }

        //Constructor with complex parameters
        public ConstructorTestsType(OtherTestClass z)
        {
        }

        
    }

    public class OtherTestClass
    {

        public OtherTestClass()
        {}

        public OtherTestClass(int x, int y)
        { }
    }

    [TestFixture]
    public class CodeWriterConstructorTests
    {

        private ConstructorTest m_IntsConstructorTest;
        private ConstructorTest m_ComplexConstructorTest;
        private ConstructorTest m_MoreComplexConstructorTest;

        private Type m_TestType;
        private ObjectInstance m_TestTypeInstance, m_TestTypeComplexInstance;
        private ObjectInstance m_OtherTestClassInstance;
        private ConstructorInfo m_TestTypeIntsConstructor;
        private ConstructorInfo m_TestTypeComplexConstructor;
        private NUnitUnitTestCodeWriter m_TestCodeWriter;

        [SetUp]
        public void Setup()
        {
            m_OtherTestClassInstance = new ObjectInstance(new OtherTestClass(), new ObjectCreationData(typeof(OtherTestClass).GetConstructor(Type.EmptyTypes)));
            m_TestCodeWriter = new NUnitUnitTestCodeWriter();
            m_TestType = typeof(ConstructorTestsType);
            m_TestTypeIntsConstructor = m_TestType.GetConstructor(new[] {typeof(int), typeof(int)});

            m_TestTypeComplexConstructor = m_TestType.GetConstructor(new[] {typeof(OtherTestClass)});

            List<ObjectInstance> intArgs = new List<ObjectInstance>(){new ObjectInstance(1), new ObjectInstance(2)};
            m_TestTypeInstance = new ObjectInstance(new ConstructorTestsType(1, 2),
                                                    new ObjectCreationData(m_TestTypeIntsConstructor, intArgs));

            m_TestTypeComplexInstance = new ObjectInstance(new ConstructorTestsType(new OtherTestClass()), new ObjectCreationData(m_TestTypeComplexConstructor, new []{m_OtherTestClassInstance}));

            List<ObjectInstance> complexArgs = new List<ObjectInstance> { new ObjectInstance(new OtherTestClass(), new ObjectCreationData(typeof(OtherTestClass).GetConstructor(Type.EmptyTypes))) };

            m_IntsConstructorTest = new ConstructorTest(TimeSpan.FromMilliseconds(1), m_TestTypeIntsConstructor, m_TestTypeInstance.Instance, intArgs);

            m_ComplexConstructorTest = new ConstructorTest(TimeSpan.FromMilliseconds(2), m_TestTypeComplexConstructor, m_TestTypeComplexInstance.Instance, complexArgs);

            List<ObjectInstance> moreComplexArgs = new List<ObjectInstance> { new ObjectInstance(new OtherTestClass(), new ObjectCreationData(typeof(OtherTestClass).GetConstructor(new []{typeof(Int32), typeof(Int32)}), intArgs)) };
            m_MoreComplexConstructorTest = new ConstructorTest(TimeSpan.FromMilliseconds(1), m_TestTypeComplexConstructor, m_TestTypeComplexInstance.Instance, moreComplexArgs);
        }

        [Test]
        public void IntsConstructorTest()
        {
            var code = m_TestCodeWriter.GetCode(m_IntsConstructorTest);
            Assert.That(code, Is.EqualTo(
@"[Test]
public void Int32Int32ConstructorTest()
{
    var instance = new ConstructorTestsType(1, 2);
    Assert.That(instance, Is.Not.Null);
}
"));
        }

        [Test]
        public void ComplexConstructorTest()
        {
            var code = m_TestCodeWriter.GetCode(m_ComplexConstructorTest);
            Assert.That(code, Is.EqualTo(
@"[Test]
public void OtherTestClassConstructorTest()
{
    var instance = new ConstructorTestsType(new OtherTestClass());
    Assert.That(instance, Is.Not.Null);
}
"                ));
        }

        [Test]
        public void MoreComplexConstructorTest()
        {
            var code = m_TestCodeWriter.GetCode(m_MoreComplexConstructorTest);
            Assert.That(code, Is.EqualTo(
@"[Test]
public void OtherTestClassConstructorTest()
{
    var instance = new ConstructorTestsType(new OtherTestClass(1, 2));
    Assert.That(instance, Is.Not.Null);
}
"               ));
        }
    }
}

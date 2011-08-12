using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NUnit.Framework;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Tests.UnitTests.CodeGenerationTests
{
    public class MethodTestsType
    {
        public void SimpleMethod()
        {
        }

        public int TestProperty { get { return 3; } }

        public int IntReturningMethod()
        {
            return 3;
        }

        public void MethodTakingAnInt(int a)
        {
        }

        public void MethodTakingAComplexType(OtherType a)
        {
        }

        public void MethodCalledWithNull(OtherType a)
        {
        }

        public OtherType MethodReturningAComplexType()
        {
            return new OtherType();
        }

        public OtherType MethodReturningNull()
        {
            return null;
        }

    }

    public class OtherType
    {
        public int TestProperty { get { return 3; } }
    }

    [TestFixture]
    class NUnitUnitTestCodeWriterMethodTests
    {
        private MethodTest m_SimpleMethodTest;
        private MethodTest m_IntReturningMethodTest;
        private MethodTest m_MethodTakingAnIntTest;
        private MethodTest m_MethodTakingAComplexTypeTest;
        private MethodTest m_MethodReturningAComplexTypeTest;
        private MethodTest m_MethodReturningNullTest;
        private MethodTest m_MethodCalledWithNullTest;


        private MethodInfo m_SimpleMethod;
        private MethodInfo m_IntReturningMethod;
        private MethodInfo m_MethodTakingAnInt;
        private MethodInfo m_MethodTakingAComplexType;
        private MethodInfo m_MethodReturningAComplexType;
        private MethodInfo m_MethodReturningNull;
        private MethodInfo m_MethodCalledWithNull;


        private MethodTestsType m_TargetInstance = new MethodTestsType();
        private ObjectInstance m_TargetInstanceMeta;
        private OtherType m_OtherType = new OtherType();
        private ObjectInstance m_OtherTypeMeta;
        private ObjectInstance m_IntMeta;
        private Type m_TypeUnderTest;

        private NUnitUnitTestCodeWriter m_TestCodeWriter;

        String m_Code = String.Empty;

        [SetUp]
        public void Setup()
        {
             m_TestCodeWriter = new NUnitUnitTestCodeWriter();
            m_TypeUnderTest = typeof(MethodTestsType);
            m_SimpleMethod = m_TypeUnderTest.GetMethod("SimpleMethod");
            m_IntReturningMethod = m_TypeUnderTest.GetMethod("IntReturningMethod");
            m_MethodTakingAnInt = m_TypeUnderTest.GetMethod("MethodTakingAnInt");
            m_MethodTakingAComplexType = m_TypeUnderTest.GetMethod("MethodTakingAComplexType");
            m_MethodReturningAComplexType = m_TypeUnderTest.GetMethod("MethodReturningAComplexType");
            m_MethodReturningNull = m_TypeUnderTest.GetMethod("MethodReturningNull");
            m_MethodCalledWithNull = m_TypeUnderTest.GetMethod("MethodCalledWithNull");

            m_TargetInstanceMeta = new ObjectInstance(m_TargetInstance, new ObjectCreationData(m_TypeUnderTest.GetConstructor(Type.EmptyTypes)));
            m_OtherTypeMeta = new ObjectInstance(m_OtherType, new ObjectCreationData(typeof(OtherType).GetConstructor(Type.EmptyTypes)));
            m_IntMeta = new ObjectInstance(1);
            
            m_SimpleMethodTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_SimpleMethod, instance: m_TargetInstanceMeta);

            m_IntReturningMethodTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_IntReturningMethod, 3, instance: m_TargetInstanceMeta);
            m_MethodTakingAnIntTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodTakingAnInt, null, new[]{m_IntMeta}, m_TargetInstanceMeta);

            m_MethodTakingAComplexTypeTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodTakingAComplexType, null, new[] { m_OtherTypeMeta }, m_TargetInstanceMeta);
            m_MethodReturningAComplexTypeTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodReturningAComplexType, m_OtherType, instance: m_TargetInstanceMeta);
        
            m_MethodReturningNullTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodReturningNull, null, instance:m_TargetInstanceMeta);
        }

        [TearDown]
        public void Cleanup()
        {
            Console.WriteLine(m_Code);
        }

        [Test]
        public void SimpleMethodTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_SimpleMethodTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void SimpleMethodTest()
{
    var instance = new MethodTestsType();
    instance.SimpleMethod();
    Assert.That(instance.TestProperty, Is.EqualTo(3));
}
"                ));
        }

        [Test]
        public void IntReturningMethodTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_IntReturningMethodTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void IntReturningMethodTest()
{
    var instance = new MethodTestsType();
    var result = instance.IntReturningMethod();
    Assert.That(instance.TestProperty, Is.EqualTo(3));
    Assert.That(result, Is.EqualTo(3));
}
"));
        }

        [Test]
        public void MethodTakingAnIntTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_MethodTakingAnIntTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void MethodTakingAnIntTest()
{
    var instance = new MethodTestsType();
    instance.MethodTakingAnInt(1);
    Assert.That(instance.TestProperty, Is.EqualTo(3));
}
"));
        }

        [Test]
        public void MethodTakingAComplexTypeTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_MethodTakingAComplexTypeTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void MethodTakingAComplexTypeTest()
{
    var instance = new MethodTestsType();
    instance.MethodTakingAComplexType(new OtherType());
    Assert.That(instance.TestProperty, Is.EqualTo(3));
}
"                ));
        }

        [Test]
        public void MethodReturningAComplexTypeTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_MethodReturningAComplexTypeTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void MethodReturningAComplexTypeTest()
{
    var instance = new MethodTestsType();
    var result = instance.MethodReturningAComplexType();
    Assert.That(instance.TestProperty, Is.EqualTo(3));
    Assert.That(result, Is.Not.Null);
    Assert.That(result.TestProperty, Is.EqualTo(3));
}
"                ));
        }

        [Test]
        public void MethodReturningNullTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_MethodReturningNullTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void MethodReturningNullTest()
{
    var instance = new MethodTestsType();
    var result = instance.MethodReturningNull();
    Assert.That(instance.TestProperty, Is.EqualTo(3));
    Assert.That(result, Is.Null);
}
"));
        }
    }
}

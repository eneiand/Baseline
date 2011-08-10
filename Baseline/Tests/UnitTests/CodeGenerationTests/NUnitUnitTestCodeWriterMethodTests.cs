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

        public OtherType MethodReturningAComplexType()
        {
            return new OtherType();
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

        private MethodInfo m_SimpleMethod;
        private MethodInfo m_IntReturningMethod;
        private MethodInfo m_MethodTakingAnInt;
        private MethodInfo m_MethodTakingAComplexType;
        private MethodInfo m_MethodReturningAComplexType;

        private MethodTestsType m_TargetInstance = new MethodTestsType();
        private ObjectInstance m_TargetInstanceMeta;
        private OtherType m_OtherType = new OtherType();
        private ObjectInstance m_OtherTypeMeta;
        private ObjectInstance m_IntMeta;
        private Type m_TypeUnderTest;

        private NUnitUnitTestCodeWriter m_TestCodeWriter;

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

            m_TargetInstanceMeta = new ObjectInstance(m_TargetInstance, new ObjectCreationData(m_TypeUnderTest.GetConstructor(Type.EmptyTypes)));
            m_OtherTypeMeta = new ObjectInstance(m_OtherType, new ObjectCreationData(typeof(OtherType).GetConstructor(Type.EmptyTypes)));
            m_IntMeta = new ObjectInstance(1);
            
            m_SimpleMethodTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_SimpleMethod, instance: m_TargetInstanceMeta);

            m_IntReturningMethodTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_IntReturningMethod, 3, instance: m_TargetInstanceMeta);
            m_MethodTakingAnIntTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodTakingAnInt, null, new[]{m_IntMeta}, m_TargetInstanceMeta);

            m_MethodTakingAComplexTypeTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodTakingAComplexType, null, new[] { m_OtherTypeMeta }, m_TargetInstanceMeta);
            m_MethodReturningAComplexTypeTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_MethodReturningAComplexType, m_OtherType, instance: m_TargetInstanceMeta);
        }

        [Test]
        public void SimpleMethodTest()
        {
            var code = m_TestCodeWriter.GetCode(m_SimpleMethodTest);
            Assert.That(code, Is.EqualTo(
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
            var code = m_TestCodeWriter.GetCode(m_IntReturningMethodTest);
            Assert.That(code, Is.EqualTo(
@"[Test]
public void IntReturningMethodTest()
{
    var instance = new MethodTestsType();
    var result = instance.IntReturningMethod();
    Assert.That(result, Is.EqualTo(3));
    Assert.That(instance.TestProperty, Is.EqualTo(3));
}
"                ));
        }

        [Test]
        public void MethodTakingAnIntTest()
        {
            var code = m_TestCodeWriter.GetCode(m_MethodTakingAnIntTest);
            Assert.That(code, Is.EqualTo(
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
            var code = m_TestCodeWriter.GetCode(m_MethodTakingAComplexTypeTest);
            Assert.That(code, Is.EqualTo(
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
            var code = m_TestCodeWriter.GetCode(m_MethodReturningAComplexTypeTest);
            Assert.That(code, Is.EqualTo(
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;
using NUnit.Framework;

namespace Tests.UnitTests.CodeGenerationTests
{
    public class PropertiesTestType
    {
        public OtherPropertiesType MPublicField1;

        public OtherPropertiesType MPublicField2 = new OtherPropertiesType();

        public OtherPropertiesType Property1 { get; set; }

        public OtherPropertiesType Property2 {get{return new OtherPropertiesType();}}

    }

    public class OtherPropertiesType
    {
        public String Property1 { get { return String.Empty; } }
    }

    [TestFixture]
    class NUnitUnitTestCodeWriterPropertiesTests
    {
        private ConstructorTest m_ConstructorTest;
        private Type m_TypeUnderTest = typeof(PropertiesTestType);
        private ConstructorInfo m_DefaultConstructor;
        private ObjectInstance m_PropInstanceMeta;
        private PropertiesTestType m_PropInstance;
        private String m_Code = String.Empty;
        private NUnitUnitTestCodeWriter m_TestCodeWriter = new NUnitUnitTestCodeWriter();

        [SetUp]
        public void Init()
        {
            m_PropInstance = new PropertiesTestType();
            m_DefaultConstructor = m_TypeUnderTest.GetConstructor(Type.EmptyTypes);
            m_PropInstanceMeta  = new ObjectInstance(m_PropInstance, new ObjectCreationData(m_DefaultConstructor));
            m_ConstructorTest = new ConstructorTest(TimeSpan.FromMilliseconds(1), m_DefaultConstructor, m_PropInstance);
        }
        [TearDown]
        public void Cleanup()
        {
            Console.WriteLine(m_Code);
        }

        [Test]
        public void PropertiesTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_ConstructorTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void DefaultConstructorTest()
{
    var instance = new PropertiesTestType();
    Assert.That(instance, Is.Not.Null);
    Assert.That(instance.MPublicField1, Is.Null);
    Assert.That(instance.MPublicField2, Is.Not.Null);
    Assert.That(instance.MPublicField2.Property1, Is.EqualTo(""""));
    Assert.That(instance.Property1, Is.Null);
    Assert.That(instance.Property2, Is.Not.Null);
    Assert.That(instance.Property2.Property1, Is.EqualTo(""""));
}
"
));
        }

    }
}

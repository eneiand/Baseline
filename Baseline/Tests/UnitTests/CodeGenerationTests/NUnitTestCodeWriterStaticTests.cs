using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness.UnitTests;
using NUnit.Framework;

namespace Tests.UnitTests.CodeGenerationTests
{
    public class StaticTestClass
    {
        public static int ReturnAnInt()
        {
            return 3;
        }
    }

    [TestFixture]
    class NUnitTestCodeWriterStaticTests
    {

        private MethodTest m_StaticReturnAnIntTest;
        private MethodInfo m_ReturnAnIntMethod;
        private Type m_TypeUnderTest = typeof(StaticTestClass);
        private String m_Code = String.Empty;
        private NUnitUnitTestCodeWriter m_TestCodeWriter = new NUnitUnitTestCodeWriter();

        [SetUp]
        public void Setup()
        {
            m_ReturnAnIntMethod = m_TypeUnderTest.GetMethod("ReturnAnInt");
            m_StaticReturnAnIntTest = new MethodTest(TimeSpan.FromMilliseconds(1), m_ReturnAnIntMethod, 3);
        }

        [Test]
        public void StaticReturnAnIntTest()
        {
            m_Code = m_TestCodeWriter.GetCode(m_StaticReturnAnIntTest);
            Assert.That(m_Code, Is.EqualTo(
@"[Test]
public void ReturnAnIntTest()
{
    var result = StaticTestClass.ReturnAnInt();
    Assert.That(result, Is.EqualTo(3));
}
"                ));
        }

    }
}

using System;
using System.Collections.Generic;
using Baseline.TestHarness.UnitTests;
using NUnit.Framework;
using Baseline.TypeAnalysis.ObjectInstantiation;
using System.Reflection;

namespace Tests.UnitTests.TestHarnessTests
{
     [TestFixture]
    class MethodTestTests
    {
         private ObjectInstance m_StringInstance;
         private String m_Str;
         private Type m_StrType = typeof(String);
         private MethodInfo m_CopyTo;

         [SetUp]
         public void Init()
         {
            
             m_Str = new String('C', 5);
             
             m_StringInstance = new ObjectInstance(m_Str);
             m_CopyTo = m_StrType.GetMethod("CopyTo", new Type[] { typeof(int), typeof(char[]), typeof(int), typeof(int) });
         }

         [Test]
         public void NullMethodTest()
         {
             Assert.Throws<ArgumentNullException>(() =>
             {
                 new MethodTest(new TimeSpan(), null);
             });
         }

         [Test]
         public void ArgumentsMissingTest()
         {
             Assert.Throws<ArgumentException>(() =>
             {
                 new MethodTest(new TimeSpan(), typeof(String).GetMethod("IndexOf", new Type[]{typeof(char)}));
             }
                 );
         }

         [Test]
         public void ArgumentTypesDontMatchTest()
         {
             Assert.Throws<ArgumentException>(() =>
             {
                 new MethodTest(new TimeSpan(), typeof(String).GetMethod("IndexOf", new Type[] { typeof(char) }), null, 
                     new List<ObjectInstance>(){new ObjectInstance(5)});
             }
                 );
         }

         [Test]
         public void ReturnTypesDontMatchTest()
         {
             Assert.Throws<ArgumentException>(() =>
             {
                 new MethodTest(new TimeSpan(), typeof(String).GetMethod("IndexOf", new Type[] { typeof(char) }), "TEST_STRING",
                     new List<ObjectInstance>() { new ObjectInstance('C') }, m_StringInstance);
             }
                 );
         }

         private class TestType
         {
             public TestType()
             {
             }
             public void TestMethod()
             {
             }
         }

         [Test]
         public void VoidReturnTypeDoesntMatchCharTest()
         {
             var t = new TestType();
             t.TestMethod();

             Assert.Throws<ArgumentException>(() =>
             {
                 new MethodTest(new TimeSpan(), typeof(TestType).GetMethod("TestMethod"), "TEST_STRING", 
                     null,
                    new ObjectInstance(t, new ObjectCreationData(typeof(TestType).GetConstructor(Type.EmptyTypes))));
             }
                 );
         }

         [Test]
         public void IntReturnTypeDoesntMatchNullTest()
         {
             Assert.Throws<ArgumentNullException>(() =>
             {
                 new MethodTest(new TimeSpan(), typeof(String).GetMethod("IndexOf", new Type[] { typeof(char) }), null,
                     new List<ObjectInstance>() { new ObjectInstance('C') }, m_StringInstance);
             }
                 );
         }

         [Test]
         public void InstanceMethodTest()
         {
              var args = new List<ObjectInstance>();
              var method = typeof(String).GetMethod("Normalize", Type.EmptyTypes);
              var mTest = new MethodTest(new TimeSpan(), method, "TEST_STRING", args, m_StringInstance);

              Assert.That(mTest.Arguments, Is.EqualTo(args));
              Assert.That(mTest.Result, Is.EqualTo("TEST_STRING"));
              Assert.That(mTest.Method, Is.SameAs(method));
              Assert.That(mTest.Instance, Is.SameAs(m_StringInstance));
         }

         [Test]
         public void StaticMethodTest()
         {
             var args = new List<ObjectInstance>() { m_StringInstance, m_StringInstance };
             var method = typeof(String).GetMethod("Compare", new Type[] { m_StrType, m_StrType });
             var mTest = new MethodTest(new TimeSpan(), method, 1, args);

             Assert.That(mTest.Arguments, Is.EqualTo(args));
             Assert.That(mTest.Result, Is.EqualTo(1));
             Assert.That(mTest.Method, Is.SameAs(method));
             Assert.That(mTest.Instance, Is.Null);
         }

         [Test]
         public void StaticMethodCantHaveAnInstance()
         {
             var args = new List<ObjectInstance>() { m_StringInstance, m_StringInstance};
             var method = typeof(String).GetMethod("Compare", new Type[]{m_StrType, m_StrType});

             Assert.Throws<ArgumentException>(() =>
                 {
                     new MethodTest(new TimeSpan(), method, 1, args, m_StringInstance);
                 });
         }

         [Test]
         public void InstanceMethodMustHaveAnInstance()
         {
             var args = new List<ObjectInstance>();
             var method = typeof(String).GetMethod("Normalize", Type.EmptyTypes);

             Assert.Throws<ArgumentNullException>(() =>
             {
                 new MethodTest(new TimeSpan(), method, "TEST_STRING", args);
             });
         }

         [Test]
         public void InstanceTypeMustMatchInstanceMethodType()
         {
             var args = new List<ObjectInstance>();
             var method = typeof(String).GetMethod("Normalize", Type.EmptyTypes);

             Assert.Throws<ArgumentException>(() =>
             {
                 new MethodTest(new TimeSpan(), method, "TEST_STRING", args,new ObjectInstance(34));
             });
         }
 
    }
}

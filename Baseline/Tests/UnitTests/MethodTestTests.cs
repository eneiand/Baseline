using System;
using System.Collections.Generic;
using Baseline.ObjectCreation;
using Baseline.TestHarness.UnitTests;
using NUnit.Framework;

namespace Tests.UnitTests
{
     [TestFixture]
    class MethodTestTests
    {
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
                     new List<ObjectInstance>() { new ObjectInstance('C') });
             }
                 );
         }

         [Test]
         public void VoidReturnTypeDoesntMatchStringTest()
         {
             Assert.Throws<ArgumentException>(() =>
             {
                 new MethodTest(new TimeSpan(), typeof(List<String>).GetMethod("Reverse", Type.EmptyTypes), "TEST_STRING");
             }
                 );
         }

         //[Test]
         //public void StringReturnTypeDoesntMatchNullTest()
         //{
         //    Assert.Throws<ArgumentNullException>(() =>
         //    {
         //        new MethodTest(new TimeSpan(), typeof(String).GetMethod("Reverse", Type.EmptyTypes));
         //    }
         //        );
         //}

         //[Test]
         //public void StringTest()
         //{
         //    Assert.Throws<ArgumentNullException>(() =>
         //    {
         //        new MethodTest(new TimeSpan(), typeof(String).GetMethod("Reverse", Type.EmptyTypes), "TEST_STRING");
         //    }
         //        );
         //}
 
    }
}

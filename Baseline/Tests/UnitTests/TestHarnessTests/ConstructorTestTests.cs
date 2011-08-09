using System;
using System.Collections.Generic;
using Baseline.TestHarness.UnitTests;
using NUnit.Framework;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Tests.UnitTests.TestHarnessTests
{
    [TestFixture]
    class ConstructorTestTests
    {
        [Test]
        public void NullCreatedObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ConstructorTest(new TimeSpan(), this.GetType().GetConstructor(Type.EmptyTypes), null);
            }
            );
        }

        [Test]
        public void ConstructorAndObjectDoNotMatchTest()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new ConstructorTest(new TimeSpan(), typeof(List<String>).GetConstructor(Type.EmptyTypes), 30);
            }

                );
        }

        [Test]
        public void StringTest()
        {
                var cTest = new ConstructorTest(new TimeSpan(), typeof(String).GetConstructor(new Type[] { typeof(Char), typeof(Int32) }), new String('C', 5), 
                    new List<ObjectInstance>(){new ObjectInstance('C'), new ObjectInstance(5)});
                
                Assert.That(cTest.Result, Is.EqualTo("CCCCC"));
        }
    }
}

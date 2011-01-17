using System;
using System.Reflection;
using Baseline.TestHarness.UnitTests;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    internal class ExceptionThrowingTestTests
    {
        [Test]
        public void ExceptionTest()
        {
            ConstructorInfo method = GetType().GetConstructor(Type.EmptyTypes);
            var e = new ArgumentNullException();
            var t = new TimeSpan();
            var exceptTest = new ExceptionThrowingTest(t, method, e);


            Assert.That(exceptTest.Exception, Is.SameAs(e));
            Assert.That(exceptTest.Method, Is.SameAs(method));
            Assert.That(exceptTest.RunningTime, Is.EqualTo(t));
           
        }

        [Test]
        public void NullExceptionTest()
        {
            ConstructorInfo method = GetType().GetConstructor(Type.EmptyTypes);
            Assert.Throws<ArgumentNullException>(
                () => { var exceptTest = new ExceptionThrowingTest(new TimeSpan(), method, null); }
                );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;
using NUnit.Framework;

namespace Tests.UnitTests.TestHarnessTests
{
    [TestFixture]
    class TestSuiteTests
    {
        [Test]
        public void NullTypeThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { TestSuite s = new TestSuite(null, new List<UnitTest>()); });
        }

        [Test]
        public void NullTestListThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { TestSuite s = new TestSuite(typeof(TestSuiteTests), null); });
        }
    }
}

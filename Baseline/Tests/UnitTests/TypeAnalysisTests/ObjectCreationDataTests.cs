using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Tests.UnitTests
{
    [TestFixture]
    class ObjectCreationDataTests
    {

        [Test]
        public void ArgumentsDontMatchParametersTest()
        {
            Type stringType = typeof(String);
            var constructor = stringType.GetConstructor(new Type[] { typeof(char), typeof(Int32) });
            var c = new ObjectInstance('I');
            var i = new ObjectInstance(32);

            Assert.Throws<ArgumentException>(() =>
            {
                ObjectCreationData objCreationData = new ObjectCreationData(constructor, new List<ObjectInstance>() { i, c });
            });

        }
        [Test]
        public void ArgumentsMatchParametersTest()
        {
            Type stringType = typeof(String);
            var constructor = stringType.GetConstructor(new Type[] { typeof(char), typeof(Int32) });
            var c = new ObjectInstance('I');
            var i = new ObjectInstance(32);

            Assert.DoesNotThrow(() =>
            {
                ObjectCreationData objCreationData = new ObjectCreationData(constructor, new List<ObjectInstance>() { c, i });
            });

        }
        [Test]
        public void StringTest()
        {
            Type stringType = typeof(String);
            var constructor = stringType.GetConstructor(new Type[]{typeof(char), typeof(Int32)});
            var c = new ObjectInstance('I');
            var i = new ObjectInstance(32);

            var objCreationData = new ObjectCreationData(constructor, new List<ObjectInstance>(){c, i});
            Assert.That(objCreationData.HasArguments, Is.True);
            Assert.That(objCreationData.Arguments.Count, Is.EqualTo(2));
            Assert.That(objCreationData.Constructor, Is.EqualTo(constructor));
        }
        [Test]
        public void NoConstructorInfoTest()
        {
            Assert.Throws<ArgumentNullException>(() => {
                var objCreationData = new ObjectCreationData(null);
            });
        }

        [Test]
        public void ToStringTest()
        {
            Type stringType = typeof(String);
            var constructor = stringType.GetConstructor(new Type[] { typeof(char), typeof(Int32) });
            var c = new ObjectInstance('I');
            var i = new ObjectInstance(32);

            var objCreationData = new ObjectCreationData(constructor, new List<ObjectInstance>() { c, i });

            var s = new StringBuilder(objCreationData.Constructor.ToString() + " ");

            objCreationData.Arguments.ForEach(a => { s.Append(a); s.Append(" "); });

            Assert.That(s.ToString() == objCreationData.ToString());
        }
    }
}

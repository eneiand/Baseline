using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Tests.UnitTests
{
    [TestFixture]
    class ObjectInstanceTests
    {
        [Test]
        public void NeedsConstructorTest()
        {
            Assert.That(ObjectInstance.NeedsConstructor(typeof(ArgumentException)), Is.True);
            Assert.That(ObjectInstance.NeedsConstructor(typeof(Char)), Is.False);
        }

        [Test]
        public void PrimitiveInstanceTest()
        {
            var objInstance = new ObjectInstance('T');
            Assert.That(objInstance.Instance, Is.EqualTo('T'));
            Assert.That(objInstance.InstanceNeedsConstructor, Is.False);
            Assert.That(objInstance.CreationData, Is.Null);
        }

        [Test]
        public void NonPrimitiveInstanceTest()
        {
            String testString = new String('T', 5);
            Type stringType = testString.GetType();

            var creationData = new ObjectCreationData(stringType.GetConstructor(new Type[] { typeof(Char), typeof(Int32) }),
                new List<ObjectInstance>() { new ObjectInstance('T'), new ObjectInstance(5) });

            var objInstance = new ObjectInstance(testString, creationData);
            Assert.That(objInstance.Instance, Is.SameAs(testString));
            Assert.That(objInstance.InstanceNeedsConstructor, Is.True);
            Assert.That(objInstance.CreationData, Is.SameAs(creationData));
        }


        [Test]
        public void ConstructorMustMatchInstanceTest()
        {
            String testString = new String('T', 5);
            Type t = typeof(ArgumentException);

            var creationData = new ObjectCreationData(t.GetConstructor(Type.EmptyTypes));

            Assert.Throws<ArgumentException>(() =>
            {
                var objInstance = new ObjectInstance(testString, creationData);
            });
           
        }

        [Test]
        public void NullInstanceTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    var objInstance = new ObjectInstance(null);
                });
        }

        [Test]
        public void InstanceThatDoesntNeedConstructorMustNotHaveCreationDataTest()
        {
            var creationData = new ObjectCreationData(typeof(String).GetConstructor(new Type[] { typeof(Char), typeof(Int32) }),
                new List<ObjectInstance>() { new ObjectInstance('T'), new ObjectInstance(5) });

            Assert.Throws<ArgumentException>(() =>
            {
                var objInstance = new ObjectInstance('c', creationData);
            }

                );
        }

        [Test]
        public void InstanceThatNeedsConstructorMustHaveCreationDataTest()
        {
            String testString = "TEST";

           Assert.Throws<ArgumentException>(()=>{
               var strInstance = new ObjectInstance(testString);
           }
               );
        }
    }
}

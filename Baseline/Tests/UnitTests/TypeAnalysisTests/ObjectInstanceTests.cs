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
            List<String> testList = new List<String>();
            Type listType = testList.GetType();

            var creationData = new ObjectCreationData(listType.GetConstructor(Type.EmptyTypes));

            var objInstance = new ObjectInstance(testList, creationData);
            Assert.That(objInstance.Instance, Is.SameAs(testList));
            Assert.That(objInstance.InstanceNeedsConstructor, Is.True);
            Assert.That(objInstance.CreationData, Is.SameAs(creationData));
        }


        [Test]
        public void ConstructorMustMatchInstanceTest()
        {
            List<String> testList = new List<string>();
            Type t = typeof(ArgumentException);

            var creationData = new ObjectCreationData(t.GetConstructor(Type.EmptyTypes));

            Assert.Throws<ArgumentException>(() =>
            {
                var objInstance = new ObjectInstance(testList, creationData);
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
            List<String> testList = new List<string>();

           Assert.Throws<ArgumentException>(()=>{
               var strInstance = new ObjectInstance(testList);
           }
               );
        }

        [Test]
        public void ToStringTest()
        {
            var testInstance = new ObjectInstance(12);
            Assert.That(testInstance.ToString() == "12");
        }
    }
}

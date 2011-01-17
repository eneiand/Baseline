using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public sealed class ConstructorTest : UnitTest
    {
        public ConstructorTest(TimeSpan runningTime, ConstructorInfo constructor, Object instance, IEnumerable<ObjectInstance> arguments = null)
            : base(runningTime, constructor, arguments)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (constructor.ReflectedType != instance.GetType()) throw new ArgumentException("instance type does not match constructor reflected type", "instance");

            Instance = instance;
        }

        public Object Instance
        {
            get; private set;
        }
    }
}
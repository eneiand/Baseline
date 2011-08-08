using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public sealed class ConstructorTest : UnitTest
    {
        public ConstructorTest(TimeSpan runningTime, ConstructorInfo constructor, Object result, IEnumerable<ObjectInstance> arguments = null)
            : base(runningTime, constructor, arguments:arguments, result:result)
        {
            if (result == null) throw new ArgumentNullException("result");
            if (constructor.ReflectedType != result.GetType()) throw new ArgumentException("instance type does not match constructor reflected type", "result");

        
        }


    }
}
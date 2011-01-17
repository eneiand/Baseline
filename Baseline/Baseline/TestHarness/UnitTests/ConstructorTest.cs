using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.ObjectCreation;

namespace Baseline.TestHarness.UnitTests
{
    public sealed class ConstructorTest : MethodTest
    {
        public ConstructorTest(TimeSpan runningTime, ConstructorInfo constructor, Object createdObj, IEnumerable<ObjectInstance> arguments = null)
            : base(runningTime, constructor, createdObj, arguments)
        { 
        
        }

    }
}
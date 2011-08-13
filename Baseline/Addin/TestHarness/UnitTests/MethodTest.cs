using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public class MethodTest : UnitTest
    {
        public MethodTest(TimeSpan runningTime, MethodInfo method, Object result = null, IEnumerable<IObjectInstance> arguments = null, IObjectInstance instance = null)
            : base(runningTime, method, instance, arguments, result)
        {
            if(instance != null && method.IsStatic) throw new ArgumentException("method is static so no instance should be specified");
            if(instance == null && !method.IsStatic) throw new ArgumentNullException("instance", "non-static method needs instance to be called on");
            if (instance != null)
                if (instance.Instance.GetType() != method.ReflectedType)
                    throw new ArgumentException("instance type does not match method reflected type");
            
        }


    }
}
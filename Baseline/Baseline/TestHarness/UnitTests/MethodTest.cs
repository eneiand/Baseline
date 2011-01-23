using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public class MethodTest : UnitTest
    {
        public MethodTest(TimeSpan runningTime, MethodInfo method, Object result = null, IEnumerable<ObjectInstance> arguments = null, ObjectInstance instance = null) : base(runningTime, method, arguments)
        {
            if(instance != null && method.IsStatic) throw new ArgumentException("method is static so no instance should be specified");
            if(instance == null && !method.IsStatic) throw new ArgumentNullException("instance", "non-static method needs instance to be called on");
            if (instance != null)
                if (instance.Instance.GetType() != method.ReflectedType)
                    throw new ArgumentException("instance type does not match method reflected type");
            
            Instance = instance;

            if(result != null && method.ReturnType == typeof(void)) throw new ArgumentException("result must be null or Void for this method", "result");
            if (result == null && method.ReturnType != typeof(void))
                throw new ArgumentNullException("result", "result required for this method");
            if(result !=null)
                if(!method.ReturnType.IsAssignableFrom(result.GetType())) throw new ArgumentException( "result type and method return type do not match", "result");


            Result = result;
        }


        public ObjectInstance Instance
        {
            get;
            private set;
        }

        public Object Result
        {
            get;
            private set;
        }
    }
}
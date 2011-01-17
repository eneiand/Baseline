using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.ObjectCreation;

namespace Baseline.TestHarness.UnitTests
{
    public class MethodTest : UnitTest
    {
        protected MethodTest(TimeSpan runningTime, MethodBase method, Object result = null, IEnumerable<ObjectInstance> arguments = null) : base(runningTime)
        {
            if (method == null) throw new ArgumentNullException("method");

            Method = method;

            if (arguments == null)
                Arguments = new List<ObjectInstance>();
            else
                Arguments = new List<ObjectInstance>(arguments);

            var parameterList = new List<ParameterInfo>( method.GetParameters());

            if (parameterList.Count != Arguments.Count)
                throw new ArgumentException("argument count does not match parameter count");

            for (int i = 0; i < parameterList.Count; ++i)
            {
                if (!parameterList[i].ParameterType.IsAssignableFrom(Arguments[i].Instance.GetType()))
                    throw new ArgumentException("argument types do not match parameter types");
            }

            Result = result;
        }

        public MethodBase Method
        {
            get;
            private set;
        }

        public List<ObjectInstance> Arguments
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
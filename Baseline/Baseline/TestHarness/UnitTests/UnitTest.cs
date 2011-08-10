using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public abstract class UnitTest
    {
         protected UnitTest(TimeSpan runningTime, MethodBase method, ObjectInstance instance = null,  IEnumerable<ObjectInstance> arguments = null, Object result = null)
        {
  
            RunningTime = runningTime;

            if (method == null) throw new ArgumentNullException("method");

            Method = method;

            if (arguments == null)
                Arguments = new List<ObjectInstance>();
            else
                Arguments = new List<ObjectInstance>(arguments);

            var parameterList = new List<ParameterInfo>(method.GetParameters());

            if (parameterList.Count != Arguments.Count)
                throw new ArgumentException("argument count does not match parameter count");

            for (int i = 0; i < parameterList.Count; ++i)
            {
                if (!parameterList[i].ParameterType.IsAssignableFrom(Arguments[i].Instance.GetType()))
                    throw new ArgumentException("argument types do not match parameter types");
            }

            if (!method.IsConstructor)
            {
                if (!method.IsStatic && instance == null)
                    throw new ArgumentException("non-static method must have an instance to be invoked on");
                if (method.IsStatic && instance != null)
                    throw new ArgumentException("static method cannot have an instance");
            }
             Result = result;
             Instance = instance;
        }

        public TimeSpan RunningTime
        {
            get;
            protected set;
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

        public ObjectInstance Instance
        {
            get; private set; 
        }

        public Object Result { get; private set; }

        public virtual String Name
        {
            get { return String.Format("{0}Test",  this.Method.Name); }
        }
    }
}

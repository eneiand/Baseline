using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Baseline.TypeAnalysis.ObjectInstantiation;


namespace Baseline.TestHarness
{
    public abstract class UnitTest
    {
         protected UnitTest(TimeSpan runningTime, MethodBase method,  IEnumerable<ObjectInstance> arguments = null)
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
    }
}

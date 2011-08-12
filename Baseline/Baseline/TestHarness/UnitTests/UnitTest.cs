using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public abstract class UnitTest
    {
        private static long m_GlobalId = 0;

        protected long m_id;

        protected UnitTest(TimeSpan runningTime, MethodBase method, IObjectInstance instance = null, IEnumerable<IObjectInstance> arguments = null, Object result = null)
        {
  
            RunningTime = runningTime;

            if (method == null) throw new ArgumentNullException("method");

            Method = method;

            if (arguments == null)
                Arguments = new List<IObjectInstance>();
            else
                Arguments = new List<IObjectInstance>(arguments);

            var parameterList = new List<ParameterInfo>(method.GetParameters());

            if (parameterList.Count != Arguments.Count)
                throw new ArgumentException("argument count does not match parameter count");

            for (int i = 0; i < parameterList.Count; ++i)
            {
                if (!(Arguments[i] is NullObjectInstance) && !parameterList[i].ParameterType.IsAssignableFrom(Arguments[i].Instance.GetType()))
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

             m_id = Interlocked.Increment(ref m_GlobalId);
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

        public List<IObjectInstance> Arguments
        {
            get;
            private set;
        }

        public IObjectInstance Instance
        {
            get; private set; 
        }

        public Object Result { get; private set; }

        public virtual String Name
        {
            get { return String.Format("{0}Test{1}",  this.Method.Name, this.m_id); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Baseline.ObjectCreation;

namespace Baseline.TestHarness
{
    public abstract class UnitTest
    {
        public UnitTest(TimeSpan runningTime)
        {
            if (runningTime == null) throw new ArgumentNullException("runningTime");
            RunningTime = runningTime;
        }

        public TimeSpan RunningTime
        {
            get;
            protected set;
        }
    }

    public class MethodTest : UnitTest
    {
        public MethodTest(TimeSpan runningTime, MethodBase method, Object result = null, IEnumerable<ObjectInstance> arguments = null) : base(runningTime)
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

    public sealed class ExceptionThrowingTest : MethodTest
    {
        
        public ExceptionThrowingTest(TimeSpan runningTime, MethodBase method, Exception thrownException,
            IEnumerable<ObjectInstance> arguments = null)
            : base(runningTime, method, arguments)
        {
            if (thrownException == null) throw new ArgumentNullException("thrownException");
            Exception = thrownException;
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }

    public sealed class ConstructorTest : MethodTest
    {
        public ConstructorTest(TimeSpan runningTime, ConstructorInfo constructor, Object createdObj, IEnumerable<ObjectInstance> arguments = null)
            : base(runningTime, constructor, createdObj, arguments)
        { 
        
        }

    }

    
}

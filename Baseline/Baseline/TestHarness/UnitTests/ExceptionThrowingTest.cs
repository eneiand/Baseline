using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;


namespace Baseline.TestHarness.UnitTests
{
    public sealed class ExceptionThrowingTest : UnitTest
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

        public override string Name
        {
            get { return String.Format("{0}{1}ThrowsExceptionTest", this.Method.DeclaringType.Name, this.Method.Name); }
        }
    }
}
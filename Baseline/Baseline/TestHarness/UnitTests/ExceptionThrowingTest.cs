using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.ObjectCreation;

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
    }
}
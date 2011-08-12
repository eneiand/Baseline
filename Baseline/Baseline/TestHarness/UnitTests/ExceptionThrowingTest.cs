using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;


namespace Baseline.TestHarness.UnitTests
{
    public sealed class ExceptionThrowingTest : UnitTest
    {

        public ExceptionThrowingTest(TimeSpan runningTime, MethodBase method, Exception thrownException, IObjectInstance instance = null,
                                     IEnumerable<IObjectInstance> arguments = null)
            : base(runningTime, method, instance, arguments)
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
            get { return String.Format("{0}ThrowsExceptionTest{1}",  this.Method.Name, this.m_id); }
        }
    }
}
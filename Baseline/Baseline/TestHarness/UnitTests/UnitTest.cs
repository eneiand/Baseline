using System;
using System.Linq;
using System.Text;

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
}

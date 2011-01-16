﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baseline.TestHarness
{
    public class TestSuite
    {
        
        public TestSuite(Type t, IEnumerable<UnitTest> tests)
        {
            if (t == null) throw new ArgumentNullException("t");
            if (tests == null) throw new ArgumentNullException("tests");

            Type = t;
            Tests = new List<UnitTest>(tests);

            if (Tests.Count == 0)
                throw new ArgumentException("tests must have unit tests");

        }

        public Type Type
        {
            get;
            private set;
        }

        public List<UnitTest> Tests
        {
            get;
            private set;
        }
    }
}

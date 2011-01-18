using System;
using System.Collections.Generic;
using System.Reflection;
using Baseline.TestHarness;

namespace Baseline.TypeAnalysis
{
    class Tester
    {
        private readonly TestValueCalculator m_RangeCalculator;

        public Tester(TestValueCalculator rangeCalculator)
        {
            if (rangeCalculator == null) throw new ArgumentNullException("rangeCalculator");
            m_RangeCalculator = rangeCalculator;
        }

        public Tester() : this(new DefaultTestValueCalculator())
        {
        }

        public TestSuite GenerateTests(Type t)
        {
            throw new NotImplementedException();
        }

        public List<TestSuite> GenerateTests(Assembly a)
        {
            List<TestSuite> testSuites = new List<TestSuite>();

            foreach (var type in a.GetTypes())
                testSuites.Add(GenerateTests(type));

            return testSuites;
        }
    }
}

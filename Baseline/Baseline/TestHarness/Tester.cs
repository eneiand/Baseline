using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Baseline.TestHarness
{
    class Tester
    {
        public static TestSuite GenerateTests(Type t)
        {
            throw new NotImplementedException();
        }

        public static List<TestSuite> GenerateTests(Assembly a)
        {
            List<TestSuite> testSuites = new List<TestSuite>();

            foreach (var type in a.GetTypes())
                testSuites.Add(GenerateTests(type));

            return testSuites;
        }
    }
}

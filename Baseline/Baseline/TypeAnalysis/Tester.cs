using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TypeAnalysis
{
    public class Tester
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
            if (t == null) throw new ArgumentNullException("t");

            var tests = new List<UnitTest>();

            tests.AddRange(TestConstructors(t.GetConstructors()));
            tests.AddRange(TestMethods(t.GetMethods()));

            return new TestSuite(t, tests);
        }

        private IEnumerable<UnitTest> TestMethods(IEnumerable<MethodInfo> methods)
        {
            var tests = new List<UnitTest>();

            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();
                List<List<ObjectInstance>> argumentCombinations = CalculateCombinations(parameters);

                foreach (var argumentCombination in argumentCombinations)
                {
                    foreach (ObjectInstance objectInstance in GetInstancesFor(method))
                    {
                        var timer = new Stopwatch();
                        try
                        {
                            timer.Start();
                            object result = method.Invoke(objectInstance == null ? null : objectInstance.Instance,
                                                          GetArguments(argumentCombination));
                            timer.Stop();
                            tests.Add(new MethodTest(timer.Elapsed, method, result, argumentCombination,
                                                     objectInstance));
                        }
                        catch (Exception e)
                        {
                            timer.Stop();
                            tests.Add(new ExceptionThrowingTest(timer.Elapsed, method, e, argumentCombination));
                        }
                    }
                }
            }

            return tests;
        }

        private IEnumerable<ObjectInstance> GetInstancesFor(MethodInfo method)
        {
            var objs = new List<ObjectInstance>();
            if (method.IsStatic)
            {
                objs.Add(null);
            }
            else
            {
                objs.AddRange(m_RangeCalculator.GetTestValues(method.ReflectedType));
            }
            return objs;
        }

        private List<UnitTest> TestConstructors(IEnumerable<ConstructorInfo> constructors)
        {
            var tests = new List<UnitTest>();
            foreach (ConstructorInfo constructorInfo in constructors)
            {
                ParameterInfo[] parameters = constructorInfo.GetParameters();
                List<List<ObjectInstance>> argumentCombinations = CalculateCombinations(parameters);
                
                if(parameters.Count() == 0)
                {   //adding an empty argument list for the default constructor
                    argumentCombinations.Add(new List<ObjectInstance>());
                }


                foreach (var argumentCombination in argumentCombinations)
                {
                    var timer = new Stopwatch();
                    try
                    {
                        timer.Start();
                        object instance = constructorInfo.Invoke(GetArguments(argumentCombination));
                        timer.Stop();
                        tests.Add(new ConstructorTest(timer.Elapsed, constructorInfo, instance, argumentCombination));
                    }
                    catch (Exception e)
                    {
                        timer.Stop();
                        tests.Add(new ExceptionThrowingTest(timer.Elapsed, constructorInfo, e, argumentCombination));
                    }
                }
            }

            return tests;
        }

        private List<List<ObjectInstance>> CalculateCombinations(IEnumerable<ParameterInfo> parameters)
        {
            List<List<ObjectInstance>> rangeOfArgumentValues =
                parameters.Select(param => m_RangeCalculator.GetTestValues(param.ParameterType)).ToList();

            return CalculateCombinations(rangeOfArgumentValues);
        }

        public List<TestSuite> GenerateTests(Assembly a)
        {
            var testSuites = new List<TestSuite>();

            foreach (Type type in a.GetTypes())
                testSuites.Add(GenerateTests(type));

            return testSuites;
        }

        public static List<List<ObjectInstance>> CalculateCombinations(List<List<ObjectInstance>> input)
        {
            List<List<ObjectInstance>> combinations = CalculateCombinations(input, 0);
            combinations.ForEach(l => l.Reverse());
            return combinations;
        }

        // i is used for recursion, for the initial call this should be 0
        private static List<List<ObjectInstance>> CalculateCombinations(List<List<ObjectInstance>> input, int i)
        {
            // stop condition
            if (i == input.Count)
            {
                // return a list with an empty list
                var r = new List<List<ObjectInstance>>();
                r.Add(new List<ObjectInstance>());
                return r;
            }

            var result = new List<List<ObjectInstance>>();
            List<List<ObjectInstance>> recursive = CalculateCombinations(input, i + 1); // recursive call

            // for each element of the first list of input
            for (int j = 0; j < input[i].Count; j++)
            {
                // add the element to all combinations obtained for the rest of the lists
                for (int k = 0; k < recursive.Count; k++)
                {
                    // copy a combination from recursive
                    var newList = new List<ObjectInstance>();
                    foreach (ObjectInstance obj in recursive[k]) newList.Add(obj);
                    // add element of the first list
                    newList.Add(input[i][j]);
                    // add new combination to result
                    result.Add(newList);
                }
            }

            return result;
        }


        public static object[] GetArguments(List<ObjectInstance> paramCombination)
        {
            var arguments = new List<object>();

            foreach (ObjectInstance objectInstance in paramCombination)
            {
                arguments.Add(objectInstance.Instance);
            }
            return arguments.ToArray();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TypeAnalysis
{
    public class Tester
    {
        private readonly TestValueCalculator m_RangeCalculator;
        private readonly List<string> m_MethodExclusions;

        public Tester(TestValueCalculator rangeCalculator, List<String> methodExclusions)
        {
            if (rangeCalculator == null) throw new ArgumentNullException("rangeCalculator");
            if (methodExclusions == null) throw new ArgumentNullException("methodExclusions");
            m_RangeCalculator = rangeCalculator;
            m_MethodExclusions = methodExclusions;
        }

        public Tester() : this(new DefaultTestValueCalculator(), new List<string>(){"GetType", "GetHashCode"})
        {
        }

        public TestSuite GenerateTests(Type t)
        {
            if (t == null) throw new ArgumentNullException("t");

            var tests = new List<UnitTest>();

            var constructorsDoneEvent = new ManualResetEvent(false);
            var methodsDoneEvent = new ManualResetEvent(false);

            ThreadPool.QueueUserWorkItem((w) =>
                                             {
                                                 List<UnitTest> constructorTests = TestConstructors(t.GetConstructors());
                                                 lock (tests)
                                                 {
                                                     tests.AddRange(constructorTests);
                                                 }
                                                 constructorsDoneEvent.Set();
                                             });

            var methodsToTest = (from m in t.GetMethods() where !m_MethodExclusions.Contains(m.Name) select m);

            ThreadPool.QueueUserWorkItem((w) =>
                                             {
                                                 IEnumerable<UnitTest> methodTests = TestMethods(methodsToTest.ToArray());
                                                 lock (tests)
                                                 {
                                                     tests.AddRange(methodTests);
                                                 }
                                                 methodsDoneEvent.Set();
                                             });

            methodsDoneEvent.WaitOne();
            constructorsDoneEvent.WaitOne();
          //  WaitHandle.WaitAll(new WaitHandle[] {constructorsDoneEvent, methodsDoneEvent});

            return new TestSuite(t, tests);
        }

        private List<UnitTest> TestMethods(MethodInfo[] methods)
        {
            var tests = new List<UnitTest>();

            int numTasks = methods.Length;

            if (methods.Length > 0)
            {
                using (var finishedEvent = new ManualResetEvent(false))
                {
                    for (int i = 0; i < methods.Length; ++i)
                    {
                        MethodInfo method = methods[i];

                        ThreadPool.QueueUserWorkItem(w =>
                                                         {
                                                             ParameterInfo[] parameters = method.GetParameters();
                                                             List<List<IObjectInstance>> argumentCombinations =
                                                                 CalculateCombinations(parameters);

                                                             foreach (var argumentCombination in argumentCombinations)
                                                             {
                                                                 foreach (
                                                                     ObjectInstance objectInstance in
                                                                         GetInstancesFor(method))
                                                                 {
                                                                     var timer = new Stopwatch();
                                                                     try
                                                                     {
                                                                         timer.Start();
                                                                         object result =
                                                                             method.Invoke(
                                                                                 objectInstance == null
                                                                                     ? null
                                                                                     : objectInstance.Instance,
                                                                                 GetArguments(argumentCombination));
                                                                         timer.Stop();

                                                                         lock (tests)
                                                                         {
                                                                             tests.Add(new MethodTest(timer.Elapsed,
                                                                                                      method,
                                                                                                      result,
                                                                                                      argumentCombination,
                                                                                                      objectInstance));
                                                                         }
                                                                     }
                                                                     catch (Exception e)
                                                                     {
                                                                         timer.Stop();
                                                                         lock (tests)
                                                                         {
                                                                             tests.Add(
                                                                                 new ExceptionThrowingTest(
                                                                                     timer.Elapsed,
                                                                                     method, e.InnerException, objectInstance,
                                                                                     argumentCombination));
                                                                         }
                                                                     }
                                                                 }
                                                             }
                                                             if (Interlocked.Decrement(ref numTasks) == 0)
                                                                 finishedEvent.Set();
                                                         });
                    }


                    finishedEvent.WaitOne();
                }
            }

            return tests;
        }

        private IEnumerable<IObjectInstance> GetInstancesFor(MethodInfo method)
        {
            var objs = new List<IObjectInstance>();
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

        private List<UnitTest> TestConstructors(ConstructorInfo[] constructors)
        {
            var tests = new List<UnitTest>();
            int numTasks = constructors.Length;

            if (constructors.Length > 0)
            {
                using (var finishedEvent = new ManualResetEvent(false))
                {
                    for (int i = 0; i < constructors.Length; ++i)
                    {
                        ConstructorInfo constructorInfo = constructors[i];

                        ThreadPool.QueueUserWorkItem(w =>
                                                         {
                                                             ParameterInfo[] parameters =
                                                                 constructorInfo.GetParameters();
                                                             List<List<IObjectInstance>> argumentCombinations =
                                                                 CalculateCombinations(parameters);

                                                             if (parameters.Count() == 0)
                                                             {
                                                                 //adding an empty argument list for the default constructor
                                                                 argumentCombinations.Add(new List<IObjectInstance>());
                                                             }


                                                             foreach (var argumentCombination in argumentCombinations)
                                                             {
                                                                 var timer = new Stopwatch();
                                                                 try
                                                                 {
                                                                     timer.Start();
                                                                     object instance =
                                                                         constructorInfo.Invoke(
                                                                             GetArguments(argumentCombination));
                                                                     timer.Stop();
                                                                     lock (tests)
                                                                     {
                                                                         tests.Add(new ConstructorTest(timer.Elapsed,
                                                                                                       constructorInfo,
                                                                                                       instance,
                                                                                                       argumentCombination));
                                                                     }
                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     timer.Stop();
                                                                     lock (tests)
                                                                     {
                                                                         tests.Add(
                                                                             new ExceptionThrowingTest(timer.Elapsed,
                                                                                                       constructorInfo,
                                                                                                       e.InnerException, null,
                                                                                                       argumentCombination));
                                                                     }
                                                                 }
                                                             }

                                                             if (Interlocked.Decrement(ref numTasks) == 0)
                                                                 finishedEvent.Set();
                                                         });
                    }

                    finishedEvent.WaitOne();
                }
            }
            return tests;
        }

        private List<List<IObjectInstance>> CalculateCombinations(IEnumerable<ParameterInfo> parameters)
        {
            List<List<IObjectInstance>> rangeOfArgumentValues =
                parameters.Select(param => m_RangeCalculator.GetTestValues(param.ParameterType)).ToList();

            return CalculateCombinations(rangeOfArgumentValues);
        }

        public List<TestSuite> GenerateTests(Assembly a)
        {
            var testSuites = new List<TestSuite>();

            Type[] types = a.GetTypes();
            int numTasks = types.Length;

            if (types.Length > 0)
            {
                using (var finishedEvent = new ManualResetEvent(false))
                {
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type type = types[i];
                        ThreadPool.QueueUserWorkItem((w) =>
                                                         {
                                                             TestSuite suite = GenerateTests(type);
                                                             lock (testSuites)
                                                                 testSuites.Add(suite);
                                                             if (Interlocked.Decrement(ref numTasks) == 0)
                                                                 finishedEvent.Set();
                                                         });
                    }

                    finishedEvent.WaitOne();
                }
            }

            return testSuites;
        }

        public static List<List<IObjectInstance>> CalculateCombinations(List<List<IObjectInstance>> input)
        {
            List<List<IObjectInstance>> combinations = CalculateCombinations(input, 0);
            combinations.ForEach(l => l.Reverse());
            return combinations;
        }

        // i is used for recursion, for the initial call this should be 0
        private static List<List<IObjectInstance>> CalculateCombinations(List<List<IObjectInstance>> input, int i)
        {
            // stop condition
            if (i == input.Count)
            {
                // return a list with an empty list
                var r = new List<List<IObjectInstance>>();
                r.Add(new List<IObjectInstance>());
                return r;
            }

            var result = new List<List<IObjectInstance>>();
            List<List<IObjectInstance>> recursive = CalculateCombinations(input, i + 1); // recursive call

            // for each element of the first list of input
            for (int j = 0; j < input[i].Count; j++)
            {
                // add the element to all combinations obtained for the rest of the lists
                for (int k = 0; k < recursive.Count; k++)
                {
                    // copy a combination from recursive
                    var newList = new List<IObjectInstance>();
                    foreach (IObjectInstance obj in recursive[k]) newList.Add(obj);
                    // add element of the first list
                    newList.Add(input[i][j]);
                    // add new combination to result
                    result.Add(newList);
                }
            }

            return result;
        }


        public static object[] GetArguments(List<IObjectInstance> paramCombination)
        {
            var arguments = new List<object>();

            foreach (IObjectInstance objectInstance in paramCombination)
            {
                arguments.Add(objectInstance.Instance);
            }
            return arguments.ToArray();
        }
    }
}
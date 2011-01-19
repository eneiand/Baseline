using System;
using System.Collections.Generic;
using Baseline.TypeAnalysis.ObjectInstantiation;
using System.Reflection;
using System.Linq;

namespace Baseline.TypeAnalysis
{
    public class DefaultTestValueCalculator : TestValueCalculator
    {
        protected override List<ObjectInstance> GetTestValuesFor(Type type)
        {
            var values = new List<ObjectInstance>();

            ConstructorInfo[] constructors = type.GetConstructors();

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameterInfos = constructor.GetParameters();

                if (parameterInfos.Count() != 0)
                {
                    List<List<ObjectInstance>> rangeOfParameterValues =
                        parameterInfos.Select(param => this.GetTestValues(param.ParameterType)).ToList();

                    List<List<ObjectInstance>> paramCombinations = (CalculateCombinations(rangeOfParameterValues, 0));

                    foreach (var paramCombination in paramCombinations)
                    {
                        paramCombination.Reverse();
                        try
                        {
                            //every combination of parameters should create a range of object via the constructor
                            values.Add(new ObjectInstance(constructor.Invoke(GetArguments(paramCombination)),
                                new ObjectCreationData(constructor,
                                                          paramCombination)));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception thrown trying to call constructor " + constructor
                                              + " with values "
                                              + paramCombination);
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                else
                {
                    //default constructor
                    try
                    {
                        values.Add(new ObjectInstance(constructor.Invoke(null), 
                            new ObjectCreationData(constructor)));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception thrown trying to call constructor " + constructor
                                          + " using default constructor");
                        Console.WriteLine(e.ToString());
                    }
                }
            }

            return values;
        }

        private static object[] GetArguments(List<ObjectInstance> paramCombination)
        {
            List<Object> arguments = new List<object>();

            foreach (ObjectInstance objectInstance in paramCombination)
            {
                arguments.Add(objectInstance.Instance);
            }
            return arguments.ToArray();
        }

        private static object[] GetArguments(ObjectInstance[] paramCombination)
        {
            return GetArguments(new List<ObjectInstance>(paramCombination));
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

        protected override List<ObjectInstance> GetEnumTestValues(Type type)
        {
            List<ObjectInstance> vals = new List<ObjectInstance>();

            foreach (object enumValue in type.GetEnumValues())
            {
                vals.Add(new ObjectInstance(enumValue));
            }

            return vals;
        }


        protected override List<ObjectInstance> GetStringTestValues()
        {
            return new List<ObjectInstance>()
            {
                new ObjectInstance(""),
                new ObjectInstance(@" "),
                new ObjectInstance("TEST_STRING"),
                new ObjectInstance("1234567890")
            };
        }

        protected override List<ObjectInstance> GetObjectTestValues()
        {
            var vals = new List<ObjectInstance>(GetStringTestValues());

            vals.AddRange(GetDecimalTestValues());
            vals.AddRange(GetBoolTestValues());

            return vals;
        }

        protected override List<ObjectInstance> GetDecimalTestValues()
        {
            var vals = GetMaxAndMin(DECIMAL_TYPE);
            vals.AddRange(new List<ObjectInstance>()
            {
                new ObjectInstance(Decimal.MinusOne),
                new ObjectInstance(Decimal.Zero),
                new ObjectInstance(Decimal.One)
            });

            return vals;
        }

        protected override List<ObjectInstance> GetBoolTestValues()
        {
            return new List<ObjectInstance>()
            {
                new ObjectInstance(true),
                new ObjectInstance(false)
            };
        }

        protected override List<ObjectInstance> GetDoubleTestValues()
        {
            return GetMaxAndMin(DOUBLE_TYPE);
        }

        protected override List<ObjectInstance> GetFloatTestValues()
        {
            return GetMaxAndMin(FLOAT_TYPE);
        }

        protected override List<ObjectInstance> GetCharTestValues()
        {
            return GetMaxAndMin(CHAR_TYPE);
        }

        protected override List<ObjectInstance> GetULongTestValues()
        {
            return GetMaxAndMin(ULONG_TYPE);
        }

        protected override List<ObjectInstance> GetLongTestValues()
        {
            return GetMaxAndMin(LONG_TYPE);
        }

        protected override List<ObjectInstance> GetUIntTestValues()
        {
            return GetMaxAndMin(UINT_TYPE);

        }

        protected override List<ObjectInstance> GetIntTestValues()
        {
            return GetMaxAndMin(INT_TYPE);
        }

        protected override List<ObjectInstance> GetUShortTestValues()
        {
            return GetMaxAndMin(USHORT_TYPE);

        }

        protected override List<ObjectInstance> GetShortTestValues()
        {
            return GetMaxAndMin(SHORT_TYPE);

        }

        protected override List<ObjectInstance> GetByteTestValues()
        {
            return GetMaxAndMin(BYTE_TYPE);

        }

        protected override List<ObjectInstance> GetSByteTestValues()
        {
            return GetMaxAndMin(SBYTE_TYPE);

        }

        private static List<ObjectInstance> GetMaxAndMin(Type t)
        {
            return new List<ObjectInstance>()
            {
                new ObjectInstance(t.GetField("MaxValue").GetValue(null)),
                new ObjectInstance(t.GetField("MinValue").GetValue(null))
            };
        }
    }
}
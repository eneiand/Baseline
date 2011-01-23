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

                    List<List<ObjectInstance>> paramCombinations = (Tester.CalculateCombinations(rangeOfParameterValues));

                    foreach (var paramCombination in paramCombinations)
                    {
                        try
                        {
                            //every combination of parameters should create a range of object via the constructor
                            values.Add(new ObjectInstance(constructor.Invoke(Tester.GetArguments(paramCombination)),
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
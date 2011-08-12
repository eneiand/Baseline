using System;
using System.Collections.Generic;
using Baseline.TypeAnalysis.ObjectInstantiation;
using System.Reflection;
using System.Linq;

namespace Baseline.TypeAnalysis
{
    public class DefaultTestValueCalculator : TestValueCalculator
    {
        protected override List<IObjectInstance> GetTestValuesFor(Type type)
        {
            var values = new List<IObjectInstance>();

            ConstructorInfo[] constructors = type.GetConstructors();

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameterInfos = constructor.GetParameters();

                if (parameterInfos.Count() != 0)
                {
                    List<List<IObjectInstance>> rangeOfParameterValues =
                        parameterInfos.Select(param => this.GetTestValues(param.ParameterType)).ToList();

                    List<List<IObjectInstance>> paramCombinations = (Tester.CalculateCombinations(rangeOfParameterValues));

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
                            //Console.WriteLine("Exception thrown trying to call constructor " + constructor
                            //                  + " with values "
                            //                  + paramCombination);
                            //Console.WriteLine(e.ToString());
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



        protected override List<IObjectInstance> GetEnumTestValues(Type type)
        {
            List<IObjectInstance> vals = new List<IObjectInstance>();

            foreach (object enumValue in type.GetEnumValues())
            {
                vals.Add(new ObjectInstance(enumValue));
            }

            return vals;
        }


        protected override List<IObjectInstance> GetStringTestValues()
        {
            return new List<IObjectInstance>()
            {
                new ObjectInstance(""),
                new ObjectInstance(@" "),
                new ObjectInstance("TEST_STRING"),
                new ObjectInstance("1234567890"),
                new NullObjectInstance()
            };
        }

        protected override List<IObjectInstance> GetObjectTestValues()
        {
            var vals = new List<IObjectInstance>(GetStringTestValues());

            vals.AddRange(GetDecimalTestValues());
            vals.AddRange(GetBoolTestValues());

            return vals;
        }

        protected override List<IObjectInstance> GetDecimalTestValues()
        {
            var vals = GetMaxAndMin(DECIMAL_TYPE);
            vals.AddRange(new List<IObjectInstance>()
            {
                new ObjectInstance(Decimal.MinusOne),
                new ObjectInstance(Decimal.Zero),
                new ObjectInstance(Decimal.One)
            });

            return vals;
        }

        protected override List<IObjectInstance> GetBoolTestValues()
        {
            return new List<IObjectInstance>()
            {
                new ObjectInstance(true),
                new ObjectInstance(false)
            };
        }

        protected override List<IObjectInstance> GetDoubleTestValues()
        {
            return GetMaxAndMin(DOUBLE_TYPE);
        }

        protected override List<IObjectInstance> GetFloatTestValues()
        {
            return GetMaxAndMin(FLOAT_TYPE);
        }

        protected override List<IObjectInstance> GetCharTestValues()
        {
            return new List<IObjectInstance>(){new ObjectInstance('a'), new ObjectInstance('z'), new ObjectInstance('0'), new ObjectInstance('1'), new ObjectInstance(' ')};
        }

        protected override List<IObjectInstance> GetULongTestValues()
        {
            return GetMaxAndMin(ULONG_TYPE);
        }

        protected override List<IObjectInstance> GetLongTestValues()
        {
            return GetMaxAndMin(LONG_TYPE);
        }

        protected override List<IObjectInstance> GetUIntTestValues()
        {
            return GetMaxAndMin(UINT_TYPE);

        }

        protected override List<IObjectInstance> GetIntTestValues()
        {
            return GetMaxAndMin(INT_TYPE);
        }

        protected override List<IObjectInstance> GetUShortTestValues()
        {
            return GetMaxAndMin(USHORT_TYPE);

        }

        protected override List<IObjectInstance> GetShortTestValues()
        {
            return GetMaxAndMin(SHORT_TYPE);

        }

        protected override List<IObjectInstance> GetByteTestValues()
        {
            return GetMaxAndMin(BYTE_TYPE);

        }

        protected override List<IObjectInstance> GetSByteTestValues()
        {
            return GetMaxAndMin(SBYTE_TYPE);

        }

        private static List<IObjectInstance> GetMaxAndMin(Type t)
        {
            return new List<IObjectInstance>()
            {
                new ObjectInstance(t.GetField("MaxValue").GetValue(null)),
                new ObjectInstance(t.GetField("MinValue").GetValue(null))
            };
        }


  
    }
}
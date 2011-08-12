using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.TestHarness.UnitTests
{
    public sealed class ConstructorTest : UnitTest
    {
        public ConstructorTest(TimeSpan runningTime, ConstructorInfo constructor, Object result, IEnumerable<IObjectInstance> arguments = null)
            : base(runningTime, constructor, arguments:arguments, result:result)
        {
            if (result == null) throw new ArgumentNullException("result");
            if (constructor.ReflectedType != result.GetType()) throw new ArgumentException("instance type does not match constructor reflected type", "result");

        
        }

        public override string Name
        {
            get
            {


                List<String> paramTypes = new List<string>(from p in Method.GetParameters() select p.ParameterType.Name);
                String pTypes = String.Empty;
                paramTypes.ForEach(p => pTypes+=p);

                return String.Format("{0}ConstructorTest{1}",
                                     this.Method.GetParameters().Length == 0 ? "Default" : pTypes, this.m_id);
            }
        }

    }
}
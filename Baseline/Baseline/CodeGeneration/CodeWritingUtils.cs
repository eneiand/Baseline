using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.CodeGeneration
{
    static class CodeWritingUtils
    {
        public static String GetVariableInstantiationCode(ObjectInstance instance, String variableName = "instance")
        {
            StringBuilder code = new StringBuilder();
            
            code.AppendFormat("var {0} = ", variableName );

            code.Append(GetObjectCreationExpression(instance));

            return code.ToString();
        }

        public static String GetObjectCreationExpression(ObjectInstance instance)
        {
            StringBuilder code = new StringBuilder();

            if (instance.InstanceNeedsConstructor)
            {
                code.AppendFormat("new {0}();", instance.Instance.GetType().Name);
            }
            else
            {

            }

            return code.ToString();
        }
    }
}

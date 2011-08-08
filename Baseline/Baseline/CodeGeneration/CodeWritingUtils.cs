using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.CodeGeneration
{
    static class CodeWritingUtils
    {
        public static String GetVariableInstantiationStatement(ObjectInstance instance, String variableName = "instance")
        {
            StringBuilder code = new StringBuilder();
            
            code.AppendFormat("var {0} = ", variableName );

            code.Append(GetObjectCreationExpression(instance));
            code.Append(";");

            return code.ToString();
        }

        public static String GetObjectCreationExpression(ObjectInstance instance)
        {
            StringBuilder code = new StringBuilder();

            if (instance.InstanceNeedsConstructor)
            {
                code.AppendFormat("new {0}()", instance.Instance.GetType().Name);
            }
            else
            {

            }

            return code.ToString();
        }

        public static String GetObjectCreationExpression(Object instance)
        {
            var typeOfInstance = instance.GetType();

            if(!ObjectInstance.NeedsConstructor(typeOfInstance))
            {
                if (instance is String)
                    return "\"" + instance + "\"";
                else if (instance is char)
                    return "'" + instance + "'";
                else
                    return instance.ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
          
        }

        public static String GetMethodInvocationExpression(String entityToCallMethodOn, MethodBase method, IEnumerable<ObjectInstance> arguments = null )
        {
            StringBuilder code = new StringBuilder();
            code.AppendFormat("{0}.{1}(", entityToCallMethodOn, method.Name);

            code.Append(")");
            return code.ToString();
        }

        public static String GetMethodInvocationStatement(String entityToCallMethodOn, MethodBase method, IEnumerable<ObjectInstance> arguments = null)
        {
            return GetMethodInvocationExpression(entityToCallMethodOn, method, arguments) + ";";
        }
    }
}

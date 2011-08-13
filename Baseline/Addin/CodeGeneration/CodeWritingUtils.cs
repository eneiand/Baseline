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
        public static String GetVariableInstantiationStatement(IObjectInstance instance, String variableName = "instance")
        {
            StringBuilder code = new StringBuilder();
            
            code.AppendFormat("var {0} = ", variableName );

            code.Append(GetObjectCreationExpression(instance));
            code.Append(";");

            return code.ToString();
        }

        public static String GetVariableInstantiationStatement(ConstructorInfo constructor, IEnumerable<IObjectInstance> arguments, String variableName = "instance")
        {
            StringBuilder code = new StringBuilder();

            code.AppendFormat("var {0} = ", variableName);

            code.Append(GetConstructorInvocationExpression(constructor, arguments));
            code.Append(";");

            return code.ToString();
        }

        public static String GetObjectCreationExpression(IObjectInstance instance)
        {

            if (instance == null)
                return "null";

            StringBuilder code = new StringBuilder();

                
            if (instance.InstanceNeedsConstructor)
            {
                code.AppendFormat("new {0}(", instance.Instance.GetType().Name);

                var args = instance.CreationData.Arguments;
                for (int i = 0; i < args.Count; ++i )
                {
                    code.AppendFormat("{0}{1}", GetObjectCreationExpression(args[i]), i != args.Count-1? ", " : String.Empty);
                }


                    code.AppendFormat(")");
            }
            else
            {
                if (instance.Instance is String)
                    return "\"" + instance.Instance + "\"";
                else if (instance.Instance is char)
                    return "'" + instance.Instance + "'";
                else if (instance.Instance is Enum)
                    return instance.Instance.GetType().Name + "." + instance.Instance.ToString();
                else if (instance is NullObjectInstance)
                    return "null";
                else if (instance.Instance is bool)
                    return instance.ToString().ToLower();
                else
                    return instance.Instance.ToString();
            }

            return code.ToString();
        }

        public static String GetObjectCreationExpression(Object instance)
        {
            if (instance == null)
                    return "null";

            var typeOfInstance = instance.GetType();

            if(!ObjectInstance.NeedsConstructor(typeOfInstance))
            {
                if (instance is String)
                    return "\"" + instance + "\"";
                else if (instance is char)
                    return "'" + instance + "'";
                else if (instance is Enum)
                    return instance.GetType().Name + "." + instance.ToString();

                else if (instance is bool)
                    return instance.ToString().ToLower();
                else
                    return instance.ToString();
            }
            else
            {
                return String.Empty;
            }
          
        }

        public static String GetMethodInvocationExpression(String entityToCallMethodOn, MethodBase method, IEnumerable<IObjectInstance> arguments = null)
        {
            List<IObjectInstance> argumentList = arguments == null ? new List<IObjectInstance>() : new List<IObjectInstance>(arguments);

            StringBuilder code = new StringBuilder();
            code.AppendFormat("{0}.{1}(", entityToCallMethodOn, method.Name);
            for (int i = 0; i < argumentList.Count; ++i)
            {
                code.AppendFormat("{0}{1}", GetObjectCreationExpression(argumentList[i]),
                                  i != argumentList.Count - 1 ? ", " : String.Empty);
            }
            code.Append(")");
            return code.ToString();
        }

        public static String GetConstructorInvocationExpression(ConstructorInfo constructor, IEnumerable<IObjectInstance> arguments = null)
        {
            List<IObjectInstance> argumentList = arguments == null ? new List<IObjectInstance>() : new List<IObjectInstance>(arguments);

            StringBuilder code = new StringBuilder();
            code.AppendFormat("new {0}(", constructor.ReflectedType.Name);

            for (int i = 0; i < argumentList.Count; ++i)
            {
                code.AppendFormat("{0}{1}", GetObjectCreationExpression(argumentList[i]),
                                  i != argumentList.Count - 1 ? ", " : String.Empty);
            }

            code.Append(")");
            return code.ToString();
        }

        public static String GetMethodInvocationStatement(String entityToCallMethodOn, MethodBase method, IEnumerable<IObjectInstance> arguments = null)
        {
            return GetMethodInvocationExpression(entityToCallMethodOn, method, arguments) + ";";
        }
    }
}

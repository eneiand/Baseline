using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Baseline.CodeGeneration.UnitTestCodeGeneration
{
    public class NUnitUnitTestCodeWriter : UnitTestCodeWriter
    {
        private static readonly String TEST_ATTRIBUTE = "[Test]";
        private static readonly String INSTANCE_NAME = "instance";
        private static readonly String RESULT_NAME = "result";

        public override string GetCode(UnitTest test)
        {
            StringBuilder testCode = new StringBuilder();
            testCode.AppendLine(TEST_ATTRIBUTE);
            testCode.AppendLine("public void " + test.Name + "()");
            testCode.AppendLine("{");

            ExceptionThrowingTest exceptionThrowingTest = test as ExceptionThrowingTest;
            MethodTest methodTest = test as MethodTest;
            ConstructorTest constructorTest = test as ConstructorTest;

            if(exceptionThrowingTest != null)
            {
                testCode.Append(GetExceptionThrowingMethodTestCode(exceptionThrowingTest));
            }
            else if(methodTest != null)
            {
                testCode.Append(GetMethodTestCode(methodTest));
            }
            else if(constructorTest != null)
            {
                testCode.Append(GetConstructorTestCode(constructorTest));
            }

            
            testCode.AppendLine("}");
            return testCode.ToString();
        }

        private static String GetConstructorTestCode(ConstructorTest constructorTest)
        {
            StringBuilder testCode = new StringBuilder();
            testCode.Append(TestSuiteGenerator.INDENT);
            testCode.AppendLine(CodeWritingUtils.GetVariableInstantiationStatement(constructorTest.Method as ConstructorInfo, constructorTest.Arguments, INSTANCE_NAME));
            testCode.Append(TestSuiteGenerator.INDENT);
            
            if(constructorTest.Result == null)
            {
                testCode.AppendFormat("Assert.That({0}, Is.Null);", INSTANCE_NAME);
                testCode.AppendLine();
            }
            else
            {
                testCode.AppendFormat("Assert.That({0}, Is.Not.Null);", INSTANCE_NAME);
                testCode.AppendLine();

                String propTestCode = GetPublicFieldsAndPropertiesTestCode(constructorTest.Result);
                if (propTestCode != String.Empty)
                {
                    testCode.Append(TestSuiteGenerator.INDENT);
                    testCode.AppendLine(propTestCode.Replace("\r\n", "\r\n" + TestSuiteGenerator.INDENT));
                }
            }
 
            return testCode.ToString();
        }

        private static String GetExceptionThrowingMethodTestCode(ExceptionThrowingTest exceptionThrowingTest)
        {
            StringBuilder testCode = new StringBuilder();
            
            testCode.Append(TestSuiteGenerator.INDENT);
            testCode.AppendFormat("Assert.Throws<{0}>(()=>", exceptionThrowingTest.Exception.GetType().Name);
            testCode.AppendLine();
            testCode.Append(TestSuiteGenerator.INDENT);
            testCode.AppendLine("{");
            testCode.Append(TestSuiteGenerator.INDENT + TestSuiteGenerator.INDENT);
            testCode.AppendLine(GetInvokeMethodCode(exceptionThrowingTest.Method, exceptionThrowingTest.Instance).Replace("\r\n", "\r\n"+ TestSuiteGenerator.INDENT + TestSuiteGenerator.INDENT));
            testCode.Append(TestSuiteGenerator.INDENT);
            testCode.AppendLine("});");
            
            return testCode.ToString();
        }

        private static String GetMethodTestCode(MethodTest methodTest)
        {
            StringBuilder testCode = new StringBuilder();

            var constructorInfo = methodTest.Method as ConstructorInfo;
            var methodInfo = methodTest.Method as MethodInfo;


            bool methodHasAReturnValue = (constructorInfo != null || !(methodInfo != null && methodInfo.ReturnType == typeof(void)));

            testCode.Append(TestSuiteGenerator.INDENT);
            testCode.Append(GetInvokeMethodCode(methodTest.Method, methodTest.Instance, methodTest.Arguments, methodHasAReturnValue).Replace("\r\n", "\r\n"+ TestSuiteGenerator.INDENT));
            testCode.AppendLine();

            if (methodTest.Instance != null)
            {
                String propTestCode = GetPublicFieldsAndPropertiesTestCode(methodTest.Instance.Instance);
                if (propTestCode != String.Empty)
                {
                    testCode.Append(TestSuiteGenerator.INDENT);
                    testCode.AppendLine(propTestCode);
                }
            }

            if (methodHasAReturnValue)
            {
                if(methodTest.Result != null)
                {
                    testCode.Append(TestSuiteGenerator.INDENT);
                    testCode.AppendFormat("Assert.That({0}, {1});", RESULT_NAME, ObjectInstance.NeedsConstructor(methodTest.Result.GetType()) ?  "Is.Not.Null" : String.Format("Is.EqualTo({0})",CodeWritingUtils.GetObjectCreationExpression(methodTest.Result) ));
                    testCode.AppendLine();

                    String propTestCode = GetPublicFieldsAndPropertiesTestCode(methodTest.Result, RESULT_NAME);
                    if (propTestCode != String.Empty)
                    {
                        testCode.Append(TestSuiteGenerator.INDENT);
                        testCode.AppendLine(propTestCode);
                    }
                }
                else
                {
                    testCode.Append(TestSuiteGenerator.INDENT);
                    testCode.AppendFormat("Assert.That({0}, Is.Null);", RESULT_NAME);
                    testCode.AppendLine();
                }
            }



            return testCode.ToString();
        }

        private static string GetInvokeMethodCode(MethodBase method,  ObjectInstance instance = null, IEnumerable<ObjectInstance> arguments = null, bool methodHasReturnValue = false)
        {
            StringBuilder testCode = new StringBuilder();
            if (!method.IsStatic)
            {
                
                testCode.AppendLine(CodeWritingUtils.GetVariableInstantiationStatement(instance, INSTANCE_NAME));
                testCode.AppendFormat("{0}{1}", methodHasReturnValue ? "var " + RESULT_NAME + " = " : String.Empty,
                                      CodeWritingUtils.GetMethodInvocationStatement(INSTANCE_NAME, method, arguments));
            }

            return testCode.ToString();
        }

        private static string GetPublicFieldsAndPropertiesTestCode(Object result, String instanceName = "instance")
        {
            StringBuilder testCode = new StringBuilder();

            var fields = result.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            for(int i = 0; i < fields.Length; ++i)
            {
                var fieldInfo = fields[i];

                var fieldVal = fieldInfo.GetValue(result);

                if (fieldVal == null)
                {
                    testCode.AppendFormat("Assert.That({0}, Is.Null);", instanceName + "." + fieldInfo.Name);
                }
                else if (ObjectInstance.NeedsConstructor(fieldVal.GetType()))
                {
                    testCode.AppendFormat("Assert.That({0}, Is.Not.Null);", instanceName + "." + fieldInfo.Name);
                    testCode.AppendLine();
                    testCode.Append(GetPublicFieldsAndPropertiesTestCode(fieldVal,
                                                              instanceName + "." + fieldInfo.Name));
                }
                else
                    testCode.AppendFormat("Assert.That({0}.{1}, Is.EqualTo({2}));", instanceName, fieldInfo.Name, CodeWritingUtils.GetObjectCreationExpression(fieldVal));

                    testCode.AppendLine();
            }



            var properties = result.GetType().GetProperties();


            for (int i = 0; i < properties.Length; ++i )
            {
                var propertyInfo = properties[i];
                if (propertyInfo.CanRead)
                {
                    try
                    {
                        Object propertyVal = propertyInfo.GetGetMethod().Invoke(result, null);

                        if(propertyVal == null)
                        {
                            testCode.AppendFormat("Assert.That({0}, Is.Null);", instanceName + "." + propertyInfo.Name);
                        }
                        else if(ObjectInstance.NeedsConstructor(propertyVal.GetType()))
                        {
                            testCode.AppendFormat("Assert.That({0}, Is.Not.Null);", instanceName + "." + propertyInfo.Name);
                            testCode.AppendLine();
                            testCode.Append(GetPublicFieldsAndPropertiesTestCode(propertyVal,
                                                                      instanceName + "." + propertyInfo.Name));
                        }
                        else
                            testCode.AppendFormat("Assert.That({0}.{1}, Is.EqualTo({2}));", instanceName, propertyInfo.Name, CodeWritingUtils.GetObjectCreationExpression(propertyVal));
                    }
                    catch(Exception e)
                    {
                        testCode.AppendFormat("Assert.Throws<{0}>(()=>{1}.{2}));", e.GetType().Name, instanceName, propertyInfo.Name);                        
                    }
                    
                 if(i != properties.Length -1)
                    testCode.AppendLine();
                }
            }

            return testCode.ToString();
        }
    }
}
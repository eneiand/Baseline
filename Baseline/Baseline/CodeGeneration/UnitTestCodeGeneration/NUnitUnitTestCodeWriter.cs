using System;
using System.Reflection;
using System.Text;
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
                testCode.Append(GetPropertiesTestCode(constructorTest.Result));
            }

            
            testCode.AppendLine("}");
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
            testCode.AppendLine(GetInvokeMethodCode(exceptionThrowingTest.Method, exceptionThrowingTest.Instance, indent: TestSuiteGenerator.INDENT + TestSuiteGenerator.INDENT));
            testCode.Append(TestSuiteGenerator.INDENT);
            testCode.AppendLine("});");
            
            return testCode.ToString();
        }

        private String GetMethodTestCode(MethodTest methodTest)
        {
            StringBuilder testCode = new StringBuilder();
            bool methodHasAReturnValue = methodTest.Result != null;


            testCode.Append(GetInvokeMethodCode(methodTest.Method, methodTest.Instance, methodHasAReturnValue));
            testCode.AppendLine();

            if (methodHasAReturnValue)
            {
                testCode.Append(TestSuiteGenerator.INDENT);
                testCode.AppendFormat("Assert.That({0}, Is.Equal.To({1}));", RESULT_NAME,
                                      CodeWritingUtils.GetObjectCreationExpression(methodTest.Result));
                testCode.AppendLine();
            }

            return testCode.ToString();
        }

        private static string GetInvokeMethodCode(MethodBase method,  ObjectInstance instance = null, bool methodHasReturnValue = false, String indent = TestSuiteGenerator.INDENT)
        {
            StringBuilder testCode = new StringBuilder();
            if (!method.IsStatic)
            {
                testCode.Append(indent);
                testCode.AppendLine(CodeWritingUtils.GetVariableInstantiationStatement(instance, INSTANCE_NAME));

                testCode.Append(indent);
                testCode.AppendFormat("{0}{1}", methodHasReturnValue ? "var " + RESULT_NAME + " = " : String.Empty,
                                      CodeWritingUtils.GetMethodInvocationStatement(INSTANCE_NAME, method));
            }

            return testCode.ToString();
        }

        private static string GetPropertiesTestCode(Object result)
        {
            StringBuilder testCode = new StringBuilder();
            var properties = result.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                if(propertyInfo.CanRead)
                {
                    Object propertyVal = propertyInfo.GetGetMethod().Invoke(result, null);

                    testCode.AppendFormat("Assert.That({0}.{1}, Is.Equal.To({2})", RESULT_NAME, propertyInfo.Name, CodeWritingUtils.GetObjectCreationExpression(propertyVal));
                }
            }

            return testCode.ToString();
        }
    }
}
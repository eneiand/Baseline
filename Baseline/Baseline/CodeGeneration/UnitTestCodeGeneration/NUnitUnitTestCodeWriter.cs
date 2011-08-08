using System;
using System.Text;
using Baseline.TestHarness.UnitTests;

namespace Baseline.CodeGeneration.UnitTestCodeGeneration
{
    public class NUnitUnitTestCodeWriter : UnitTestCodeWriter
    {
        private const String TEST_ATTRIBUTE = "[Test]";

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
                testCode.Append(TestSuiteGenerator.INDENT);
                testCode.AppendFormat("Assert.Throws<{0}>(()=>", exceptionThrowingTest.Exception.GetType().Name);
                testCode.AppendLine("{});");
            }
            else if(methodTest != null)
            {
                testCode.Append(TestSuiteGenerator.INDENT);
                
                if(methodTest.Instance != null)
                {
                    testCode.AppendLine(CodeWritingUtils.GetVariableInstantiationCode(methodTest.Instance));

                   
                    
                }


            }
            else if(constructorTest != null)
            {}

            


            testCode.AppendLine("}");
            return testCode.ToString();
        }
    }
}
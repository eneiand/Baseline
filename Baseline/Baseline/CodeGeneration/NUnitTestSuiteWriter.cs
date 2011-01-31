using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;

namespace Baseline.CodeGeneration
{
    public class NUnitTestSuiteWriter : TestSuiteWriter
    {
        protected override void AppendPostAmble(StreamWriter file, TestSuite testSuite)
        {
            throw new NotImplementedException();
        }

        protected override void AppendPreAmble(StreamWriter file, TestSuite testSuite)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTestCode(StreamWriter outputStream, ExceptionThrowingTest exceptionThrowingTest)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTestCode(StreamWriter outputStream, ConstructorTest constructorTest)
        {
            throw new NotImplementedException();
        }

        protected override void GenerateTestCode(StreamWriter outputStream, MethodTest methodTest)
        {
            throw new NotImplementedException();
        }

        protected override string FileExtension
        {
            get { return "cs"; }
            
        }
    }
}

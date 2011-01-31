using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;

namespace Baseline.CodeGeneration
{
    public abstract class TestSuiteWriter
    {
        public void GenerateTestCode(IEnumerable<TestSuite> testSuites) 
        { 
            GenerateTestCode(new DirectoryInfo(Environment.CurrentDirectory), testSuites); 
        }
        public void GenerateTestCode(String outputDir, IEnumerable<TestSuite> testSuites)
        {
            GenerateTestCode(new DirectoryInfo(outputDir), testSuites);
        }

        public void GenerateTestCode(DirectoryInfo outputDir, IEnumerable<TestSuite> testSuites)
        {
            if (outputDir == null) throw new ArgumentNullException("outputDir");
            if (testSuites == null) throw new ArgumentNullException("testSuites");

            foreach (var testSuite in testSuites)
            {
                using (var file = File.CreateText(Path.Combine(outputDir.FullName, testSuite.Type.FullName + "." + this.FileExtension)))
                {
                    AppendPreAmble(file, testSuite);
                    testSuite.Tests.ForEach(u =>
                                                {
                                                    if(u is ExceptionThrowingTest) GenerateTestCode(file, u as ExceptionThrowingTest);
                                                    else if(u is ConstructorTest) GenerateTestCode(file, u as ConstructorTest);
                                                    else if(u is MethodTest) GenerateTestCode(file, u as MethodTest);
                                                    else throw new InvalidOperationException("unrecoginised unit test type " + u.GetType());
                                                });
                    AppendPostAmble(file, testSuite);
                }
            }
        }

        protected abstract void AppendPostAmble(StreamWriter file, TestSuite testSuite);
        protected abstract void AppendPreAmble(StreamWriter file, TestSuite testSuite);

        protected abstract void GenerateTestCode(StreamWriter outputStream, ExceptionThrowingTest exceptionThrowingTest);
        protected abstract void GenerateTestCode(StreamWriter outputStream, ConstructorTest constructorTest);
        protected abstract void GenerateTestCode(StreamWriter outputStream, MethodTest methodTest);


        protected abstract string FileExtension { get; }
    }
}

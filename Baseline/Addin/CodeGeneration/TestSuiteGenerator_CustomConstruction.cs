using System;
using System.Collections.Generic;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness;
using Baseline.TestHarness.UnitTests;
using Baseline.TypeAnalysis.ObjectInstantiation;

namespace Addin.CodeGeneration
{
    public partial class TestSuiteGenerator
    {
        private TestSuite TestSuite { get; set; }
        private HashSet<String> NamespacesRequired { get { return _mNamespacesUsed; } }
        private UnitTestCodeWriter UnitTestCodeWriter { get; set; }
        private string ClassAttribute { get; set; }

        public const String INDENT = "    ";

        private HashSet<String> _mNamespacesUsed = new HashSet<String>(); 

        public TestSuiteGenerator(TestSuite tSuite, UnitTestCodeWriter unitTestCodeWriter, String classAttribute = null)
        {
            this.TestSuite = tSuite;

            CalculateRequiredNamespaces(tSuite);

            UnitTestCodeWriter = unitTestCodeWriter;
            ClassAttribute = classAttribute;
        }

        private void CalculateRequiredNamespaces(TestSuite tSuite)
        {
            _mNamespacesUsed.Add(tSuite.Type.Namespace);
            foreach (var unitTest in tSuite.Tests)
            {
                ExceptionThrowingTest exceptionThrowingTest = unitTest as ExceptionThrowingTest;
                MethodTest methodTest = unitTest as MethodTest;

                if(exceptionThrowingTest != null)
                {
                    _mNamespacesUsed.Add(exceptionThrowingTest.Exception.GetType().Namespace);
                }
                else if (methodTest != null && methodTest.Result != null)
                {
                    _mNamespacesUsed.Add(methodTest.Result.GetType().Namespace);
                }
                
                if(unitTest.Arguments != null)
                    AddArgumentTypes(unitTest.Arguments);
            }
        }

        private void AddArgumentTypes(List<IObjectInstance> arguments)
        {
            foreach (var objectInstance in arguments)
            {
                if (objectInstance is ObjectInstance)
                {
                    this._mNamespacesUsed.Add(objectInstance.Instance.GetType().Namespace);
                    if (objectInstance.CreationData != null && objectInstance.CreationData.HasArguments)
                    {
                        AddArgumentTypes(objectInstance.CreationData.Arguments);
                    }
                }
            }
        }

        public TestSuiteGenerator(TestSuite tSuite, UnitTestCodeWriter unitTestCodeWriter) : this(tSuite, unitTestCodeWriter, "[TestFixture]")
        {

        }
    }
}

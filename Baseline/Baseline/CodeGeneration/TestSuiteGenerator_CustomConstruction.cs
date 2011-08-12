using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Baseline.CodeGeneration.UnitTestCodeGeneration;
using Baseline.TestHarness;

namespace Baseline.CodeGeneration
{
    partial class TestSuiteGenerator
    {
        private TestSuite TestSuite { get; set; }
        private UnitTestCodeWriter UnitTestCodeWriter { get; set; }
        private string ClassAttribute { get; set; }

        public const String INDENT = "    ";

        public TestSuiteGenerator(TestSuite tSuite, UnitTestCodeWriter unitTestCodeWriter, String classAttribute = null)
        {
            this.TestSuite = tSuite;
            UnitTestCodeWriter = unitTestCodeWriter;
            ClassAttribute = classAttribute;
        }
        public TestSuiteGenerator(TestSuite tSuite, UnitTestCodeWriter unitTestCodeWriter) : this(tSuite, unitTestCodeWriter, "[TestFixture]")
        {

        }
    }
}

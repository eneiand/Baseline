using System;
using Baseline.TestHarness.UnitTests;

namespace Baseline.CodeGeneration.UnitTestCodeGeneration
{
    public abstract class UnitTestCodeWriter
    {
        public abstract String GetCode(UnitTest test);

    }
}

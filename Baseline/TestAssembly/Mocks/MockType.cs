using System;
using System.Threading;

namespace TestAssembly.Mocks
{
    public class MockType
    {
        public MockType()
        {}

        public MockType(int i)
        {}

        public MockType(char c)
        {
            throw new ArgumentException("don't use this constructor", "c");
        }

        public void VoidReturnType(){}
        public void VoidReturnTypeIntParameter(Int32 i){}
        public Int32 IntReturnType()
        {
            return 1;  
        }
        public Int32 IntReturnTypeIntParameter(Int32 i)
        {
            return 1;
        }

        public Int32 IntReturnTypeIntCharParameter(Int32 i, Char c)
        {
            return 1;
        }

        public void ExceptionThrower()
        {
            throw new AbandonedMutexException();
        }


    }
}

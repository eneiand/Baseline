using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tests.Mocks
{
    public class MockType
    {
        public MockType()
        {}

        public MockType(int i)
        {}

        private void PrivateMethod()
        {
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

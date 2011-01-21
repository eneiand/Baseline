using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Mocks
{
    class DefaultConstructorTestType
    {
        public DefaultConstructorTestType()
        {

        }
    }

    class PrimitiveTypeConstructorTestType
    {
        public Int32 I
        {
            get; private set; 
        }

        public PrimitiveTypeConstructorTestType(Int32 i)
        {
            I = i;
        }
    }

}

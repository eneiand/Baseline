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

    class DefaultConstructorThrowsAnExceptionTestType
    {
        public DefaultConstructorThrowsAnExceptionTestType()
        {
            throw new NotImplementedException();
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

    class MultiplePrimitiveTypeConstructorTestType
    {
        public Int32 I
        {
            get;
            private set;
        }

        public char C { get; set; }

        public MultiplePrimitiveTypeConstructorTestType(Int32 i, char c)
        {
            I = i;
            C = c;
        }
    }

    class PrimitiveTypeConstructorThrowsAnExceptionTestType
    {

        public PrimitiveTypeConstructorThrowsAnExceptionTestType(Int32 i)
        {
            throw new NotImplementedException();
        }
    }

}

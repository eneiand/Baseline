using System;

namespace TestAssembly.Mocks
{
    public class DefaultConstructorTestType
    {
        public DefaultConstructorTestType()
        {

        }
    }

    public class DefaultConstructorThrowsAnExceptionTestType
    {
        public DefaultConstructorThrowsAnExceptionTestType()
        {
            throw new NotImplementedException();
        }
 
    }

    public class PrimitiveTypeConstructorTestType
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

    public class MultiplePrimitiveTypeConstructorTestType
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

    public class PrimitiveTypeConstructorThrowsAnExceptionTestType
    {

        public PrimitiveTypeConstructorThrowsAnExceptionTestType(Int32 i)
        {
            throw new NotImplementedException();
        }
    }

}

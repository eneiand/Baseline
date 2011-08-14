using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foo
{
    public class SomeOtherType
    {
        public SomeType SomeType { get; private set; }
        public SomeOtherType(SomeType s)
        {
            SomeType = s;
        }
    }
}

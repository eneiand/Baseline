using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foo
{
    public class SomeType
    {

        public string Name { get; private set; }

        public SomeType(String name)
        {
            Name = name;
        }

        public String StringToLower(String s)
        {
            StringBuilder lowerCaseString = new StringBuilder(String.Empty);

            for (int i = 0; i < s.Length; ++i)
                lowerCaseString.Append(s.Substring(i, 1).ToLower() );

            return lowerCaseString.ToString().Trim();
        }

        public Int32 AtoI(String s)
        {
            return Int32.Parse(s);
        }

        public String GetSelectStatement(String table, String col)
        {
            if(String.IsNullOrEmpty(table) || String.IsNullOrEmpty(col)) throw new ArgumentException("table or col is empty");

            return (String.Format("SELECT '{0}' FROM '{1}';", col, table));
        }

        public SomeOtherType MethodThatReturnsNull()
        {
            return null;
        }

        public SomeOtherType MethodThatReturnsSomeOtherType()
        {

            return new SomeOtherType(this);
        }

    }
}

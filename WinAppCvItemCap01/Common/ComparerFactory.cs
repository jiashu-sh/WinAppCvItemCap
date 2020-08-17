using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppCvItemCap01.Common
{
    class ComparerFactory
    {
        public static IComparer<int> GetIntComparer()
        {
            return new IntComparer();
        }

        public class IntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x.CompareTo(y);
            }
        }
    }
}

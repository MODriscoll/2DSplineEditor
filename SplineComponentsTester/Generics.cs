using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplineComponentsTester
{
    public class Generics
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = rhs;
            rhs = lhs;
            lhs = temp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplineComponentsTester
{
    interface ISwappable<T>
    {
        void Swap(ref T other);
    }
}

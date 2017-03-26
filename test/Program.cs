using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            test();
        }

        static void test()
        {
            int[] a = new int[3] { 3, 4, 5 };
            int[] b = new int[3] { 0, 0, 0 };
            b[1] = a[1];
            a[1] = 9;
        }
    }
}

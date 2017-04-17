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
            List<double[,]> one = new List<double[,]>();
            List<double[,]> two = new List<double[,]>();
            double[,] matrix = new double[2, 2] { { 1, 2 }, { 3, 4 } };
            one.Add(matrix);
            matrix = new double[2, 2] { { 9, 8 }, { 7, 6 } };
            two.Add(matrix);

            one[0][0, 0] = 3;
            two[0][0, 0] = 4;
            int k = 0;
        }
    }
}

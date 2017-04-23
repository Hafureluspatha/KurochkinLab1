using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointsGeneration;
using Common;

namespace NN
{
    public class TestDataGenerator
    {
        public static int orFunc(double first, double second)
        {
            return Convert.ToInt32(Convert.ToBoolean(first) || Convert.ToBoolean(second));
        }
        public static int andFunc(double first, double second)
        {
            return Convert.ToInt32(Convert.ToBoolean(first) && Convert.ToBoolean(second));
        }
        public static int xorFunc(double first, double second)
        {
            bool p = Convert.ToBoolean(first);
            bool q = Convert.ToBoolean(second);
            return Convert.ToInt32((p || q) && !(p && q));
        }
        public static MultidimensionalPoint[] InitializeArrayOfPoints(int size)
        {
            MultidimensionalPoint[] result = new MultidimensionalPoint[size];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new MultidimensionalPoint(2, 0);
            }
            return result;
        }
        public static MultidimensionalPoint[] GenerateTestData(int size, Func<double, double, int> func)
        {
            MultidimensionalPoint[] data = InitializeArrayOfPoints(size);
            Random rnd = new Random();
            for (int i = 0; i < data.Length; ++i)
            {
                data[i].coordinates[0] = rnd.Next(2);
                data[i].coordinates[1] = rnd.Next(2);
                data[i].pointClass = func(data[i].coordinates[0], data[i].coordinates[1]);
            }

            return data;
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Common
{
    public class Methods
    {
        static void Main()
        {
        }
        public static void NormalizeData(MultidimensionalPoint[] data, double maxOverallValue)
        {
            double
                maxValue,
                minValue,
                delta = 0;
            // Coordinates
            for (int dimension = 0; dimension < data[0].coordinates.Length; ++dimension)
            {
                maxValue = (-1) * double.MaxValue;
                minValue = double.MaxValue;
                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i].coordinates[dimension] > maxValue)
                        maxValue = data[i].coordinates[dimension];
                    if (data[i].coordinates[dimension] < minValue)
                        minValue = data[i].coordinates[dimension];
                }
                delta = maxValue - minValue;
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i].coordinates[dimension] = (data[i].coordinates[dimension] - minValue - delta / 2) * 2 * maxOverallValue / delta;
                }
            }
            
            // Radiuses
            for (int i = 0; i < data.Length; ++i )
            {
                data[i].radius *= (2 * maxOverallValue / delta);
            }
            for (int i = 0; i < data.Length; ++i)
            {
                for (int j = 0; j < data[0].coordinates.Length; ++j)
                {
                    Debug.Assert(Math.Abs(data[i].coordinates[j]) <= maxOverallValue + 0.001);
                }
            }
        }
    }
}

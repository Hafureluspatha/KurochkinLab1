using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointsGeneration
{
    public class MultidimensionalPoint
    {
        public double[] coordinates;
        public int pointClass;
        public double radius;
        public MultidimensionalPoint()
        {
            coordinates = new double[15];
            pointClass = -1;
            radius = -1;
        }

    }
}

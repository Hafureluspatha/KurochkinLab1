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
        public MultidimensionalPoint(double number)
        {
            coordinates = new double[15];
            for (int i = 0; i < coordinates.Length; ++i )
            {
                coordinates[i] = number;
            }
            pointClass = -1;
            radius = -1;
        }
        public MultidimensionalPoint(MultidimensionalPoint a)
        {
            coordinates = new double[15];
            pointClass = a.pointClass;
            radius = a.radius;
            for(int i = 0; i < coordinates.Length; ++i)
            {
                coordinates[i] = a.coordinates[i];
            }
        }
        public void Add(double number)
        {
            for (int i = 0; i < coordinates.Length; ++i)
            {
                coordinates[i] += number;
            }
        }
        public void Add(double[] vector)
        {
            if (vector.Length != coordinates.Length)
                throw new Exception("Wrong vector length.");
            for (int i = 0; i < coordinates.Length; ++i)
            {
                coordinates[i] += vector[i];
            }
        }
    }
}

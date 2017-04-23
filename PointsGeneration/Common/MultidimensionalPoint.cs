using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MultidimensionalPoint
    {
        public double[] coordinates;
        public int pointClass;
        public double radius;
        public int classifiedAs;

        public MultidimensionalPoint(int dimensions, double valueOfCoordinates)
        {
            coordinates = new double[dimensions];
            for (int i = 0; i < coordinates.Length; ++i )
            {
                coordinates[i] = valueOfCoordinates;
            }
            pointClass = -1;
            radius = -1;
            classifiedAs = -1;
        }
        public MultidimensionalPoint(MultidimensionalPoint a)
        {
            coordinates = new double[a.coordinates.Length];
            pointClass = a.pointClass;
            radius = a.radius;
            classifiedAs = a.classifiedAs;
            for(int i = 0; i < coordinates.Length; ++i)
            {
                coordinates[i] = a.coordinates[i];
            }
        }
        public MultidimensionalPoint(double[] outerSourcePointsnClasswoRadius)
        {
            coordinates = new double[outerSourcePointsnClasswoRadius.Length - 1];
            for (int i = 0; i < outerSourcePointsnClasswoRadius.Length - 1; ++i)
            {
                coordinates[i] = outerSourcePointsnClasswoRadius[i];
            }
            radius = -1;
            pointClass = Convert.ToInt32(outerSourcePointsnClasswoRadius[outerSourcePointsnClasswoRadius.Length - 1]);
            classifiedAs = -1;
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
        public override string ToString()
        {
            StringBuilder overallString = new StringBuilder();
            for (int i = 0; i < coordinates.Length; ++i )
            {
                overallString.Append(coordinates[i].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + ',');
            }
            overallString.Append(pointClass);
            return overallString.ToString();
        }
    }
}

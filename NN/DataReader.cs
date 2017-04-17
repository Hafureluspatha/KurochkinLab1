using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PointsGeneration;
using System.Diagnostics;

namespace NN
{
    public class DataReader
    {
        public MultidimensionalPoint[] ReadData(string path, string[] splitPointsString, char[] splitCoordinatesString)
        {
            StreamReader sr = new StreamReader(path);
            string rawData = sr.ReadToEnd();
            string[] notParsedPoints = rawData.Split(splitPointsString, StringSplitOptions.None);
            MultidimensionalPoint[] result = new MultidimensionalPoint[notParsedPoints.Length];
            // Iterate over all array
            int dimensions = notParsedPoints[0].Count(c => c == ',');
            for (int j = 0; j < notParsedPoints.Length; ++j)
            {
                string[] oneString = notParsedPoints[j].Split(splitCoordinatesString, StringSplitOptions.None);
                for (int i = 0; i < oneString.Length; ++i)
                {
                    oneString[i] = oneString[i].Replace('.', ',');
                }
                double[] oneStringConverted = new double[oneString.Length];
                for (int i = 0; i < oneString.Length; ++i)
                {
                    oneStringConverted[i] = Convert.ToDouble(oneString[i]);
                }
                result[j] = new MultidimensionalPoint(oneStringConverted);
            }
            return result;
        }
    }
}

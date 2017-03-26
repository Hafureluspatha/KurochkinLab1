using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace PointsGeneration
{
    // Расстояние Евклидово
    // TODO: fix ignorance or the between-class distance randomization
    public partial class PointGeneration : Form
    {
        static StringBuilder csv = new StringBuilder();
        static MultidimensionalPoint[] points;
        static string path;
        static int numberOfPoints;
        static double firstRadius;
        static double distanceBetweenClasses;
        static double differenceInRadiuses;
        static double intersectionPercentageInDouble;
        static double intersectionTrustIntervalInRadiuses = 0.2;
        static double intersectionTrustIntervalInClasses = 0.02;
        static double shiftingValueForSeparable = 0.01;
        static double shiftingValueForNonSeparable = 11;

        public PointGeneration()
        {
            InitializeComponent();
            LinearSeparability.SelectedIndex = 0;
            filePath.Text = "generated_data.csv";
            pointsCount.Text = "100";
            multiplicityRadius.Text = "100";
            multiplicityDistance.Text = "0";
            radiusDerivation.Text = "25";
            distanceDifference.Text = "30";
            intersectionPercentage.Text = "10";
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // State now: takes path, firstRadius, numberOfPoints, differenceInRadiuses, distanceBetweenClasses

            path = filePath.Text;
            numberOfPoints = Convert.ToInt32(pointsCount.Text);
            firstRadius = Convert.ToDouble(multiplicityRadius.Text);
            distanceBetweenClasses = Convert.ToDouble(multiplicityDistance.Text);
            differenceInRadiuses = Convert.ToDouble(radiusDerivation.Text)/100;
            intersectionPercentageInDouble = Convert.ToDouble(intersectionPercentage.Text) / 100;

            points = new MultidimensionalPoint[numberOfPoints * 15];
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = new MultidimensionalPoint();
            }

            if(LinearSeparability.SelectedIndex == 0)
            {
                GenerateLinearSeparable();
                WriteToFile(filePath.Text);
            }
            else
            {
                GenerateLinearUnseparable();
                WriteToFile(filePath.Text);
            }
            
        }

        private void GenerateLinearUnseparable ()
        {
            Random random = new Random();

            // In current version first point of class is its center
            int pointClass = 0;
            int firstClassIndex;
            int secondClassIndex;
            //Generating 8 groups of 2
            for (int groupIterator = 0; groupIterator < 8; groupIterator++ )
            {
                // First class

                // Data needed soon
                firstClassIndex = groupIterator * 2 * numberOfPoints; 
                points[firstClassIndex].pointClass = pointClass;
                points[firstClassIndex].radius = GenerateRadius(firstRadius, differenceInRadiuses, random);
                secondClassIndex = firstClassIndex + numberOfPoints;
                points[secondClassIndex].radius = GenerateRadius(firstRadius, differenceInRadiuses, random);

                for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                {
                    points[firstClassIndex].coordinates[i] = random.Next(1000) - 500;
                }
                if (!IsThisCentralPointFarFromOtherClasses(groupIterator, 
                    points[firstClassIndex].radius + points[secondClassIndex].radius)){
                    double[] shiftingVector;
                    shiftingVector = GenerateNewShiftingVector(15, shiftingValueForNonSeparable);
                    do
                    {
                        for(int i = 0; i < points[firstClassIndex].coordinates.Length; ++i)
                        {
                            points[firstClassIndex].coordinates[i] += shiftingVector[i];
                        }

                    } while (!IsThisCentralPointFarFromOtherClasses(groupIterator, 
                        points[firstClassIndex].radius + points[secondClassIndex].radius));
                }

                // Second class
                // Generating certain distance between two classes, so there will be around 20% of interception
                points[secondClassIndex].pointClass = pointClass + 1;
                double intersectionInDouble;
                do
                {
                    for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                    {
                        points[secondClassIndex].coordinates[i] = points[firstClassIndex].coordinates[i] + 
                            random.Next((int)points[firstClassIndex].radius + (int)points[secondClassIndex].radius) ;
                    }
                    intersectionInDouble = GetIntersectionPercentageOfCenters(points[firstClassIndex], points[secondClassIndex]);
                } while (intersectionInDouble < intersectionPercentageInDouble * (1 + intersectionTrustIntervalInRadiuses) &&
                    intersectionInDouble > intersectionPercentageInDouble * (1 - intersectionTrustIntervalInRadiuses));

                GeneratePointsOfClass(firstClassIndex, numberOfPoints, ref pointClass, random);

                // Generating second class points.
                // If we get not what we want, change center of one class and remake its points.
                // Solution should be equal to 0 if no solution is required
                double solution = 0;
                double finalIntersectionPercentage;
                bool thisIsAppropriatePercentage = false;
                do
                {
                    
                    GeneratePointsOfClass(secondClassIndex, numberOfPoints, ref pointClass, random);
                    finalIntersectionPercentage = GetIntersectionPercentageOfClasses(firstClassIndex, secondClassIndex, numberOfPoints);

                    thisIsAppropriatePercentage = finalIntersectionPercentage < intersectionPercentageInDouble * 
                        (1 + intersectionTrustIntervalInClasses) &&
                        finalIntersectionPercentage > intersectionPercentageInDouble * (1 - intersectionTrustIntervalInClasses);
                    if(thisIsAppropriatePercentage)
                    {
                        solution = 0;
                    }
                    else
                    {
                        //solution = FindSolutionForTheSituation();
                    }
                } while (!thisIsAppropriatePercentage);
            }
            
        }

        private void GenerateLinearSeparable ()
        {
            Random random = new Random();

            // In current version first point of class is its center
            int pointClass = 0;
            int trueIndex;

            // First class
            for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
            {
                points[0].coordinates[i] = random.Next(1000) - 500;
            }
            points[0].radius = GenerateRadius(firstRadius, differenceInRadiuses, random);
            points[0].pointClass = pointClass;
            GeneratePointsOfClass(0, numberOfPoints, ref pointClass, random);
            pointClass++;

            // Other classes
            double[] shiftingVector;
            for (int overallIteator = 1; overallIteator < 15; overallIteator++)
            {
                shiftingVector = GenerateNewShiftingVector(15, shiftingValueForSeparable);
                trueIndex = overallIteator * numberOfPoints;
                // Generate central point of a class
                for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                {
                    points[trueIndex].coordinates[i] = points[trueIndex - 1].coordinates[i];
                }
                points[trueIndex].radius = GenerateRadius(firstRadius, differenceInRadiuses, random);
                points[trueIndex].pointClass = pointClass;
                while (DoesThisCentralPointIntersectWithOtherClasses(trueIndex, distanceBetweenClasses))
                {
                    ShiftPoint(points[trueIndex], shiftingVector);
                }

                // Generating all other points inside the group
                GeneratePointsOfClass(trueIndex, numberOfPoints, ref pointClass, random);
                pointClass++;
            }
        }

        private void WriteToFile(string path)
        {
            //var l = points[300].radius + points[400].radius;
            //var k = Distance(points[300], points[400]);
            for (int i = 0; i < points.Length; ++i)
            {
                csv.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}\n",
                    points[i].coordinates[0].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[1].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[2].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[3].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[4].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[5].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[6].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[7].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[8].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[9].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[10].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[11].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[12].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[13].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].coordinates[14].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].radius.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture),
                    points[i].pointClass));
            }
            File.WriteAllText(path, csv.ToString(), Encoding.UTF8);
            MessageBox.Show("done");
        }

        private double Distance(MultidimensionalPoint a, MultidimensionalPoint b)
        {
            double sum = 0;
            for (int i = 0; i < Math.Min(a.coordinates.Length, b.coordinates.Length); ++i )
            {
                sum += Math.Pow(a.coordinates[i] - b.coordinates[i], 2);
            }
            return Math.Sqrt(sum);
        }

        private bool DoesThisCentralPointIntersectWithOtherClasses(int index, double distanceBetweenClasses)
        {
            for(int i = index - numberOfPoints; i >= 0; i-=numberOfPoints)
            {
                if (points[i].radius < 0)
                {
                    throw new Exception("Got wrong non-central point");
                }
                if(Distance(points[i],points[index]) < points[i].radius + points[index].radius + distanceBetweenClasses + 0.00001)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShiftPoint(MultidimensionalPoint p, double[] shiftingVector)
        {
            for(int i = 0; i< p.coordinates.Length; ++i)
            {
                p.coordinates[i] += shiftingVector[i];
            }
        }

        private double[] GenerateNewShiftingVector (int size, double coordinateDifference)
        {
            double[] newShiftingVector = new double[size];
            Random rand = new Random();
            for (int i = 0; i < newShiftingVector.Length; ++i)
            {
                if (rand.Next(1000) - 500 < 0)
                {
                    newShiftingVector[i] = (-1) * coordinateDifference;
                }
                else
                {
                    newShiftingVector[i] = coordinateDifference;
                }
            }

            return newShiftingVector;
        }

        private double GenerateRadius(double initialRadius, double differenceInRadius, Random rand)
        {
            return initialRadius + differenceInRadius * rand.Next(1000) * initialRadius / 1000.0;
        }

        private bool IsThisCentralPointFarFromOtherClasses(int index, double radiusSum)
        {
            for (int i = index - numberOfPoints; i >= 0; i -= numberOfPoints)
            {
                if (points[i].radius < 0)
                {
                    throw new Exception("Got wrong non-central point");
                }
                if (Distance(points[i], points[index]) < Math.Max(2 * (points[i].radius + points[index].radius), distanceBetweenClasses))
                {
                    return false;
                }
            }
            return true;
        }

        private double GetIntersectionPercentageOfCenters(MultidimensionalPoint a, MultidimensionalPoint b)
        {
            if(a.radius <= 0 || b.radius <= 0)
            {
                throw new Exception("Wrong radius");
            }
            double[] result = new double[a.coordinates.Length];
            double aBegin, bBegin, aEnd, bEnd;
            for(int i = 0; i < a.coordinates.Length; ++i)
            {
                aBegin = a.coordinates[i] - a.radius;
                aEnd = a.coordinates[i] + a.radius;
                bBegin = b.coordinates[i] - b.radius;
                bEnd = b.coordinates[i] +b.radius;
                if(aBegin > bBegin)
                {
                    var temp = aBegin;
                    aBegin = bBegin;
                    bBegin = temp;
                    temp = aEnd;
                    aEnd = bEnd;
                    bEnd = temp;
                }
                if(aBegin <  bEnd && bBegin < aEnd)
                {
                    result[i] = aEnd - bBegin;
                }
                else
                {
                    if(aBegin < bBegin && aEnd > bEnd)
                    {
                        result[i] = bEnd - bBegin;
                    }
                    else
                    {
                        result[i] = (-1) * (bBegin - aEnd);
                    }
                }
            }
            double overallResult = 0;
            for(int i = 0; i < result.Length; ++i)
            {
                overallResult += result[i];
            }
            overallResult /= (double)result.Length;

            return overallResult;
        }

        private double GetIntersectionPercentageOfClasses(int indexA, int indexB, int numberOfPointsInClass)
        {
            int pointsInIntersectionCounter = 0;
            for(int i = 0; i < numberOfPoints; ++i)
            {
                if(Distance(points[indexA + i], points[indexB]) < points[indexB].radius)
                {
                    pointsInIntersectionCounter++;
                }
                if(Distance(points[indexB + i], points[indexA]) < points[indexA].radius)
                {
                    pointsInIntersectionCounter++;
                }
            }
            double result = (double)pointsInIntersectionCounter/(numberOfPointsInClass * 2);

            return result;
        }

        private void GeneratePointsOfClass(int startingIndex_, int numberOfPoints_, ref int pointClass_, Random random)
        {
            for (int i = 1; i < numberOfPoints_; ++i)
            {
                points[startingIndex_ + i].pointClass = pointClass_;
                do
                {
                    for (int j = 0; j < points[startingIndex_ + i].coordinates.Length; ++j)
                    {
                        points[startingIndex_ + i].coordinates[j] = points[startingIndex_].coordinates[j] + points[startingIndex_].radius
                            * (random.Next(1000) - 499) / 1000.0;
                    }
                } while (Distance(points[startingIndex_ + i], points[startingIndex_]) >= points[startingIndex_].radius);
            }
        }

        private double FindSolutionForTheSituation(double percentageNeeded, double percentageGot, double trustInterval)
        {
            if (percentageNeeded > percentageGot)
            {
                //find vector
                //find how it will be divided
                //shift center in this direction
            }
            else
            {

            }
        }
    }
}

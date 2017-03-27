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
using System.Diagnostics;


namespace PointsGeneration
{
    // Расстояние Евклидово
    // TODO: fix ignorance or the between-class distance randomization
    //        static double in start can be suppressed
    //        counters. Something should be done. Otherwise I will forget about them.
    //          do something with crossLength function. Do I need it?
    public partial class PointGeneration : Form
    {
        static StringBuilder csv = new StringBuilder();
        static MultidimensionalPoint[] points;
        static string path;
        static int initialNumberOfPoints;
        static double initialFirstRadius;
        static double initialDistanceBetweenClasses;
        static double initialDifferenceInRadiuses;
        static double initialCrossRateInDouble;
        static double crossOkDeltaInRadiuses = 0.2;
        static double crossOkDeltaInClasses = 0.05;
        static double shiftingValueForSeparable = 0.01;
        static double shiftingValueForNonSeparable = 11;

        public PointGeneration()
        {
            InitializeComponent();
            LinearSeparability.SelectedIndex = 1;
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
            initialNumberOfPoints = Convert.ToInt32(pointsCount.Text);
            initialFirstRadius = Convert.ToDouble(multiplicityRadius.Text);
            initialDistanceBetweenClasses = Convert.ToDouble(multiplicityDistance.Text);
            initialDifferenceInRadiuses = Convert.ToDouble(radiusDerivation.Text)/100;
            initialCrossRateInDouble = Convert.ToDouble(intersectionPercentage.Text) / 100;

            points = new MultidimensionalPoint[initialNumberOfPoints * 15];
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
            for (int groupIterator = 0; groupIterator < 7; groupIterator++ )
            {
                // First class

                // Data needed soon
                firstClassIndex = groupIterator * 2 * initialNumberOfPoints; 
                points[firstClassIndex].pointClass = pointClass;
                points[firstClassIndex].radius = GenerateRadius(initialFirstRadius, initialDifferenceInRadiuses, random);
                secondClassIndex = firstClassIndex + initialNumberOfPoints;
                points[secondClassIndex].radius = GenerateRadius(initialFirstRadius, initialDifferenceInRadiuses, random);

                for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                {
                    points[firstClassIndex].coordinates[i] = random.Next(1000) - 500;
                }
                if (!ThisCentralPointIsFarFromOtherClasses(groupIterator, 
                    points[firstClassIndex].radius + points[secondClassIndex].radius)){
                    double[] shiftingVector;
                    shiftingVector = GenerateNewShiftingVector(15, shiftingValueForNonSeparable);
                    do
                    {
                        for(int i = 0; i < points[firstClassIndex].coordinates.Length; ++i)
                        {
                            points[firstClassIndex].coordinates[i] += shiftingVector[i];
                        }

                    } while (!ThisCentralPointIsFarFromOtherClasses(groupIterator, 
                        points[firstClassIndex].radius + points[secondClassIndex].radius));
                }
                GeneratePointsOfClass(firstClassIndex, initialNumberOfPoints, ref pointClass, random);

                // Second class
                // Generating certain distance between two classes, so there will be given percentage of interception
                double intersectionInDouble;
                double distanceBetweenCenters;

                points[secondClassIndex].pointClass = pointClass + 1;
                distanceBetweenCenters = (points[firstClassIndex].radius + points[secondClassIndex].radius) * ( 1 - initialCrossRateInDouble);
                GenerateRandomPointWithSetDistance(firstClassIndex, secondClassIndex, distanceBetweenCenters, random);
                intersectionInDouble = GetCrossRateOfSpheres(points[firstClassIndex], points[secondClassIndex]);
                Debug.Assert(intersectionInDouble < initialCrossRateInDouble * (1 + crossOkDeltaInRadiuses) && 
                    intersectionInDouble > initialCrossRateInDouble * (1 - crossOkDeltaInRadiuses), "IntersectionInDouble");

                // Generating second class points.
                // If we get not what we want, change center of one class and remake its points.
                double finalCrossRate = 0;
                bool thisIsAppropriatePercentage = false;
                int attempts = 0;
                do
                {
                    thisIsAppropriatePercentage = false;
                    for (int i = 0; i < 5; ++i)
                    {
                        GeneratePointsOfClass(secondClassIndex, initialNumberOfPoints, ref pointClass, random);
                        finalCrossRate = GetCrossRateOfClasses(firstClassIndex, secondClassIndex, initialNumberOfPoints);
                        thisIsAppropriatePercentage = finalCrossRate < initialCrossRateInDouble * (1 + crossOkDeltaInClasses) &&
                            finalCrossRate > initialCrossRateInDouble * (1 - crossOkDeltaInClasses);
                        if (thisIsAppropriatePercentage)
                            break;
                    }
                    if(!thisIsAppropriatePercentage)
                    {
                        SolveSituation(firstClassIndex, secondClassIndex, initialCrossRateInDouble, finalCrossRate, crossOkDeltaInClasses);
                    }
                    attempts++;
                } while (!thisIsAppropriatePercentage);
                check(points[0], points[firstClassIndex]);
                pointClass += 2;
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
            points[0].radius = GenerateRadius(initialFirstRadius, initialDifferenceInRadiuses, random);
            points[0].pointClass = pointClass;
            GeneratePointsOfClass(0, initialNumberOfPoints, ref pointClass, random);
            pointClass++;

            // Other classes
            double[] shiftingVector;
            for (int overallIteator = 1; overallIteator < 15; overallIteator++)
            {
                shiftingVector = GenerateNewShiftingVector(15, shiftingValueForSeparable);
                trueIndex = overallIteator * initialNumberOfPoints;
                // Generate central point of a class
                for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                {
                    points[trueIndex].coordinates[i] = points[trueIndex - 1].coordinates[i];
                }
                points[trueIndex].radius = GenerateRadius(initialFirstRadius, initialDifferenceInRadiuses, random);
                points[trueIndex].pointClass = pointClass;
                while (ThisCentralPointIntersectsWithOtherClasses(trueIndex, initialDistanceBetweenClasses))
                {
                    ShiftPoint(points[trueIndex], shiftingVector);
                }

                // Generating all other points inside the group
                GeneratePointsOfClass(trueIndex, initialNumberOfPoints, ref pointClass, random);
                pointClass++;
            }
        }

        private void WriteToFile(string path)
        {
            //var l = points[300].radius + points[400].radius;
            //var k = Distance(points[300], points[400]);
            for (int i = 0; i < points.Length; ++i)
            {
                csv.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}\n",
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

        private bool ThisCentralPointIntersectsWithOtherClasses(int index, double distanceBetweenClasses)
        {
            for(int i = index - initialNumberOfPoints; i >= 0; i-=initialNumberOfPoints)
            {
                Debug.Assert(points[i].radius > 0, "ThisCentralPointIntersectsWithOtherClasses");
                if(Distance(points[i],points[index]) < points[i].radius + points[index].radius + distanceBetweenClasses)
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
            double randResult = 0;
            while(randResult == 0)
            {
                randResult = rand.NextDouble();
            }
            return initialRadius + differenceInRadius * randResult * initialRadius;
        }

        private bool ThisCentralPointIsFarFromOtherClasses(int index, double radiusSum)
        {
            for (int i = index - initialNumberOfPoints; i >= 0; i -= initialNumberOfPoints)
            {
                Debug.Assert(points[i].radius > 0, "ThisCentralPointIsFarFromOtherClasses");
                if (Distance(points[i], points[index]) < Math.Max(2 * (points[i].radius + points[index].radius), initialDistanceBetweenClasses))
                {
                    return false;
                }
            }
            return true;
        }
        
        private double GetCrossRateOfSpheres(MultidimensionalPoint a, MultidimensionalPoint b)
        {
            Debug.Assert(a.radius > 0 && b.radius > 0);

            double midResult = GetCrossLength(a, b);
            double finalResult = Math.Max(0, midResult /= Distance(a,b));

            return finalResult;
        }

        private double GetCrossRateOfClasses(int indexA, int indexB, int numberOfPointsInClass)
        {
            int pointsInIntersectionCounter = 0;
            double minDistComparedToRadius = double.MaxValue;
            for(int i = 0; i < initialNumberOfPoints; ++i)
            {
                double distAiToB = Distance(points[indexA + i], points[indexB]);
                double distBiToA = Distance(points[indexB + i], points[indexA]);

                // TODO this is for test
                if (distAiToB - points[indexB].radius < minDistComparedToRadius)
                {
                    minDistComparedToRadius = distAiToB - points[indexB].radius;
                }
                if (distBiToA - points[indexA].radius < minDistComparedToRadius)
                {
                    minDistComparedToRadius = distBiToA - points[indexA].radius;
                }

                if(distAiToB < points[indexB].radius)
                {
                    pointsInIntersectionCounter++;
                }
                if(distBiToA < points[indexA].radius)
                {
                    pointsInIntersectionCounter++;
                }
            }
            double result = (double)pointsInIntersectionCounter/(numberOfPointsInClass * 2);

            return result;
        }

        private double GetCrossLength(MultidimensionalPoint a, MultidimensionalPoint b)
        {
            Debug.Assert(a.radius > 0 && b.radius > 0);
            double result = Math.Min(Math.Min(a.radius, b.radius), a.radius + b.radius - Distance(a, b));
            return result;
        }

        private void GeneratePointsOfClass(int startingIndex_, int numberOfPoints_, ref int pointClass_, Random random)
        {
            double distanceFromCenter = 0;
            for(int i = 1; i < numberOfPoints_; ++i)
            {
                // Set random distance < radius
                distanceFromCenter = random.NextDouble() * points[startingIndex_].radius;
                GenerateRandomPointWithSetDistance(startingIndex_, startingIndex_ + i, distanceFromCenter, random);
                points[startingIndex_ + i].pointClass = pointClass_;
            }
        }

        private void GenerateRandomPointWithSetDistance(int anchorPointIndex, int generatedPointIndex, double setDistance, Random random)
        {
            // Here it would be sqrt(n), where n = dimensions
            double averageCoordinate = setDistance / Math.Sqrt(15);
            for (int i = 0; i < points[generatedPointIndex].coordinates.Length; ++i)
            {
                points[generatedPointIndex].coordinates[i] = points[anchorPointIndex].coordinates[i] + averageCoordinate;
            }
            int indexTake = 0,
                indexGive = 0;
            var generatedArray = points[generatedPointIndex].coordinates;
            var anchorArray = points[anchorPointIndex].coordinates;
            double 
                ResultForTake, 
                ResultForGive,
                x0,
                x1,
                c;
            // Randomly jump 40(?) times
            for (int j = 0; j < 40; j++)
            {
                indexTake = random.Next(generatedArray.Length);
                indexGive = random.Next(generatedArray.Length);

                // Taking zone
                x0 = anchorArray[indexTake];
                x1 = generatedArray[indexTake];
                c = random.NextDouble() * Math.Pow(anchorArray[indexTake] - generatedArray[indexTake], 2);
                if (x0 >= x1) {
                    ResultForTake = x0 - Math.Sqrt(Math.Pow(x0 - x1, 2) - c);
                }
                else {
                    ResultForTake = x0 + Math.Sqrt(Math.Pow(x0 - x1, 2) - c);
                }
                generatedArray[indexTake] = ResultForTake;

                //Giving zone
                x0 = anchorArray[indexGive];
                x1 = generatedArray[indexGive];
                if (x0 >= x1) {
                    ResultForGive = x0 - Math.Sqrt(Math.Pow(x0 - x1, 2) + c);
                }
                else {
                    ResultForGive = x0 + Math.Sqrt(Math.Pow(x0 - x1, 2) + c);
                }
                generatedArray[indexGive] = ResultForGive;
            }
            var k = Distance(points[anchorPointIndex], points[generatedPointIndex]);
        }
        
        private void check(MultidimensionalPoint a, MultidimensionalPoint b)
        {
            double some = Distance(a, b) - (a.radius + b.radius);
        }

        private void SolveSituation(int indexOf1stCenter, int indexOf2ndCenter, double percentageNeeded, double percentageGot, double trustInterval)
        {
            // Moving second to first.
            double crossingLength;
            double distance = Distance(points[indexOf1stCenter], points[indexOf2ndCenter]);
            crossingLength = GetCrossLength(points[indexOf1stCenter], points[indexOf2ndCenter]);
            double newCrossingLength = 0;
            if (percentageGot < percentageNeeded * (1 - trustInterval))
            {
                if (crossingLength > 0)
                {
                    // TODO. Is this right number?
                    newCrossingLength = Math.Min(Math.Min(crossingLength * 1.01, points[indexOf1stCenter].radius), points[indexOf2ndCenter].radius);
                }
                else
                {
                    newCrossingLength = 0.1;
                }
            }
            else
            {
                newCrossingLength *= 0.5;
            }
            
            Debug.Assert(newCrossingLength < Math.Min(points[indexOf1stCenter].radius, points[indexOf2ndCenter].radius), "Wrong crossingLength!");
            double deltaCrossingLength = newCrossingLength - crossingLength;
            double desiredDistance = distance - deltaCrossingLength;

            // Finding normalized vector of movement
            double[] movingVector = new double[points[indexOf1stCenter].coordinates.Length];
            double sqrtedDistanse = Math.Sqrt(distance);
            for (int i = 0; i < points[indexOf1stCenter].coordinates.Length; ++i)
            {
                movingVector[i] = (points[indexOf1stCenter].coordinates[i] - points[indexOf2ndCenter].coordinates[i]) / sqrtedDistanse;
            }

            //Move
            for (int i = 0; i < points[indexOf2ndCenter].coordinates.Length; ++i )
            {
                points[indexOf2ndCenter].coordinates[i] += movingVector[i] * 0.2 * deltaCrossingLength;
            }
        }
    }
}

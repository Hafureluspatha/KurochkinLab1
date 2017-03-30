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
    // Пересечение - высчитывается вхождение класса малого радиуса в класс большего радиуса. Классы пересекаются по двое.
    // TODO:  need dimension tolerance
    public partial class PointGeneration : Form
    {
        static StringBuilder csv = new StringBuilder();
        static MultidimensionalPoint[] points;
        static string path;
        static int userPointsCountInClass;
        static double userFirstRadius;
        static double userDistBetweenClasses;
        static double userDiffInRadiuses;
        static double userDistBetweenClassesDiff;
        static double userCrossRateInDouble;
        static double crossOkDeltaInClasses;
        static double shiftingValueForSeparable = 0.01;
        static double shiftingValueForNonSeparable = 1;
        static Stopwatch watch = new Stopwatch();
        static bool thoroughGenerationOfPoints = false;
        static int userNumberOfDimensions;

        public PointGeneration()
        {
            InitializeComponent();
            LinearSeparability.SelectedIndex = 1;
            filePath.Text = "generated_data.csv";
            pointsCount.Text = "100";
            dimensions.Text = "15";
            multiplicityRadius.Text = "100";
            multiplicityDistance.Text = "0";
            radiusDerivation.Text = "25";
            distanceDifference.Text = "30";
            intersectionPercentage.Text = "50";
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            watch.Restart();

            path = filePath.Text;
            userPointsCountInClass = Convert.ToInt32(pointsCount.Text);
            userFirstRadius = Convert.ToDouble(multiplicityRadius.Text);
            userDistBetweenClasses = Convert.ToDouble(multiplicityDistance.Text);
            userDiffInRadiuses = Convert.ToDouble(radiusDerivation.Text) / 100;
            userDistBetweenClassesDiff = Convert.ToDouble(distanceDifference.Text) / 100;
            userCrossRateInDouble = Convert.ToDouble(intersectionPercentage.Text) / 100;
            crossOkDeltaInClasses = Math.Max(0.01, 1.0/userPointsCountInClass);
            userNumberOfDimensions = Convert.ToInt32(dimensions.Text);

            points = new MultidimensionalPoint[userPointsCountInClass * 15];
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
                firstClassIndex = groupIterator * 2 * userPointsCountInClass; 
                points[firstClassIndex].pointClass = pointClass;
                points[firstClassIndex].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
                secondClassIndex = firstClassIndex + userPointsCountInClass;
                points[secondClassIndex].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);

                for (int i = 0; i < points[firstClassIndex].coordinates.Length; ++i)
                {
                    points[firstClassIndex].coordinates[i] = random.Next(1000) - 500;
                }
                if (!ThisCentralPointIsFarFromOtherClasses(groupIterator, userPointsCountInClass))
                {
                    double[] shiftingVector;
                    shiftingVector = GenerateNewShiftingVector(15, shiftingValueForNonSeparable);
                    do
                    {
                        for(int i = 0; i < points[firstClassIndex].coordinates.Length; ++i)
                        {
                            points[firstClassIndex].coordinates[i] += shiftingVector[i];
                        }

                    } while (!ThisCentralPointIsFarFromOtherClasses(groupIterator, userPointsCountInClass));
                }
                GeneratePointsOfClass(firstClassIndex, pointClass, random);

                // Second class
                // Overall idea: generating center in the same place as 1st. Moving it till get right interseption.
                pointClass++;
                points[secondClassIndex].pointClass = pointClass;
                for (int i = 0; i < points[secondClassIndex].coordinates.Length; ++i )
                {
                    points[secondClassIndex].coordinates[i] = points[firstClassIndex].coordinates[i];
                }
                GeneratePointsOfClass(secondClassIndex, pointClass, random);
                double initialCrossRate = GetCrossRateOfClassPoints(firstClassIndex, secondClassIndex, userPointsCountInClass);
                Debug.Assert(initialCrossRate > 0.8, "InitialCrossRate is really low");

                // Defining where the zero interseption is located
                double[] movingVector = GenerateNewShiftingVector(15, shiftingValueForNonSeparable);
                bool overallCrossRateIsOk = false;
                do{
                    points[secondClassIndex].Add(movingVector);
                    overallCrossRateIsOk = true;
                    for (int i = 0; i < 5; ++i )
                    {
                        GeneratePointsOfClass(secondClassIndex, pointClass, random);
                        if(GetCrossRateOfClassPoints(firstClassIndex, secondClassIndex, userPointsCountInClass) > 0)
                            overallCrossRateIsOk = false;
                    }
                    if (watch.Elapsed.Seconds > 5) pointClass = pointClass;
                } while (!overallCrossRateIsOk);
                BinarySearch(movingVector, firstClassIndex, secondClassIndex, pointClass, random);
                pointClass++;
            }
            
        }

        private void GenerateLinearSeparable ()
        {
            Random random = new Random();

            // First point of class is its center
            int pointClass = 0;
            int trueIndex;

            // First class
            for (int i = 0; i < points[0].coordinates.Length; ++i)
            {
                points[0].coordinates[i] = random.Next(1000) - 500;
            }
            points[0].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
            points[0].pointClass = pointClass;
            GeneratePointsOfClass(0, pointClass, random);
            pointClass++;

            // Other classes
            double[] shiftingVector;
            for (int overallIteator = 1; overallIteator < 15; overallIteator++)
            {
                shiftingVector = GenerateNewShiftingVector(15, shiftingValueForSeparable);
                trueIndex = overallIteator * userPointsCountInClass;
                // Generate central point of a class
                for (int i = 0; i < points[trueIndex].coordinates.Length; ++i)
                {
                    points[trueIndex].coordinates[i] = points[trueIndex - 1].coordinates[i];
                }
                points[trueIndex].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
                points[trueIndex].pointClass = pointClass;
                while (ThisClassCrossesOtherClasses(trueIndex, userDistBetweenClasses, userDistBetweenClassesDiff, random))
                {
                    ShiftPoint(points[trueIndex], shiftingVector);
                }

                // Generating all other points inside the group
                GeneratePointsOfClass(trueIndex, pointClass, random);
                pointClass++;
            }
        }

        private void WriteToFile(string path)
        {
            //ValidateData();
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
            TimeSpan time = watch.Elapsed;
            watch.Stop();
            status.Text = String.Format("Done in {0} ms", time.Milliseconds);
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

        private bool ThisClassCrossesOtherClasses(int index, double distanceBetweenClasses, double distanceDifference, Random rand)
        {
            double actualDistanceBetweenClasses = distanceBetweenClasses * (1 + rand.NextDouble() * distanceDifference * distanceBetweenClasses);
            for(int i = index - userPointsCountInClass; i >= 0; i-=userPointsCountInClass)
            {
                Debug.Assert(points[i].radius > 0, "ThisCentralPointIntersectsWithOtherClasses");
                if (Distance(points[i], points[index]) < points[i].radius + points[index].radius + actualDistanceBetweenClasses)
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

        private bool ThisCentralPointIsFarFromOtherClasses(int index, int pointsInClass)
        {
            for (int i = index - pointsInClass; i >= 0; i -= pointsInClass)
            {
                Debug.Assert(points[i].radius > 0, "ThisCentralPointIsFarFromOtherClasses");
                if (Distance(points[i], points[index]) < 3 * (points[i].radius + points[index].radius))
                {
                    return false;
                }
            }
            return true;
        }

        private double GetCrossRateOfClassPoints(int indexA, int indexB, int numberOfPointsInClass)
        {
            int pointsInIntersectionCounter = 0;
            int minRadiusClassIndex;
            int otherRadiusClassIndex;
            if (points[indexA].radius <= points[indexB].radius){
                minRadiusClassIndex = indexA;
                otherRadiusClassIndex = indexB;
            }
            else{
                minRadiusClassIndex = indexB;
                otherRadiusClassIndex = indexA;
            }
            double actualClassRadius = CalculateActualRadiusOfClass(otherRadiusClassIndex);
            for(int i = 0; i < userPointsCountInClass; ++i)
            {
                double distMinToOther = Distance(points[minRadiusClassIndex + i], points[otherRadiusClassIndex]);

                if (distMinToOther < actualClassRadius)
                {
                    pointsInIntersectionCounter++;
                }
            }
            double result = (double)pointsInIntersectionCounter / numberOfPointsInClass;

            return result;
        }

        private void GeneratePointsOfClass(int startingIndex_, int pointClass_, Random random)
        {
            int sign = 1;
            if (thoroughGenerationOfPoints)
            {
                for(int i = 1; i < userPointsCountInClass; ++i)
                {
                    do 
                    {
                        for(int j = 0; j < points[startingIndex_].coordinates.Length; ++j)
                        {
                            sign = (random.NextDouble() > 0.5) ? 1 : -1;
                            points[startingIndex_ + i].coordinates[j] = points[startingIndex_].coordinates[j] + sign * random.NextDouble() * points[startingIndex_].radius;
                        }
                    }while(Distance(points[startingIndex_], points[startingIndex_ + i]) > points[startingIndex_].radius);
                }
            }
            else
            {
                MultidimensionalPoint a = new MultidimensionalPoint(0);
                MultidimensionalPoint b = new MultidimensionalPoint(points[startingIndex_].radius);
                double dist = Distance(a, b);
                double transformingValue = points[startingIndex_].radius / dist;

                for (int j = 1; j < userPointsCountInClass; ++j)
                {
                    points[startingIndex_ + j].pointClass = pointClass_;
                    for (int i = 0; i < points[startingIndex_].coordinates.Length; ++i)
                    {
                        sign = (random.NextDouble() > 0.5) ? 1 : -1;
                        points[startingIndex_ + j].coordinates[i] = sign * random.NextDouble() * points[startingIndex_].radius * transformingValue + points[startingIndex_].coordinates[i];
                    }
                    Debug.Assert(Distance(points[startingIndex_ + j], points[startingIndex_]) <= points[startingIndex_].radius);
                }
            }
        }
        
        private void BinarySearch(double[] shiftingVector, int firstClassIndex_, int secondClassIndex_, int pointClass_, Random rand_)
        {
            double[] first = new double[points[firstClassIndex_].coordinates.Length];
            double[] last = new double[points[firstClassIndex_].coordinates.Length];
            points[firstClassIndex_].coordinates.CopyTo(first, 0);
            points[secondClassIndex_].coordinates.CopyTo(last, 0);

            double crossRate = GetCrossRateOfClassPoints(firstClassIndex_, secondClassIndex_, userPointsCountInClass);
            bool crossRateIsOk = crossRate >= userCrossRateInDouble - crossOkDeltaInClasses && crossRate <= userCrossRateInDouble + crossOkDeltaInClasses;
            while(!crossRateIsOk)
            {
                for(int i = 0; i < last.Length; ++i)
                {
                    points[secondClassIndex_].coordinates[i] = first[i] + (last[i] - first[i]) / 2;
                }
                for(int i = 0; i < 10; ++i)
                {
                    GeneratePointsOfClass(secondClassIndex_, pointClass_, rand_);
                    crossRate = GetCrossRateOfClassPoints(firstClassIndex_, secondClassIndex_, userPointsCountInClass);
                    crossRateIsOk = crossRate >= userCrossRateInDouble - crossOkDeltaInClasses && crossRate <= userCrossRateInDouble + crossOkDeltaInClasses;
                    if (crossRateIsOk)
                        break;
                }
                if(!crossRateIsOk)
                {
                    if (crossRate < userCrossRateInDouble - crossOkDeltaInClasses)
                    {
                        points[secondClassIndex_].coordinates.CopyTo(last, 0);
                    }
                    else
                    {
                        Debug.Assert(crossRate > userCrossRateInDouble + crossOkDeltaInClasses);
                        points[secondClassIndex_].coordinates.CopyTo(first, 0);
                    }
                }
                if (watch.Elapsed.Seconds > 7) pointClass_ = pointClass_;
            }

        }

        private double CalculateActualRadiusOfClass(int classIndex)
        {
            double dist;
            double result = 0;
            for (int j = 0; j < userPointsCountInClass; ++j)
            {
                dist = Distance(points[classIndex], points[classIndex + j]);
                if (dist > result)
                    result = dist;
            }

            return result;
        }

        private void ValidateData()
        {
            double result = GetCrossRateOfClassPoints(0, 100, userPointsCountInClass);
            if(result < 0.3) result=result;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 17; ++i)
            {
                GenerateButton_Click(sender, e);
            }
            MessageBox.Show("Success");
        }
    }
}

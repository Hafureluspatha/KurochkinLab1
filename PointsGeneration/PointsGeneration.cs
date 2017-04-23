using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using Common;
using Visualizer;


namespace PointsGeneration
{
    // Расстояние Евклидово
    // Пересечение - высчитывается вхождение класса малого радиуса в класс большего радиуса. Классы пересекаются по двое.
    public partial class PointGeneration : Form
    {
        public static MultidimensionalPoint[] points = new MultidimensionalPoint[1500];
        static string path = "generated_data.csv";
        static int userPointsCountInClass = 100;
        static int userNumberOfDimensions = 2;
        static double userFirstRadius = 100;
        static double userDistBetweenClasses = 0;
        static double userDiffInRadiuses = 0.3;
        static double userDistBetweenClassesDiff = 0.3;
        static double userCrossRateInDouble = 0.7;
        static int userNumberOfClassesToGenerate = 5;
        static double crossOkDeltaInClasses = 0.01;
        static double shiftingValueForSeparable = 0.01;
        static double shiftingValueForNonSeparable = 5;
        static Stopwatch watch = new Stopwatch();
        static bool thoroughGenerationOfPoints = false;
        static bool needLoneClassInSeparable = false;
        static bool needAllNonSeparableCrossed = false;
        static bool needLoneClassInNonSeparable = false;

        public PointGeneration()
        {
            InitializeComponent();
            LinearSeparability.SelectedIndex = 0;
            filePath.Text = "generated_data.csv";
            classCount.Text = "5";
            pointsCount.Text = "100";
            dimensions.Text = "2";
            multiplicityRadius.Text = "100";
            multiplicityDistance.Text = "200";
            radiusDerivation.Text = "25";
            distanceDifference.Text = "30";
            intersectionPercentage.Text = "70";
        }

        public void GenerateButton_Click(object sender, EventArgs e)
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
            userNumberOfClassesToGenerate = Convert.ToInt32(classCount.Text);
            needLoneClassInSeparable = loneClass.Checked;
            needAllNonSeparableCrossed = allCrossed.Checked;
            needLoneClassInNonSeparable = allCrossedBar1.Checked;
            
            if(LinearSeparability.SelectedIndex == 0) 
                { GenerateLinearSeparable(); }
            else
            {
                if (needAllNonSeparableCrossed || needLoneClassInNonSeparable) 
                    { GenerateAllCrossedLinearUnseparable(); }
                else 
                { GenerateLinearUnseparable(); }
            }
            WriteToFile(filePath.Text);
        }

        public void GenerateLinearUnseparable()
        {
            InitializeDataArray(14);
            Random random = new Random();

            // In current version first point of class is its center
            int pointClass = 0;
            int firstClassIndex;
            int secondClassIndex;
            //Generating 7 groups of 2
            for (int groupIterator = 0; groupIterator < 7 ; groupIterator++ )
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
                    points[firstClassIndex].coordinates[i] = random.NextDouble() * 1000 - 500;
                }
                if (!ThisCentralPointIsFarFromOtherClasses(firstClassIndex, userPointsCountInClass, 3))
                {
                    double[] shiftingVector;
                    shiftingVector = GenerateNewShiftingVector(userNumberOfDimensions, shiftingValueForNonSeparable, random);
                    do
                    {
                        for(int i = 0; i < points[firstClassIndex].coordinates.Length; ++i)
                        {
                            points[firstClassIndex].coordinates[i] += shiftingVector[i];
                        }
                        if (watch.Elapsed.Seconds > 5) pointClass = pointClass;
                    } while (!ThisCentralPointIsFarFromOtherClasses(firstClassIndex, userPointsCountInClass, 3));
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
                double[] movingVector = GenerateNewShiftingVector(userNumberOfDimensions, shiftingValueForNonSeparable, random);
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

        public void GenerateAllCrossedLinearUnseparable()
        {
            if (needLoneClassInNonSeparable)
                InitializeDataArray(15);
            else
                InitializeDataArray(14);

            Random random = new Random();
            int index = 0;
            for (int i = 0; i < 14; ++i)
            {
                index  = i * userPointsCountInClass;
                points[index].pointClass = i;
                points[index].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
                for(int j = 0; j < points[index].coordinates.Length; ++j)
                {
                    points[index].coordinates[j] = random.NextDouble() * userFirstRadius * 0.1;
                }
                GeneratePointsOfClass(index, i, random);
            }
            if (needLoneClassInNonSeparable)
            {
                int loneClassIndex = 14 * userPointsCountInClass;
                points[loneClassIndex].pointClass = 14;
                points[loneClassIndex].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
                for (int i = 0; i < points[loneClassIndex].coordinates.Length; ++i)
                {
                    points[loneClassIndex].coordinates[i] = points[1].coordinates[i];
                }
                if (!ThisCentralPointIsFarFromOtherClasses(loneClassIndex, userPointsCountInClass, 5))
                {
                    double[] shiftingVector;
                    shiftingVector = GenerateNewShiftingVector(userNumberOfDimensions, shiftingValueForNonSeparable * 5, random);
                    do
                    {
                        for (int i = 0; i < points[loneClassIndex].coordinates.Length; ++i)
                        {
                            points[loneClassIndex].coordinates[i] += shiftingVector[i];
                        }
                    } while (!ThisCentralPointIsFarFromOtherClasses(loneClassIndex, userPointsCountInClass, 5));
                }
                GeneratePointsOfClass(loneClassIndex, 14, random);
            }
        }

        public void GenerateLinearSeparable()
        {
            InitializeDataArray(15);
            Random random = new Random();

            // First point of class is its center
            int pointClass = 0;
            int trueIndex;

            // First class
            for (int i = 0; i < points[0].coordinates.Length; ++i)
            {
                points[0].coordinates[i] = random.NextDouble() * 1000 - 500;
            }
            points[0].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
            points[0].pointClass = pointClass;
            GeneratePointsOfClass(0, pointClass, random);
            pointClass++;

            // Other classes
            double[] shiftingVector;
            for (int overallIteator = 1; overallIteator < 15; overallIteator++)
            {
                shiftingVector = GenerateNewShiftingVector(userNumberOfDimensions, shiftingValueForSeparable, random);
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
                    points[trueIndex].Add(shiftingVector);
                }

                // Generating all other points inside the group
                GeneratePointsOfClass(trueIndex, pointClass, random);
                pointClass++;
            }
            if(needLoneClassInSeparable)
            {
                pointClass--;
                shiftingVector = GenerateNewShiftingVector(userNumberOfDimensions, shiftingValueForSeparable, random);
                trueIndex = 14 * userPointsCountInClass;
                // Generate central point of a class
                for (int i = 0; i < points[trueIndex].coordinates.Length; ++i)
                {
                    points[trueIndex].coordinates[i] = points[trueIndex - 1].coordinates[i];
                }
                points[trueIndex].radius = GenerateRadius(userFirstRadius, userDiffInRadiuses, random);
                points[trueIndex].pointClass = pointClass;
                while (!ThisCentralPointIsFarFromOtherClasses(trueIndex, userPointsCountInClass, 10))
                {
                    points[trueIndex].Add(shiftingVector);
                }

                // Generating all other points inside the group
                GeneratePointsOfClass(trueIndex, pointClass, random);
            }
        }

        public void InitializeDataArray(int size)
        {
            points = new MultidimensionalPoint[userPointsCountInClass * size];
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = new MultidimensionalPoint(userNumberOfDimensions, 0);
            }
        }

        public void WriteToFile(string path)
        {
            StringBuilder csv = new StringBuilder();
            //ValidateData();
            for (int i = 0; i < points.Length - 1; ++i)
            {
                csv.Append(points[i].ToString() + "\n");
            }
            csv.Append(points[points.Length - 1].ToString());
            File.WriteAllText(path, csv.ToString(), Encoding.UTF8);
            watch.Stop();
            status.Text = String.Format("Done in {0} ms", (int)watch.Elapsed.TotalMilliseconds);
        }

        public double Distance(MultidimensionalPoint a, MultidimensionalPoint b)
        {
            double sum = 0;
            Debug.Assert(a.coordinates.Length == b.coordinates.Length);
            for (int i = 0; i < a.coordinates.Length; ++i)
            {
                sum += Math.Pow(a.coordinates[i] - b.coordinates[i], 2);
            }
            return Math.Sqrt(sum);
        }

        public bool ThisClassCrossesOtherClasses(int centerPointIndex, double distanceBetweenClasses, double distanceDifference, Random rand)
        {
            double actualDistanceBetweenClasses = distanceBetweenClasses * (1 + rand.NextDouble() * distanceDifference);
            for(int i = centerPointIndex - userPointsCountInClass; i >= 0; i-=userPointsCountInClass)
            {
                Debug.Assert(points[i].radius > 0, "ThisCentralPointIntersectsWithOtherClasses");
                if (Distance(points[i], points[centerPointIndex]) < points[i].radius + points[centerPointIndex].radius + actualDistanceBetweenClasses)
                {
                    return true;
                }
            }
            return false;
        }
        
        public double[] GenerateNewShiftingVector (int size, double coordinateDifference, Random rand_)
        {
            double[] newShiftingVector = new double[size];
            for (int i = 0; i < newShiftingVector.Length; ++i)
            {
                if (rand_.NextDouble() > 0.5)
                {
                    newShiftingVector[i] = (-1) * coordinateDifference * rand_.Next(10, 20) / 20;
                }
                else
                {
                    newShiftingVector[i] = coordinateDifference * rand_.Next(10, 20) / 20;
                }
            }

            return newShiftingVector;
        }

        public double GenerateRadius(double initialRadius, double differenceInRadius, Random rand)
        {
            double randResult = 0;
            while(randResult == 0)
            {
                randResult = rand.NextDouble();
            }
            return initialRadius + differenceInRadius * randResult * initialRadius;
        }

        public bool ThisCentralPointIsFarFromOtherClasses(int index, int pointsInClass, int threeIsUsuallyFar)
        {
            for (int i = index - pointsInClass; i >= 0; i -= pointsInClass)
            {
                Debug.Assert(points[i].radius > 0, "ThisCentralPointIsFarFromOtherClasses");
                //double oneValue = Distance(points[i], points[index]);
                //double secondValue = threeIsUsuallyFar * (points[i].radius + points[index].radius);
                if (Distance(points[i], points[index]) < threeIsUsuallyFar * (points[i].radius + points[index].radius))
                {
                    return false;
                }
            }
            return true;
        }

        public double GetCrossRateOfClassPoints(int indexA, int indexB, int numberOfPointsInClass)
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

        public void GeneratePointsOfClass(int startingIndex_, int pointClass_, Random random)
        {
            int sign = 1;
            int length = points[startingIndex_].coordinates.Length;
            if (thoroughGenerationOfPoints || userNumberOfDimensions < 4)
            {
                for(int i = 1; i < userPointsCountInClass; ++i)
                {
                    points[startingIndex_ + i].pointClass = pointClass_;
                    do 
                    {
                        for (int j = 0; j < points[startingIndex_].coordinates.Length; ++j)
                        {
                            sign = (random.NextDouble() > 0.5) ? 1 : -1;
                            points[startingIndex_ + i].coordinates[j] = points[startingIndex_].coordinates[j] + sign * random.NextDouble() * points[startingIndex_].radius;
                        }
                        double dist = Distance(points[startingIndex_], points[startingIndex_ + i]);
                        double rad = points[startingIndex_].radius;
                    }while(Distance(points[startingIndex_], points[startingIndex_ + i]) > points[startingIndex_].radius);
                }
            }
            else
            {
                MultidimensionalPoint a = new MultidimensionalPoint(userNumberOfDimensions, 0);
                MultidimensionalPoint b = new MultidimensionalPoint(userNumberOfDimensions, points[startingIndex_].radius);
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
        
        public void BinarySearch(double[] shiftingVector, int firstClassIndex_, int secondClassIndex_, int pointClass_, Random rand_)
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

        public double CalculateActualRadiusOfClass(int classIndex)
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

        public void ValidateData()
        {
            double result = GetCrossRateOfClassPoints(0, 100, userPointsCountInClass);
            if(result < 0.3) result=result;
        }
        
        public void button1_Click(object sender, EventArgs e)
        {
            visualizer form = new visualizer();
            form.Show();
            form.ShowGraph(false, points, false, false, 0.8);
        }
    }
}

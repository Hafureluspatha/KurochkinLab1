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
    //Расстояние Евклидово
    public partial class Form1 : Form
    {
        static StringBuilder csv = new StringBuilder();
        static MultidimensionalPoint[] points;
        static string path;
        static int numberOfPoints;
        static double firstRadius;
        static double distanceBetweenClasses;
        static double differenceInRadiuses;

        public Form1()
        {
            InitializeComponent();
            LinearSeparability.SelectedIndex = 0;
            filePath.Text = "generated_data.csv";
            pointsCount.Text = "100";
            setsPositions.SelectedIndex = 0;
            multiplicityRadius.Text = "100";
            multiplicityDistance.Text = "0";
            radiusDerivation.Text = "25";
            intersectionPercentage.Text = "10";

            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = new MultidimensionalPoint();
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            //State now: takes path, firstRadius, 
            //Ignores: numberOfPoints, distanceBetweenClasses, differenceInRadiuses
            //Solving: numberOfPoints

            path = filePath.Text;
            numberOfPoints = Convert.ToInt32(pointsCount.Text);
            firstRadius = Convert.ToDouble(multiplicityRadius.Text);
            distanceBetweenClasses = Convert.ToDouble(multiplicityDistance.Text);
            differenceInRadiuses = Convert.ToDouble(radiusDerivation.Text)/100;

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
            throw new NotImplementedException();
        }

        private void GenerateLinearSeparable ()
        {
            Random random = new Random();

            //On current version first point of class is its center
            int pointClass = 0;
            int trueIndex;
            for (int overallIteator = 0; overallIteator < 15; overallIteator++ )
            {
                trueIndex = overallIteator * numberOfPoints;
                //Generate central point of a class
                for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                {
                    points[trueIndex].coordinates[i] = random.Next(1000) - 500;
                }
                points[trueIndex].radius = firstRadius + differenceInRadiuses * random.Next(1000) * firstRadius / 1000.0;
                points[trueIndex].pointClass = pointClass;
                while (DoesPointIntersectWithOtherClasses(trueIndex))
                {
                    ShiftPoint(points[trueIndex]);
                }

                //Generating all other points inside the group
                for (int j = 1; j < 100; ++j)
                {
                    points[trueIndex + j].pointClass = pointClass;
                    do
                    {
                        for (int i = 0; i < new MultidimensionalPoint().coordinates.Length; ++i)
                        {
                            points[trueIndex + j].coordinates[i] = points[trueIndex].coordinates[i] + points[trueIndex].radius 
                                * (random.Next(1000) - 499) * 0.5 / 1000.0;
                        }
                    } while (Distance(points[trueIndex + j], points[trueIndex]) >= points[trueIndex].radius);
                }
                pointClass++;
            }
        }

        private void WriteToFile(string path)
        {
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

        private bool DoesPointIntersectWithOtherClasses(int index)
        {
            int j = index / 100;
            for(int i = j-1; i >= 0; --i)
            {
                if(points[i*100].radius < 0)
                {
                    throw new Exception("Got wrong non-central point");
                }
                if(Distance(points[i*100],points[index]) < points[i*100].radius + points[index].radius + 0.00001)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShiftPoint(MultidimensionalPoint p)
        {
            for(int i = 0; i< p.coordinates.Length; ++i)
            {
                p.coordinates[i] += 30;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using PointsGeneration;
using System.Windows.Forms;
using Common;
using PointsGeneration;
using Visualizer;
using System.IO;


namespace NN
{
    public class NeuralNetwork
    {
        private double startingWeight;
        private double outputLength;
        private double alpha;
        private double eta;
        private double inertia;
        private double trainingSetPercentage;
        private double activationFunctionShift;

        private List<Neuron[]> neurons;
        private List<double[,]> matrixList; //from 1 to n-1. ->
        private List<double[,]> deltaMatrixList;
        public MultidimensionalPoint[] data;


        public NeuralNetwork(MultidimensionalPoint[] data, int[] neuronsInLayersCount, double alpha, double activationFunctionShift, 
            double eta, double inertia, double outputLength, double traningSetPercentage, double startingWeight)
        {
            this.startingWeight = startingWeight;
            this.trainingSetPercentage = traningSetPercentage;
            this.outputLength = outputLength;
            this.alpha = alpha;
            this.eta = eta;
            this.inertia = inertia;
            this.activationFunctionShift = activationFunctionShift;

            neurons = new List<Neuron[]>();
            matrixList = new List<double[,]>();
            deltaMatrixList = new List<double[,]>();

            this.data = new MultidimensionalPoint[data.Length];
            data.CopyTo(this.data, 0);
            Random rand = new Random();
            for(int i = 0; i < neuronsInLayersCount.Length - 1; ++i)
            {
                double[,] matrix = new double[neuronsInLayersCount[i], neuronsInLayersCount[i + 1]];
                for (int j = 0; j < matrix.GetLength(0); ++j)
                {
                    for(int k = 0; k < matrix.GetLength(1); ++k)
                    {
                        matrix[j, k] = rand.NextDouble() * startingWeight;
                    }
                }
                matrixList.Add(matrix);
                matrix = new double[neuronsInLayersCount[i], neuronsInLayersCount[i + 1]];
                deltaMatrixList.Add(matrix);
            }
            for(int i = 0; i < neuronsInLayersCount.Length; ++i)
            {
                neurons.Add(new Neuron[neuronsInLayersCount[i]]);
                for(int j = 0; j < neurons[i].Length; ++j)
                {
                    neurons[i][j] = new Neuron();
                }
            }
            Common.Methods.NormalizeData(this.data, outputLength / 2);
        }
        
        public MultidimensionalPoint[] ShuffleArray(MultidimensionalPoint[] info)
        {
            Random rnd = new Random();
            info = info.OrderBy(x => rnd.Next()).ToArray();

            return info;
        }

        public void FeedForward(double[] inputValues)
        {
            Debug.Assert(neurons[0].Length == inputValues.Length);
            for (int i = 0; i < inputValues.Length; ++i)
            {
                neurons[0][i].value = inputValues[i];
            }

            double sum;
            for(int i = 0; i < neurons.Count - 1; ++i)
            {
                for(int j = 0; j < neurons[i+1].Length; ++j)
                {
                    sum = 0;
                    for(int k = 0; k < neurons[i].Length; ++k)
                    {
                        sum += matrixList[i][k, j] * neurons[i][k].value;
                    }
                    neurons[i+1][j].value = ActivationFunction(sum, activationFunctionShift);
                }
            }
        }

        public void WriteClassifiedResult(MultidimensionalPoint[] notClassifiedData)
        {
            for(int i = 0; i < notClassifiedData.Length; ++i)
            {
                notClassifiedData[i].classifiedAs = CalcOneResult(notClassifiedData[i].coordinates);
            }
        }

        public int CalcOneResult(double[] coordinates)
        {
            double maxValue = 0;
            int indexOfMaxValue = -1;
            FeedForward(coordinates);
            for (int j = 0; j < neurons[neurons.Count - 1].Length; ++j)
            {
                if (maxValue < neurons[neurons.Count - 1][j].value) { maxValue = neurons[neurons.Count - 1][j].value; indexOfMaxValue = j; }
            }
            return indexOfMaxValue;
        }

        public int CalcTrueResultForExitNode(MultidimensionalPoint point, int nodeIndex)
        {
            int result = 0;
            if (nodeIndex == point.pointClass) result = 1;
            return result;
        }

        public int CalcTrueOverallResult(MultidimensionalPoint point)
        {
            return point.pointClass;
        }

        public double CalcTrainingErrorRate()
        {
            double errorCounter = 0, errorRate;
            int result, trueResult;
            for(int i = 0; i < data.Length * trainingSetPercentage; ++i)
            {
                result = CalcOneResult(data[i].coordinates);
                trueResult = CalcTrueOverallResult(data[i]);
                if (result != trueResult) errorCounter++;
            }
            errorRate = errorCounter / Math.Truncate(data.Length * trainingSetPercentage);

            return errorRate;
        }

        public double CalcTestingErrorRate()
        {
            double errorCounter = 0, errorRate;
            int result, trueResult;
            for (int i = data.Length - 1; i > data.Length * trainingSetPercentage; --i)
            {
                result = CalcOneResult(data[i].coordinates);
                trueResult = CalcTrueOverallResult(data[i]);
                if (result != trueResult) errorCounter++;
            }
            errorRate = errorCounter / Math.Truncate(data.Length * (1 - trainingSetPercentage));

            return errorRate;
        }

        public double CalcTrainMSE()
        {
            double mseSum = 0, currentMSE = 0;
            for (int i = 0; i < data.Length * trainingSetPercentage; ++i)
            {
                FeedForward(data[i].coordinates);
                currentMSE = 0;
                for (int j = 0; j < neurons[neurons.Count - 1].Length; ++j)
                    currentMSE += Math.Pow(CalcTrueResultForExitNode(data[i], j) - neurons[neurons.Count - 1][j].value, 2);
                currentMSE *= 0.5;
                mseSum += currentMSE;
            }
            mseSum /= Math.Truncate(data.Length * trainingSetPercentage);
            return mseSum;
        }

        public double CalcTestMSE()
        {
            double mseSum = 0, currentMSE = 0;
            for (int i = data.Length - 1; i > data.Length * trainingSetPercentage; --i)
            {
                FeedForward(data[i].coordinates);
                currentMSE = 0;
                for (int j = 0; j < neurons[neurons.Count - 1].Length; ++j)
                    currentMSE += Math.Pow(CalcTrueResultForExitNode(data[i], j) - neurons[neurons.Count - 1][j].value, 2);
                currentMSE *= 0.5;
                mseSum += currentMSE;
            }
            mseSum /= Math.Truncate(data.Length * (1 - trainingSetPercentage));
            return mseSum;
        }

        public void ShowLogMessage(int epochNumber)
        {
            double trainingRate = CalcTrainingErrorRate();
            double testingRate = CalcTestingErrorRate();
            double mseTrain = CalcTrainMSE();
            double mseTest = CalcTestMSE();
            Console.WriteLine("Epoch {0}, Train err {1:0.00}, Test err {2:0.00}, MSETrain {3:0.0000}, MSETest {4:0.0000}", epochNumber, trainingRate, testingRate, mseTrain, mseTest);
        }
        
        public void ShowGraph(bool drawOnlyTest)
        {
            visualizer form = new visualizer();
            form.Show();
            form.ShowGraph(false, data, true, drawOnlyTest, trainingSetPercentage);
        }

        public double TrainNetwork(int epochs, MultidimensionalPoint[] testData)
        {

            StringBuilder mseTrainResult = new StringBuilder();
            StringBuilder mseTestResult = new StringBuilder();
            StringBuilder mseExternalTestResult = new StringBuilder();
            double sum = 0;
            for(int epochsCounter = 0; epochsCounter < epochs; ++epochsCounter)
            {
                data = ShuffleArray(data);
                Debug.Assert(neurons[neurons.Count - 1].Length == 15);
                for (int dataCounter = 0; dataCounter < data.Length * trainingSetPercentage; ++dataCounter)
                {
                    Neuron resultNode;
                    FeedForward(data[dataCounter].coordinates);
                    for(int i = 0; i < neurons[neurons.Count - 1].Length; ++i)
                    {
                        resultNode = neurons[neurons.Count - 1][i];
                        resultNode.delta = resultNode.value * (1 - resultNode.value) * (CalcTrueResultForExitNode(data[dataCounter], i) - resultNode.value);
                    }
                    for(int i = neurons.Count - 2; i > -1; --i)
                    {
                        for(int j = 0; j < neurons[i].Length; ++j)
                        {
                            sum = 0;
                            for(int k = 0; k < neurons[i+1].Length; ++k)
                            {
                                sum += neurons[i + 1][k].delta * matrixList[i][j, k];
                            }
                            neurons[i][j].delta = neurons[i][j].value * (1 - neurons[i][j].value) * sum;
                        }
                    }
                    for(int k = 0; k < matrixList.Count; ++k)
                    {
                        for(int i = 0; i < matrixList[k].GetLength(0); ++i)
                        {
                            for(int j = 0; j < matrixList[k].GetLength(1); ++j)
                            {
                                //deltaMatrixList[k][i, j] = inertia * deltaMatrixList[k][i, j] + (1 - inertia) * eta * neurons[k + 1][j].delta * neurons[k][i].value;
                                matrixList[k][i, j] = matrixList[k][i, j] + eta * neurons[k + 1][j].delta * neurons[k][i].value;
                            }
                        }
                    }
                }
                ShowLogMessage(epochsCounter);
                if (testData != null)
                {
                    Console.WriteLine(" TestDataMSE {0:0.0000}", CalcOverfitMSE(testData));
                    mseExternalTestResult.Append(String.Format("{0}\r\n", CalcOverfitMSE(testData)));
                }
                mseTrainResult.Append(String.Format("{0}\r\n", CalcTrainMSE()));
                mseTestResult.Append(String.Format("{0}\r\n", CalcTestMSE()));
            }
            if (testData != null) File.WriteAllText("mseExternal.txt", mseExternalTestResult.ToString());
            File.WriteAllText("mseTrain.txt", mseTrainResult.ToString());
            File.WriteAllText("mseTest.txt", mseTestResult.ToString());
            Console.WriteLine("done");
            return CalcTrainingErrorRate();
        }

        private double ActivationFunction(double inputValue, double shift)
        {
            double result = 1 / (1 + Math.Exp(-2 * alpha * inputValue + shift));
            return result;
        }

        public static void GoOverfit()
        {
            PointsGeneration.PointGeneration form = new PointsGeneration.PointGeneration();
            form.Hide();
            form.GenerateLinearSeparable();
            MultidimensionalPoint[] testArray = new MultidimensionalPoint[100];
            MultidimensionalPoint[] points = PointsGeneration.PointGeneration.points;
            Random rand = new Random();
            for(int i = 0; i < 5; ++i)
            {
                for(int j = 0; j < 20; ++j)
                {
                    testArray[i * 20 + j] = new MultidimensionalPoint(2, 0);
                    do{
                    testArray[i * 20 + j].coordinates[0] = points[i * 100].coordinates[0] + rand.NextDouble() * points[i*100].radius * 1.5;
                    testArray[i * 20 + j].coordinates[1] = points[i * 100].coordinates[1] + rand.NextDouble() * points[i*100].radius * 1.5;
                    }while(form.Distance(testArray[i * 20 + j], points[i * 100]) > points[i * 100].radius &&
                        form.Distance(testArray[i * 20 + j], points[i * 100]) < points[i * 100].radius * 1.5);
                    testArray[i * 20 + j].pointClass = i;
                }
            }
            NeuralNetwork nn = new NeuralNetwork(points, new int[4] { 2, 15, 15, 15 }, 1, 0, 0.7, 0.01, 2, 1, 0.1);
            nn.TrainNetwork(150, testArray);

        }

        public double CalcOverfitMSE(MultidimensionalPoint[] testData)
        {
            double mseSum = 0, currentMSE = 0;
            for (int i = 0; i < testData.Length; ++i)
            {
                FeedForward(testData[i].coordinates);
                currentMSE = 0;
                for (int j = 0; j < neurons[neurons.Count - 1].Length; ++j)
                    currentMSE += Math.Pow(CalcTrueResultForExitNode(testData[i], j) - neurons[neurons.Count - 1][j].value, 2);
                currentMSE *= 0.5;
                mseSum += currentMSE;
            }
            mseSum /= (double)testData.Length;
            return mseSum;
        }
    }
}

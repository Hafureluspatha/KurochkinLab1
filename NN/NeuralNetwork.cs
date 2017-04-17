using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using PointsGeneration;

namespace NN
{
    public class NeuralNetwork
    {
        private double startingWeight;
        private double outputLength;
        private double alpha;
        private double eta;
        private double inertia;
        private double numberOfClasses;
        private double trainingSetPercentage;
        private double activationFunctionShift;

        private List<Neuron[]> neurons;
        private List<double[,]> matrixList; //from 1 to n-1. ->
        private List<double[,]> deltaMatrixList;
        private MultidimensionalPoint[] data;


        public NeuralNetwork(MultidimensionalPoint[] externalData, int externalNumberOfClasses, int[] externalNeuronsInLayersCount, double externalApha, double externalActivationFunctionShift, 
            double externalEta, double externalInertia, double externalOutputLength, double externalTraningSetPercentage, double externalStartingWeight)
        {
            startingWeight = externalStartingWeight;
            trainingSetPercentage = externalTraningSetPercentage;
            outputLength = externalOutputLength;
            numberOfClasses = externalNumberOfClasses;
            alpha = externalApha;
            eta = externalEta;
            inertia = externalInertia;
            activationFunctionShift = externalActivationFunctionShift;

            neurons = new List<Neuron[]>();
            matrixList = new List<double[,]>();
            deltaMatrixList = new List<double[,]>();

            data = new MultidimensionalPoint[externalData.Length];
            externalData.CopyTo(data, 0);
            Random rand = new Random();
            for(int i = 0; i < externalNeuronsInLayersCount.Length - 1; ++i)
            {
                double[,] matrix = new double[externalNeuronsInLayersCount[i], externalNeuronsInLayersCount[i + 1]];
                for (int j = 0; j < matrix.GetLength(0); ++j)
                {
                    for(int k = 0; k < matrix.GetLength(1); ++k)
                    {
                        matrix[j, k] = rand.NextDouble() * startingWeight;
                    }
                }
                matrixList.Add(matrix);
                matrix = new double[externalNeuronsInLayersCount[i], externalNeuronsInLayersCount[i + 1]];
                deltaMatrixList.Add(matrix);
            }
            for(int i = 0; i < externalNeuronsInLayersCount.Length; ++i)
            {
                neurons.Add(new Neuron[externalNeuronsInLayersCount[i]]);
                for(int j = 0; j < neurons[i].Length; ++j)
                {
                    neurons[i][j] = new Neuron();
                }
            }
            //NormalizeData(data, outputLength / 2);
        }
        
        public MultidimensionalPoint[] ShuffleArray(MultidimensionalPoint[] info)
        {
            Random rnd = new Random();
            info = info.OrderBy(x => rnd.Next()).ToArray();

            return info;
        }

        public double CalcOneResult(double[] inputValues)
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
            return neurons[neurons.Count - 1][0].value;
        }

        public double CalcTrueResult(MultidimensionalPoint point)
        {
            return point.pointClass;
        }

        public double CalcErrorRate()
        {
            double 
                errorCounter = 0,
                result,
                trueResult,
                errorRate,
                appropriateDelta;
            for(int i = 0; i < data.Length * trainingSetPercentage; ++i)
            {
                result = CalcOneResult(data[i].coordinates);
                trueResult = CalcTrueResult(data[i]);
                appropriateDelta = 1 / numberOfClasses * 0.05;
                if (!(result >= trueResult - appropriateDelta && result <= trueResult + appropriateDelta))
                {
                    errorCounter++;
                }
            }
            errorRate = errorCounter / Math.Truncate(data.Length * trainingSetPercentage);

            return errorRate;
        }

        public void ShowLogMessage(int epochNumber)
        {
            double rate = CalcErrorRate();
            Console.WriteLine("Epoch {0}, Error {1:0.0000}", epochNumber, rate);
        }

        public double TrainNetwork(int epochs)
        {
            double sum = 0;
            for(int epochsCounter = 0; epochsCounter < epochs; ++epochsCounter)
            {
                //data = ShuffleArray(data);
                Debug.Assert(neurons[neurons.Count - 1].Length == 1);
                for (int dataCounter = 0; dataCounter < data.Length * trainingSetPercentage; ++dataCounter)
                {
                    var resultNode = neurons[neurons.Count - 1][0];
                    CalcOneResult(data[dataCounter].coordinates);
                    resultNode.delta = resultNode.value * (1 - resultNode.value) * (CalcTrueResult(data[dataCounter]) - resultNode.value);
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
            }
            Console.WriteLine("done");
            return CalcErrorRate();
        }

        private double ActivationFunction(double inputValue, double shift)
        {
            double result = 1 / (1 + Math.Exp(-2 * alpha * inputValue + shift));
            return result;
        }

        public void NormalizeData(MultidimensionalPoint[] data, double maxOverallValue)
        {
            double
                maxValue,
                minValue,
                delta;
            for (int dimension = 0; dimension < data[0].coordinates.Length; ++dimension)
            {
                maxValue = (-1) * double.MaxValue;
                minValue = double.MaxValue;
                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i].coordinates[dimension] > maxValue)
                        maxValue = data[i].coordinates[dimension];
                    if (data[i].coordinates[dimension] < minValue)
                        minValue = data[i].coordinates[dimension];
                }
                delta = maxValue - minValue;
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i].coordinates[dimension] = (data[i].coordinates[dimension] - minValue - delta / 2) * 2 * maxOverallValue / delta;
                }
            }
            for (int i = 0; i < data.Length; ++i)
            {
                for (int j = 0; j < data[0].coordinates.Length; ++j)
                {
                    Debug.Assert(Math.Abs(data[i].coordinates[j]) <= maxOverallValue + 0.001);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointsGeneration;

namespace NN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            networkStructure.Text = "2 5 1";
            pathBox.Text = "generated_data.csv";
            start_Click(null, new EventArgs());
        }

        private void start_Click(object sender, EventArgs e)
        {
            int[] neuronsInLayersCount = parseNeuronsInLayersCount(networkStructure.Text);
            string path = pathBox.Text;
            DataReader dr = new DataReader();
            //MultidimensionalPoint[] data = dr.ReadData(path, new string[] { "\n" }, new char[] { ',' });
            MultidimensionalPoint[] data = TestDataGenerator.GenerateTestData(100, TestDataGenerator.orFunc);
            double error;
            NeuralNetwork NeurNet = new NeuralNetwork(data, 1, neuronsInLayersCount, 4, 4, 0.7, 0.01, 2, 1, 0.01);
            error = NeurNet.TrainNetwork(5000); 
            Console.ReadKey();
        }

        private int[] parseNeuronsInLayersCount(string rawString)
        {
            int[] outputArray = rawString.Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
            return outputArray;
        }

    }
}

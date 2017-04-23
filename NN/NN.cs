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
using Common;

namespace NN
{
    public partial class NN : Form
    {
        MultidimensionalPoint[] data;
        NeuralNetwork NeurNet;

        public NN()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            networkStructure.Text = "15 20 20 15";
            epochsNumber.Text = "500";
            pathBox.Text = "C:\\Users\\Дмитрий\\Desktop\\Lessons\\Kurochkin\\lab 1\\Report\\touching15d15.csv";
            this.Show();
            //NeuralNetwork.GoOverfit();
        }

        private void start_Click(object sender, EventArgs e)
        {
            InitializeNetwork();
            NeurNet.TrainNetwork(Convert.ToInt32(epochsNumber.Text), null);
            NeurNet.WriteClassifiedResult(NeurNet.data);
        }

        public void InitializeNetwork()
        {
            int[] neuronsInLayersCount = parseNeuronsInLayersCount(networkStructure.Text);
            string path = pathBox.Text;
            DataReader dr = new DataReader();
            data = dr.ReadData(path, new string[] { "\n" }, new char[] { ',' });
            //data = dr.ReadFisher(path, new string[] { "\r\n" }, new char[] { ',' });
            NeurNet = new NeuralNetwork(data, neuronsInLayersCount, 1, 0, 0.7, 0.01, 2, 0.8, 0.01);
        }

        private int[] parseNeuronsInLayersCount(string rawString)
        {
            int[] outputArray = rawString.Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
            return outputArray;
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            NeurNet.ShowGraph(onlyTest.Checked);
        }

    }
}

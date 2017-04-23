using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using System.Diagnostics;


namespace Visualizer
{
    public partial class visualizer : Form
    {
        Graphics g;
        public visualizer()
        {
            InitializeComponent();
        }

        public void ShowGraph(bool showBorders, MultidimensionalPoint[] data, bool showClassification, bool drawOnlyTest, double testPercentage)
        {
            g = this.CreateGraphics();
            int shiftX = 500;
            int shiftY = 300;

            Common.Methods.NormalizeData(data, 300);
            Debug.Assert(data[0].coordinates.Length == 2);

            Brush[] brushCollection = new Brush[15] { Brushes.Black, Brushes.Red, Brushes.Violet, Brushes.Green, Brushes.Gray, Brushes.Brown, Brushes.Blue, Brushes.Coral,
                Brushes.Cyan, Brushes.Crimson, Brushes.DarkGreen, Brushes.DarkOrange, Brushes.DeepPink, Brushes.DeepSkyBlue, Brushes.Gold};
            Pen[] pensCollection = new Pen[15] {Pens.Black, Pens.Red, Pens.Violet, Pens.Green, Pens.Gray, Pens.Brown, Pens.Blue, Pens.Coral,
                Pens.Cyan, Pens.Crimson, Pens.DarkGreen, Pens.DarkOrange, Pens.DeepPink, Pens.DeepSkyBlue, Pens.Gold};
            g.DrawLine(Pens.Black, -1000, shiftY, 1000, shiftY);
            g.DrawLine(Pens.Black, shiftX, -1000, shiftX, 1000);
            int i = 0;
            if(drawOnlyTest) i = (int)(data.Length * testPercentage) + 1;
            for (; i < data.Length; ++i)
            {
                if (showBorders && data[i].radius > 0) 
                    g.DrawEllipse(Pens.Black, (int)data[i].coordinates[0] + shiftX - (int)data[i].radius, 
                        (int)data[i].coordinates[1] + shiftY - (int)data[i].radius, (int)(2*data[i].radius), (int)(2*data[i].radius));
                if (showClassification)
                {
                    if(data[i].classifiedAs == data[i].pointClass)
                        g.FillRectangle(brushCollection[data[i].pointClass % brushCollection.Length], 
                            (int)data[i].coordinates[0] + shiftX, (int)data[i].coordinates[1] + shiftY, 2, 2);
                    else
                    {
                        g.DrawLine(pensCollection[data[i].classifiedAs % pensCollection.Length], shiftX + (int)data[i].coordinates[0] - 2,
                            shiftY + (int)data[i].coordinates[1], shiftX + (int)data[i].coordinates[0] + 2, shiftY + (int)data[i].coordinates[1]);
                        g.DrawLine(pensCollection[data[i].classifiedAs % pensCollection.Length], shiftX + (int)data[i].coordinates[0],
                            shiftY + (int)data[i].coordinates[1] - 2, shiftX + (int)data[i].coordinates[0], shiftY + (int)data[i].coordinates[1] + 2);
                    }
                }
                else
                    g.FillRectangle(brushCollection[data[i].pointClass % brushCollection.Length], 
                        (int)data[i].coordinates[0] + shiftX, (int)data[i].coordinates[1] + shiftY, 2, 2);
            }
        }

        private void visualizer_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        
    }
}

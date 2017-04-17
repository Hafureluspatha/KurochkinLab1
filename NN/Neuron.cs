using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN
{
    public class Neuron
    {
        public double value;
        public double delta;

        public Neuron()
        {
            value = 0;
            delta = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysisLib
{
    public class Frequency
    {
        private double start;
        private double finish;
        private int count = 0;
        public Frequency(double start, double finish)
        {
            this.start = start;
            this.finish = finish;
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public double Start
        {
            get { return start; }
        }

        public double Finish
        {
            get { return finish; }
        }
    }
}

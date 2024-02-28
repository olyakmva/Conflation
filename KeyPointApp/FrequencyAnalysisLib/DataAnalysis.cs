using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysisLib
{
    public class DataAnalysis
    {   
        public DataAnalysis() { }
        public double Analysis(double[] first_watermark, double[] second_watermark)
        {
            double cnt = 0;
            double n = first_watermark.Length;
            for (int i=0;i<first_watermark.Length;i++)
            {
                if (first_watermark[i] == second_watermark[i])
                {
                    cnt++;
                }
            }
            return (cnt / n) * 100;
        }
    }
}

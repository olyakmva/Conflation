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
        public double Analysis(double[] first_watermark, double[] second_watermark, double tolerance = 0.1)
        {
            double cnt1 = 0;
            double n = first_watermark.Length;
            for (int i=0;i<first_watermark.Length;i++)
            {
                if (Math.Abs(first_watermark[i] - second_watermark[i]) < tolerance)
                {
                    cnt1++;
                }
            }
            int cnt2 = 0;
            for (int i = 0; i < first_watermark.Length; i++)
            {
                if (Math.Abs(first_watermark[i] - second_watermark[first_watermark.Length-i-1]) < tolerance)
                {
                    cnt2++;
                }
            }
            double cnt = Math.Max(cnt1, cnt2);
            return (cnt / n) * 100;
        }
    }
}

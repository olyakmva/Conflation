using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysisLib
{
    public class DataAnalysis
    {   
        public DataAnalysis() { }
        public double Analysis(double[] first_watermark, double[] second_watermark, double tolerance = 0.1)
        {
            double cnt = 0;
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
            if (cnt2 / n * 100 > 50)
            {
                cnt = Math.Max(cnt1, cnt2);
            }
            else
            {
                cnt = cnt1;
            }
            return (cnt / n) * 100;
        }
        public double Analysis(double[,] first_watermark, double[,] second_watermark, double tolerance = 0.1)
        {
            double cnt = 0;
            double cnt1 = 0;
            int n = first_watermark.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Math.Abs(first_watermark[i,j] - second_watermark[i,j]) < tolerance)
                    {
                        cnt1++;
                    }
                }
            }
            int cnt2 = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Math.Abs(first_watermark[i, j] - second_watermark[n-1-i, n-1-j]) < tolerance)
                    {
                        cnt2++;
                    }
                }
            }
            if (cnt2 / n * 100 > 50)
            {
                cnt = Math.Max(cnt1, cnt2);
            }
            else
            {
                cnt = cnt1;
            }
            return (cnt / Math.Pow(n,2)) * 100;
        }
    }
}

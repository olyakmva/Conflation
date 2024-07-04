using AlgorithmsLibrary;
using SupportLib;
using System.Diagnostics;
using System.Globalization;
using System.Transactions;

namespace FrequencyAnalysisLib
{
    public class FreqAlg : BaseFreqAlg
    {
        //private Map curr_map;

        //private Rectangle cellRectangle;

        //private double curr_step;

        //private double[] watermark;

        //private List<Frequency> frequencies = new List<Frequency>();

        //private List<MPoint> mPoints= new List<MPoint>();
        //public FreqAlg(Map map) : this(map, 0.1) { }
        //public FreqAlg(Map map, double step)
        //{
        //    curr_map = map;
        //    cellRectangle = GetRectangle(map);
        //    curr_step = step;
        //    int n = (int)(1 / step);
        //    watermark = new double[n];
        //}

        //public Map Curr_map 
        //{ 
        //    get 
        //    { 
        //        return curr_map;
        //    } 
        //}

        //public Rectangle CellRectangle
        //{
        //    get 
        //    { 
        //        return cellRectangle;
        //    }
        //}

        //public double Curr_Step
        //{
        //    get 
        //    { 
        //        return curr_step;
        //    }
        //}

        //public double[] Watermark
        //{
        //    get
        //    {
        //        return watermark;
        //    }
        //}

        //public List<MPoint> MPoints
        //{
        //    get 
        //    { 
        //        return mPoints;
        //    }
        //}

        //public List<Frequency> Frequencies
        //{
        //    get 
        //    { 
        //        return frequencies;
        //    }
        //}
        //public MPoint RecalculateOfCoordinates(MPoint point)
        //{
        //    double x = (point.X - CellRectangle.LowLeft.X) / (CellRectangle.UpRight.X - CellRectangle.LowLeft.X);

        //    return new MPoint(x, point.Y);
        //}

        //public void Process()
        //{
        //    mPoints = AddingMPointsToList();
        //    for (int i = 0;i<watermark.Length;i++)
        //    {
        //        var fr = new Frequency(i*curr_step, (i+1)*curr_step);
        //        frequencies.Add(fr);
        //    }
        //    double epsilon = 1.0 / Math.Pow(10, 8);
        //    for (int i = 0; i < mPoints.Count; i++)
        //    {
        //        for (int j = 0; j < frequencies.Count; j++)
        //        {
        //            var fr = frequencies[j];

        //            if (fr.Start < mPoints[i].X && fr.Finish > mPoints[i].X ||
        //                Math.Abs(fr.Start - mPoints[i].X) < epsilon ||
        //                Math.Abs(fr.Finish - mPoints[i].X) < epsilon)
        //            {
        //                fr.Count++;
        //                break;
        //            }
        //        }
        //    }
        //    for (int i =0; i<frequencies.Count; i++)
        //    {
        //        watermark[i] = frequencies[i].Count;
        //    }
        //}

        //public void FrequencyRationing()
        //{
        //    double max = Watermark.Max();
        //    double min = Watermark.Min();
        //    double difference = max - min;
        //    for (int i = 0; i<watermark.Length; i++)
        //    {
        //        watermark[i] = (watermark[i] - min) / difference;
        //    }
        //}

        //private List<MPoint> AddingMPointsToList()
        //{
        //    var result = new List<MPoint>();
        //    var objects = curr_map.GetMapObjItems();
        //    foreach (var obj in objects)
        //    {
        //        for (int i = 0; i < obj.Points.Count; i++)
        //        {
        //            var point = obj.Points[i];
        //            MPoint mpoint = new MPoint(point.X, point.Y);
        //            MPoint new_mpoint = RecalculateOfCoordinates(mpoint);
        //            result.Add(new_mpoint);
        //        }
        //    }
        //    return result;
        //}

        //private Rectangle GetRectangle(Map m)
        //{
        //    double xmin = m.Xmin; double xmax = m.Xmax;
        //    double ymin = m.Ymin; double ymax = m.Ymax;
        //    MPoint LowLeft = new MPoint(xmin, ymin);
        //    MPoint UpLeft = new MPoint(xmin, ymax);
        //    MPoint UpRight = new MPoint(xmax, ymax);
        //    MPoint LowRight = new MPoint(xmax,ymin);
        //    return new Rectangle(LowLeft,UpLeft, UpRight, LowRight);
        //}

        private List<Frequency> frequencies = new List<Frequency>();

        private double[] watermark;
        
        public FreqAlg(Map map) : this(map, 0.1) { }
        public FreqAlg(Map map, double step)
        {
            Curr_map = map;
            CellRectangle = GetRectangle(map);
            Curr_Step = step;
            int n = (int)(1 / step);
            Watermark = new double[n];
        }

        public List<Frequency> Frequencies
        {
            get
            {
                return frequencies;
            }
            set
            {
                frequencies = value;
            }
        }
        public double[] Watermark
        {
            get
            {
                return watermark;
            }
            set
            {
                watermark = value;
            }
        }

        public override void FrequencyRationing()
        {
            double max = Watermark.Max();
            double min = Watermark.Min();
            double difference = max - min;
            for (int i = 0; i < watermark.Length; i++)
            {
                watermark[i] = (watermark[i] - min) / difference;
            }
        }

        public override Rectangle GetRectangle(Map m)
        {
            double xmin = m.Xmin; double xmax = m.Xmax;
            double ymin = m.Ymin; double ymax = m.Ymax;
            MPoint LowLeft = new MPoint(xmin, ymin);
            MPoint UpLeft = new MPoint(xmin, ymax);
            MPoint UpRight = new MPoint(xmax, ymax);
            MPoint LowRight = new MPoint(xmax, ymin);
            return new Rectangle(LowLeft, UpLeft, UpRight, LowRight);
        }

        public override void Process()
        {
            MPoints = AddingMPointsToList();
            for (int i = 0; i < watermark.Length; i++)
            {
                var fr = new Frequency(i * Curr_Step, (i + 1) * Curr_Step);
                Frequencies.Add(fr);
            }
            double epsilon = 1.0 / Math.Pow(10, 8);
            for (int i = 0; i < MPoints.Count; i++)
            {
                for (int j = 0; j < Frequencies.Count; j++)
                {
                    var fr = Frequencies[j];

                    if (fr.Start < MPoints[i].X && fr.Finish > MPoints[i].X ||
                        Math.Abs(fr.Start - MPoints[i].X) < epsilon ||
                        Math.Abs(fr.Finish - MPoints[i].X) < epsilon)
                    {
                        fr.Count++;
                        break;
                    }
                }
            }
            for (int i = 0; i < Frequencies.Count; i++)
            {
                watermark[i] = Frequencies[i].Count;
            }
        }

        public override MPoint RecalculateOfCoordinates(MPoint point)
        {
            double x = (point.X - CellRectangle.LowLeft.X) / (CellRectangle.UpRight.X - CellRectangle.LowLeft.X);

            return new MPoint(x, point.Y);
        }
    }
}
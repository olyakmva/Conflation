using AlgorithmsLibrary;
using SupportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysisLib
{
    public class QuadFreqAlg : BaseFreqAlg
    {
        //private Map curr_map;

        //private Rectangle cellRectangle;

        //private double curr_step;

        //private double[,] watermark;

        //private List<Frequency> frequenciesX = new List<Frequency>();
        //private List<Frequency> frequenciesY = new List<Frequency>();

        //private List<MPoint> mPoints = new List<MPoint>();
        //public QuadFreqAlg(Map map) : this(map, 0.1) { }
        //public QuadFreqAlg(Map map, double step)
        //{
        //    curr_map = map;
        //    cellRectangle = GetRectangle(map);
        //    curr_step = step;
        //    int n = (int)(1 / step);
        //    watermark = new double[n, n];
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

        //public double[,] Watermark
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

        //public List<Frequency> FrequenciesX
        //{
        //    get
        //    {
        //        return frequenciesX;
        //    }
        //}

        //public List<Frequency> FrequenciesY
        //{
        //    get
        //    {
        //        return frequenciesY;
        //    }
        //}
        //public MPoint RecalculateOfCoordinates(MPoint point)
        //{
        //    double x = (point.X - CellRectangle.LowLeft.X) / (CellRectangle.UpRight.X - CellRectangle.LowLeft.X);
        //    double y = (point.Y - CellRectangle.LowLeft.Y) / (CellRectangle.UpRight.Y - CellRectangle.LowLeft.Y);
        //    return new MPoint(x, y);
        //}

        //public void Process()
        //{
        //    mPoints = AddingMPointsToList();
        //    for (int i = 0; i < watermark.GetLength(0); i++)
        //    {
        //        var fr = new Frequency(i * curr_step, (i + 1) * curr_step);
        //        frequenciesX.Add(fr);
        //        var fr1 = new Frequency(i * curr_step, (i + 1) * curr_step);
        //        frequenciesY.Add(fr1);
        //    }
        //    double epsilon = 1.0 / Math.Pow(10, 8);
        //    for (int i = 0; i < mPoints.Count; i++)
        //    {
        //        bool isValid = false;
        //        for (int j = 0; j < frequenciesX.Count; j++)
        //        {
        //            for (int k = 0; k < frequenciesY.Count; k++)
        //            {
        //                var frx = frequenciesX[j];
        //                var fry = frequenciesY[k];
        //                bool x_flag = false;
        //                bool y_flag = false;

        //                if (frx.Start < mPoints[i].X && frx.Finish > mPoints[i].X ||
        //                    Math.Abs(frx.Start - mPoints[i].X) < epsilon ||
        //                    Math.Abs(frx.Finish - mPoints[i].X) < epsilon)
        //                {
        //                    x_flag = true;
        //                }

        //                if (fry.Start < mPoints[i].Y && fry.Finish > mPoints[i].Y ||
        //                   Math.Abs(fry.Start - mPoints[i].Y) < epsilon ||
        //                   Math.Abs(fry.Finish - mPoints[i].Y) < epsilon)
        //                {
        //                    y_flag = true;
        //                }
        //                if (y_flag && x_flag)
        //                {
        //                    isValid = true;
        //                    frx.Count++;
        //                    fry.Count++;
        //                    watermark[k, j]++;
        //                }
        //                if (isValid)
        //                {
        //                    break;
        //                }
        //            }
        //            if (isValid)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}

        //public void FrequencyRationing()
        //{
        //    double max = -1;
        //    double min = mPoints.Count;
        //    for (int i = 0; i < watermark.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < watermark.GetLength(1); j++)
        //        {
        //            if (max < watermark[i, j])
        //            {
        //                max = watermark[i, j];
        //            }
        //            if (watermark[i, j] < min)
        //            {
        //                min = watermark[i, j];
        //            }
        //        }
        //    }
        //    double difference = max - min;
        //    for (int i = 0; i < watermark.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < watermark.GetLength(1); j++)
        //        {
        //            watermark[i, j] = (watermark[i, j] - min) / difference;
        //        }
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
        //    double distance = Math.Max(xmax - xmin, ymax - ymin);
        //    MPoint LowLeft = new MPoint(xmin, ymin);
        //    MPoint UpLeft = new MPoint(xmin, distance);
        //    MPoint UpRight = new MPoint(distance, distance);
        //    MPoint LowRight = new MPoint(distance, ymin);
        //    return new Rectangle(LowLeft, UpLeft, UpRight, LowRight);
        //}


        private double[,] watermark;
        
        private List<Frequency> frequenciesX = new List<Frequency>();
        private List<Frequency> frequenciesY = new List<Frequency>();

        public QuadFreqAlg(Map map) : this(map, 0.1) { }
        public QuadFreqAlg(Map map, double step)
        {
            Curr_map = map;
            CellRectangle = GetRectangle(map);
            Curr_Step = step;
            int n = (int)(1 / step);
            watermark = new double[n, n];
        }

        public double[,] Watermark
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
        public List<Frequency> FrequenciesX
        {
            get
            {
                return frequenciesX;
            }
            set
            {
                frequenciesX = value;
            }
        }

        public List<Frequency> FrequenciesY
        {
            get
            {
                return frequenciesY;
            }
            set
            {
                frequenciesY = value;
            }
        }

        public override void FrequencyRationing()
        {
            double max = -1;
            double min = MPoints.Count;
            for (int i = 0; i < watermark.GetLength(0); i++)
            {
                for (int j = 0; j < watermark.GetLength(1); j++)
                {
                    if (max < watermark[i, j])
                    {
                        max = watermark[i, j];
                    }
                    if (watermark[i, j] < min)
                    {
                        min = watermark[i, j];
                    }
                }
            }
            double difference = max - min;
            for (int i = 0; i < watermark.GetLength(0); i++)
            {
                for (int j = 0; j < watermark.GetLength(1); j++)
                {
                    watermark[i, j] = (watermark[i, j] - min) / difference;
                }
            }
        }

        public override Rectangle GetRectangle(Map m)
        {
            double xmin = m.Xmin; double xmax = m.Xmax;
            double ymin = m.Ymin; double ymax = m.Ymax;
            double distance = Math.Max(xmax - xmin, ymax - ymin);
            MPoint LowLeft = new MPoint(xmin, ymin);
            MPoint UpLeft = new MPoint(xmin, distance);
            MPoint UpRight = new MPoint(distance, distance);
            MPoint LowRight = new MPoint(distance, ymin);
            return new Rectangle(LowLeft, UpLeft, UpRight, LowRight);
        }

        public override void Process()
        {
            MPoints = AddingMPointsToList();
            for (int i = 0; i < Watermark.GetLength(0); i++)
            {
                var fr = new Frequency(i * Curr_Step, (i + 1) * Curr_Step);
                frequenciesX.Add(fr);
                var fr1 = new Frequency(i * Curr_Step, (i + 1) * Curr_Step);
                frequenciesY.Add(fr1);
            }
            double epsilon = 1.0 / Math.Pow(10, 8);
            for (int i = 0; i < MPoints.Count; i++)
            {
                bool isValid = false;
                for (int j = 0; j < frequenciesX.Count; j++)
                {
                    for (int k = 0; k < frequenciesY.Count; k++)
                    {
                        var frx = frequenciesX[j];
                        var fry = frequenciesY[k];
                        bool x_flag = false;
                        bool y_flag = false;

                        if (frx.Start < MPoints[i].X && frx.Finish > MPoints[i].X ||
                            Math.Abs(frx.Start - MPoints[i].X) < epsilon ||
                            Math.Abs(frx.Finish - MPoints[i].X) < epsilon)
                        {
                            x_flag = true;
                        }

                        if (fry.Start < MPoints[i].Y && fry.Finish > MPoints[i].Y ||
                           Math.Abs(fry.Start - MPoints[i].Y) < epsilon ||
                           Math.Abs(fry.Finish - MPoints[i].Y) < epsilon)
                        {
                            y_flag = true;
                        }
                        if (y_flag && x_flag)
                        {
                            isValid = true;
                            frx.Count++;
                            fry.Count++;
                            watermark[k, j]++;
                        }
                        if (isValid)
                        {
                            break;
                        }
                    }
                    if (isValid)
                    {
                        break;
                    }
                }
            }
        }

        public override MPoint RecalculateOfCoordinates(MPoint point)
        {
            double x = (point.X - CellRectangle.LowLeft.X) / (CellRectangle.UpRight.X - CellRectangle.LowLeft.X);
            double y = (point.Y - CellRectangle.LowLeft.Y) / (CellRectangle.UpRight.Y - CellRectangle.LowLeft.Y);
            return new MPoint(x, y);
        }
    }
}

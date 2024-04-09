using SupportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparisonLib
{
    public class OnlyPolygonComparison
    {
        public double AngleSimilarityMeasure = 1.0;
        public double AzAngleCalculator(MapPoint mp1, MapPoint mp2)
        {
            double alfa = Math.Atan((mp2.Y - mp1.Y) / (mp2.X - mp1.X)) * 180 / Math.PI;
            if (mp1.X > mp2.X)
                return 270 - alfa;
            else return 90 - alfa;
        }
        public double DistanceBetweenPoints(MapPoint mp1, MapPoint mp2)
        {
            double dist = Math.Sqrt(Math.Pow(mp2.X - mp1.X, 2) + Math.Pow(mp2.Y - mp1.Y, 2));
            return dist;
        }
        public double FindAngleBetweenBeggestDiagonals(MapObjItem item)
        {
            (MapPoint, MapPoint) pairOfPointsWithMaxDist1 = new();
            (MapPoint, MapPoint) pairOfPointsWithMaxDist2 = new();
            double MaxDist1 = double.MinValue, MaxDist2 = double.MinValue;
            List<MapPoint> points = item.Points;
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    double tempDist = DistanceBetweenPoints(points[i], points[j]);
                    if (tempDist > MaxDist1)
                    {
                        MaxDist1 = tempDist;
                        pairOfPointsWithMaxDist1.Item1 = points[i];
                        pairOfPointsWithMaxDist1.Item2 = points[j];
                    }
                    if (tempDist > MaxDist2 && tempDist != MaxDist1)
                    {
                        MaxDist2 = tempDist;
                        pairOfPointsWithMaxDist2.Item1 = points[i];
                        pairOfPointsWithMaxDist2.Item2 = points[j];
                    }
                }
            }
            //теперь высчитываем углы
            double angle1 = AzAngleCalculator(pairOfPointsWithMaxDist1.Item1, pairOfPointsWithMaxDist1.Item2);
            double angle2 = AzAngleCalculator(pairOfPointsWithMaxDist2.Item1, pairOfPointsWithMaxDist2.Item2);
            if (angle1 > angle2)
                return angle1 - angle2 + 180;
            else return angle2 - angle1 + 180;
        }

        public double AlgorithmForPolygon(MapData md1, MapData md2)
        {
            List<MapObjItem> Poligons1 = md1.GetMapObjItems();
            List<MapObjItem> Poligons2 = md2.GetMapObjItems();

            List<double> anglesOfPolygons1 = new List<double>();
            List<double> anglesOfPolygons2 = new List<double>();


            foreach (MapObjItem mapObjItem in Poligons1)
            {
                anglesOfPolygons1.Add(FindAngleBetweenBeggestDiagonals(mapObjItem));
            }
            foreach (MapObjItem mapObjItem in Poligons2)
            {
                anglesOfPolygons2.Add(FindAngleBetweenBeggestDiagonals(mapObjItem));
            }
            double minCapacity = Math.Min(Poligons1.Count, Poligons2.Count);
            double countOfRepetition = CountRepetitionAnglesOfPolygones(anglesOfPolygons1, anglesOfPolygons2);
            return (countOfRepetition / minCapacity) * 100; //находим отношение совпадений к наименьшей мощности множеств
        }
        public double CountRepetitionAnglesOfPolygones(List<double> anglesOfPolygons1, List<double> anglesOfPolygons2)
        {
            double count = 0;
            // более мощное множество идет вторым
            if (anglesOfPolygons2.Count < anglesOfPolygons1.Count)
            {
                var temp = anglesOfPolygons1;
                anglesOfPolygons1 = anglesOfPolygons2;
                anglesOfPolygons2 = temp;
            }
            for (int i = 0; i < anglesOfPolygons1.Count; i++)
            {
                for (int j = 0; j < anglesOfPolygons2.Count; j++)
                {
                    if (IsValueQuiteСlose(anglesOfPolygons1[i], anglesOfPolygons2[j]))
                    {
                        count++;
                        break;
                    }
                }
            }
            return count;
        }
        public bool IsValueQuiteСlose(double d1, double d2)
        {
            if (Math.Abs(d1 - d2) < AngleSimilarityMeasure)
                return true;
            else return false;
        }
    }
}

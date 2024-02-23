using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class Point
    {
        public double X { set; get; }
        public double Y { set; get; }
        public double DistanceToVertex(Point v)
        {
            return Math.Sqrt(Math.Pow(X - v.X, 2) + Math.Pow(Y - v.Y, 2));
        }
        public Point(Point other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }
        public Point()
        {

        }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}

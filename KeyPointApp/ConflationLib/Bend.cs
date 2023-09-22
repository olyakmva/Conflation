using AlgorithmsLibrary;
using SupportLib;


namespace ConflationLib
{
    public class Bend
    {
        /// <summary>
        /// список вершин в последовательности, в которой они идут в изгибе
        /// </summary>
        public List<MapPoint> NodeList { set; get; }

        #region Constructors

        public Bend()
        {
            NodeList = new List<MapPoint>();
        }

        public Bend(List<MapPoint> vert)
            : this()
        {
            NodeList.AddRange(vert);
        }

        #endregion

        /// <summary>
        /// вычисление площади изгиба
        /// </summary>
        /// <returns>площадь изгиба</returns>
        public double Area()
        {
            if (NodeList.Count < 3)
                return 0;

            double result = 0;
            for (var i = 0; i < NodeList.Count - 1; i++)
            {
                result += (NodeList[i].X + NodeList[i + 1].X) * (NodeList[i].Y - NodeList[i + 1].Y);
            }

            result += (NodeList[NodeList.Count - 1].X + NodeList[0].X) * (NodeList[NodeList.Count - 1].Y - NodeList[0].Y);

            return Math.Abs(result / 2);
        }

        /// <summary>
        /// вычисление периметра изгиба
        /// </summary>
        /// <returns>периметр изгиба, включая основание</returns>
        public double Perimeter()
        {
            if (NodeList.Count < 3)
                return 0;

            return Length() + BaseLineLength();
        }
        /// <summary>
        /// вычисление длины линии, образующей изгиб
        /// </summary>
        /// <returns>длина</returns>
        public double Length()
        {
            if (NodeList.Count < 3)
                return 0;

            double result = 0;

            for (var i = 0; i < NodeList.Count - 1; i++)
            {
                result += Math.Sqrt((NodeList[i + 1].X - NodeList[i].X) * (NodeList[i + 1].X - NodeList[i].X) + (NodeList[i + 1].Y - NodeList[i].Y) * (NodeList[i + 1].Y - NodeList[i].Y));
            }
            return result;
        }
        public VectorLib.Vector? GetBaseVector()
        {
            if (NodeList.Count < 3)
                return null;
            return new VectorLib.Vector(NodeList[0], NodeList[NodeList.Count - 1]);
        }
        /// <summary>
        /// подсчёт индекса компактности
        /// </summary>
        /// <returns>индекс компактности</returns>
        public double CompactIndex()
        {
            if (NodeList.Count < 3)
                return 0;

            return 4 * Math.PI * Area() / Math.Pow(Perimeter(), 2);
        }

        public double BaseLineLength()
        {
            if (NodeList.Count < 2)
                return 0;
            return Math.Sqrt((NodeList[NodeList.Count - 1].X - NodeList[0].X) * (NodeList[NodeList.Count - 1].X - NodeList[0].X) + (NodeList[NodeList.Count - 1].Y - NodeList[0].Y) * (NodeList[NodeList.Count - 1].Y - NodeList[0].Y));
        }

        public double[] BendMiddlePoint()
        {
            if (NodeList.Count == 0)
                return new double[2];

            return new[] { (NodeList[0].X + NodeList[NodeList.Count - 1].X) / 2, (NodeList[0].Y + NodeList[NodeList.Count - 1].Y) / 2 };
        }

        /// <summary>
        /// поиск пика изгиба
        /// </summary>
        /// <returns>индекс пика изгиба</returns>
        public int PeakIndex()
        {
            if (NodeList.Count == 0)
                return 0;

            MapPoint begin = NodeList[0];
            MapPoint end = NodeList[NodeList.Count - 1];
            var peakIndex = 0;
            double maxSum = 0;
            for (var i = 1; i < NodeList.Count - 1; i++)
            {
                var tempSum = Math.Sqrt(Math.Pow((NodeList[i].X - begin.X), 2) + Math.Pow((NodeList[i].Y - begin.Y), 2)) +
                                 Math.Sqrt(Math.Pow((NodeList[i].X - end.X), 2) + Math.Pow((NodeList[i].Y - end.Y), 2));

                if (!(tempSum > maxSum)) continue;
                maxSum = tempSum;
                peakIndex = i;
            }
            return peakIndex;
        }
        /// <summary>
        /// вычисление высоты изгиба
        /// </summary>
        /// <returns>высота</returns>
        public double GetHeight()
        {
            var baseLine = new Line(NodeList[0], NodeList[NodeList.Count - 1]);
            int peakIndex = PeakIndex();
            return baseLine.GetDistance(NodeList[peakIndex]);
        }
        /// <summary>
        /// вычисление ширины изгиба
        /// </summary>
        /// <returns> ширина</returns>
        public double GetWidth()
        {
            if (NodeList.Count < 3)
                return 0;

            var baseLine = new Line(NodeList[0], NodeList[NodeList.Count - 1]);
            //  проведем перпендикулярную прямую
            var leftLine = baseLine.GetPerpendicularLine(NodeList[0]);
            var leftPoint = NodeList[0];

            if (NodeList.Count == 3)
            {
                leftPoint = baseLine.GetPerpendicularFoundationPoint(NodeList[1]);
                leftLine = new Line(NodeList[1], leftPoint);
                if (leftLine.GetSign(NodeList[0]) != leftLine.GetSign(NodeList[NodeList.Count - 1]))
                {
                    return NodeList[0].DistanceToVertex(NodeList[2]);
                }
                return Math.Max(NodeList[0].DistanceToVertex(leftPoint), NodeList[2].DistanceToVertex(leftPoint));
            }

            if (leftLine.GetSign(NodeList[1]) * leftLine.GetSign(NodeList[NodeList.Count - 1]) < 0)
            {
                leftPoint = baseLine.GetPerpendicularFoundationPoint(NodeList[1]);
                if (baseLine.GetSign(leftPoint) != 0)
                    leftLine = new Line(NodeList[1], leftPoint);
                else leftLine = baseLine.GetPerpendicularLine(NodeList[1]);
                var i = 2;
                while (i < NodeList.Count)
                {
                    if (leftLine.GetSign(NodeList[0]) == leftLine.GetSign(NodeList[i]))
                    {
                        i++;
                    }
                    else
                    {
                        leftPoint = baseLine.GetPerpendicularFoundationPoint(NodeList[i]);
                        if (baseLine.GetSign(leftPoint) != 0)
                            leftLine = new Line(NodeList[i], leftPoint);
                        else leftLine = baseLine.GetPerpendicularLine(NodeList[i]);
                        i++;
                    }
                }
            }
            // то же самое справа
            var endIndex = NodeList.Count - 1;

            var max = leftPoint.DistanceToVertex(NodeList[endIndex]);
            int j = endIndex - 1;
            while (j > 0)
            {

                var p = baseLine.GetPerpendicularFoundationPoint(NodeList[j]);
                var d = leftPoint.DistanceToVertex(p);
                if (d > max)
                    max = d;
                j--;
            }
            return Math.Round(max);


        }

        /// <summary>
        /// вычисление квадрата евклидова расстояния между изгибами, где
        /// нормализованная площадь по оси x
        /// нормализованная высота основания по оси y
        /// нормализованный иднекс компактности по оси z
        /// </summary>
        /// <param name="otherBend">изгиб, расстояние до которого вычисляем</param>
        /// <returns>квадрат расстояния между изгибами</returns>
        public double SquareDistanceToBend(Bend otherBend)
        {
            var size1 = Area();
            var size2 = otherBend.Area();
            var base1 = BaseLineLength();
            var base2 = otherBend.BaseLineLength();
            var cmp1 = CompactIndex();
            var cmp2 = otherBend.CompactIndex();

            return Math.Pow((size2 - size1) / (size2 + size1), 2) +
                Math.Pow((cmp1 - cmp2) / (cmp1 + cmp2), 2) +
                Math.Pow((base2 - base1) / (base2 + base1), 2);
        }

        /// <summary>
        /// раздувание изгиба
        /// при помощи радиального расширения
        /// </summary>
        /// <param name="factor">множитель раздувания</param>
        public void ExaggerateRadial(double factor)
        {
            var c = BendMiddlePoint();
            for (var i = 1; i < NodeList.Count - 1; i++)
            {
                NodeList[i].X = (1 - factor) * c[0] + factor * NodeList[i].X;
                NodeList[i].Y = (1 - factor) * c[1] + factor * NodeList[i].Y;
            }
        }

        

        /// <summary>
        /// сливает два последовательных похожих изгиба
        /// </summary> 
        /// <param name="nextBend">смежный изгиб</param>
        public void CombineWith(Bend nextBend)
        {
            double factor = 1.2;
            MapPoint baseVertex = nextBend.NodeList[0];
            int peakIndex = PeakIndex();
            MapPoint thisPeak = NodeList[peakIndex];
            int nextPeakInd = nextBend.PeakIndex();
            MapPoint nextPeak = nextBend.NodeList[nextPeakInd];
            var centerX = (thisPeak.X + nextPeak.X) / 2;
            var centerY = (thisPeak.Y + nextPeak.Y) / 2;

            var x = (1 - factor) * baseVertex.X + factor * centerX;
            var y = (1 - factor) * baseVertex.Y + factor * centerY;

            baseVertex.X = x;
            baseVertex.Y = y;
            NodeList.RemoveRange(peakIndex + 1, NodeList.Count - peakIndex - 1);
            NodeList.Add(baseVertex);

            for (int i = nextPeakInd; i < nextBend.NodeList.Count; i++)
            {
                NodeList.Add(nextBend.NodeList[i]);
            }
        }

        /// <summary>
        /// сливает два последовательных похожих изгиба
        /// </summary> 
        /// <param name="nextNextBend">смежный изгиб</param>
        public void CombineWith2(Bend nextNextBend)
        {
            double factor = 1.2;
            MapPoint baseVertex = nextNextBend.NodeList[0];
            int peakIndex = PeakIndex();
            MapPoint thisPeak = NodeList[peakIndex];
            int nextPeakInd = nextNextBend.PeakIndex();
            MapPoint nextPeak = nextNextBend.NodeList[nextPeakInd];
            var centerX = (thisPeak.X + nextPeak.X) / 2;
            var centerY = (thisPeak.Y + nextPeak.Y) / 2;

            var x = (1 - factor) * baseVertex.X + factor * centerX;
            var y = (1 - factor) * baseVertex.Y + factor * centerY;

            baseVertex.X = x;
            baseVertex.Y = y;
            NodeList.RemoveRange(peakIndex + 1, NodeList.Count - peakIndex - 1);
            NodeList.Add(baseVertex);

            for (int i = nextPeakInd; i < nextNextBend.NodeList.Count; i++)
            {
                NodeList.Add(nextNextBend.NodeList[i]);
            }
        }

    }

}

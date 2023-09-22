using SupportLib;
using VectorLib;

namespace ConflationLib
{
    public class BendCharacteristics
    {
        private readonly MapData _mapData1;
        private readonly MapData _mapData2;
        private double _lengthBetweenPoints;
        public double AngleBetweenVectors { get; set; } = 0.2;
        public List<KeyPoint> keyPoints = new List<KeyPoint>();
        
        public BendCharacteristics(MapData mapA, MapData mapB, double length)
        {
            _mapData1 = mapA;
            _mapData2 = mapB;
            _lengthBetweenPoints = length;
            
        }
        public List<KeyPoint> Run(bool byLengthAndVector)
        {
            var pointVectors1 = GetBendsCharacteristics(_mapData1);
            var pointVectors2 = GetBendsCharacteristics(_mapData2);
            foreach (var pv in pointVectors1)
            {
                foreach (var pv2 in pointVectors2)
                {
                   if (pv.Point.DistanceToVertex(pv2.Point) > _lengthBetweenPoints)
                            continue;
                   if (!byLengthAndVector)
                   {
                            var kp = new KeyPoint
                            {
                                PointVector1 = pv,
                                PointVector2 = pv2,
                                AngleBetweenVectors = pv.Vector.GetAngle(pv2.Vector)
                            };
                            keyPoints.Add(kp);
                   }
                   else if (pv.Vector.GetAngle(pv2.Vector) < AngleBetweenVectors)
                   {
                            var kp = new KeyPoint
                            {
                                PointVector1 = pv,
                                PointVector2 = pv2,
                                AngleBetweenVectors = pv.Vector.GetAngle(pv2.Vector)
                            };
                            keyPoints.Add(kp);
                   }

                }
            }
            return keyPoints;
        }

        public List<PointVectorRelation> GetBendsCharacteristics(MapData mapData)
        {
            var pointVectorList = new List<PointVectorRelation>();
            foreach (var chain in from obj1 in mapData.MapObjDictionary
                                  let chain = obj1.Value
                                  select chain)
            {
                if (chain.Count < 3)
                    continue;
                if (chain.Count == 3 && chain[0].CompareTo(chain[2]) == 0)
                    continue;
                var index = 0;
                List<Bend> bends = new List<Bend>();
                
                while (index < chain.Count - 2)
                {
                    Bend b;
                    ExtractBend(ref index, chain, out b);
                    Vector? vector = b.GetBaseVector();
                    if (vector != null)
                    {
                        var pointVectorRel = new PointVectorRelation
                        {
                            Point = b.NodeList[0],
                            Vector = vector
                        };
                        pointVectorList.Add(pointVectorRel);
                    }
                }
            }
            return pointVectorList;
        }


        public void ExtractBend(ref int index, List<MapPoint> chain, out Bend b)
        {
            int firstIndex = index;
            b = new Bend();
            b.NodeList.Add(chain[index]);
            b.NodeList.Add(chain[index + 1]);
            b.NodeList.Add(chain[index + 2]);
            index += 3;
            var bendOrient = Orientation(b.NodeList[0], b.NodeList[1], b.NodeList[2]);

            //ищем конец изгиба
            while (index < chain.Count)
            {
                var orient = Orientation(b.NodeList[b.NodeList.Count - 2],
                    b.NodeList[b.NodeList.Count - 1], chain[index]);
                if (orient != bendOrient)
                    break;
                b.NodeList.Add(chain[index]);
                index++;
            }
            index--;
            //добавление крайних точек к изгибу при достаточно маленьком отклонении от 180 градусов и уменьшении ширины основания
            //к концу
            while (index < chain.Count - 1)
            {
                if ((Angle(chain[index - 1], chain[index], chain[index + 1]) > 0.9)
                    && (Math.Pow(b.BaseLineLength(), 2) <
                        Math.Pow(chain[firstIndex].X - chain[index + 1].X, 2) +
                        Math.Pow(chain[firstIndex].Y - chain[index + 1].Y, 2)))
                {
                    index++;
                    b.NodeList.Add(chain[index]);
                }
                else
                {
                    break;
                }
            }
            index--;
        }
        private bool Orientation(MapPoint u, MapPoint v, MapPoint w)
        {
            return (!((v.X - u.X) * (w.Y - u.Y) - (w.X - u.X) * (v.Y - u.Y) < 0));
        }
        /// <summary>
        /// косинус угла между векторами [u,v] и [v,w]
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public double Angle(MapPoint u, MapPoint v, MapPoint w)
        {
            return ((v.X - u.X) * (w.X - v.X) + (v.Y - u.Y) * (w.Y - v.Y)) / Math.Sqrt((Math.Pow(v.X - u.X, 2) + Math.Pow(v.Y - u.Y, 2)) * (Math.Pow(w.X - v.X, 2) + Math.Pow(w.Y - v.Y, 2)));
        }
    }
}

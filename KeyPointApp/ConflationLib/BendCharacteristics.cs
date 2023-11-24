using SupportLib;

namespace ConflationLib
{
    public class BendCharacteristics
    {
        private readonly MapData _mapData1;
        private readonly MapData _mapData2;
        private readonly double _lengthBetweenPoints;
        public double AngleBetweenVectors { get; set; } = 0.2;
        public List<ObjAccordance> objAccordanceList = new();
        public Dictionary<(int, int), List<MapKeyPoint>> result = new Dictionary<(int, int), List<MapKeyPoint>>(); 
        public BendCharacteristics(MapData mapA, MapData mapB, double length)
        {
            _mapData1 = mapA;
            _mapData2 = mapB;
            _lengthBetweenPoints = length;
            
        }
        public void Run(bool byLengthAndVector)
        {
            var bendProps1 = GetBendsCharacteristics(_mapData1);
            var bendProps2 = GetBendsCharacteristics(_mapData2);
            foreach (var pv1 in bendProps1)
            {
                var bends1 = pv1.Value;
                foreach( var b1 in bends1)
                {
                    foreach (var pv2 in bendProps2)
                    {
                        var bends2 = pv2.Value;
                        foreach (var b2 in bends2)
                        {
                            if (b1.PeakPoint.DistanceToVertex(b2.PeakPoint) > _lengthBetweenPoints)
                                continue;
                            
                            if (b1.Orientation != b2.Orientation)
                                continue;
                            if(!result.ContainsKey((pv1.Key,pv2.Key)))
                            {
                                  result.Add((pv1.Key, pv2.Key), new List<MapKeyPoint>());
                                
                            }
                            result[(pv1.Key, pv2.Key)].Add(new MapKeyPoint
                            {
                                Point1 = b1.PeakPoint,
                                Point2 = b2.PeakPoint
                            });
                           
                        }
                    }
                }
            }
            foreach(var pair in result)
            {
                var item = new ObjAccordance()
                {
                    Map1ObjId = pair.Key.Item1,
                    Map2ObjId = pair.Key.Item2,
                    KeyPointNumber = pair.Value.Count,
                    BendsNumber1 = bendProps1[pair.Key.Item1].Count,
                    BendsNumber2 = bendProps2[pair.Key.Item2].Count
                };
                objAccordanceList.Add(item);
            }
           
        }

        public Dictionary<int, List<BendProperty>> GetBendsCharacteristics(MapData mapData)
        {
            var bendPropsDictionary = new Dictionary<int,List<BendProperty>>();
            foreach (var obj in mapData.MapObjDictionary)                               
            {
                var chain = obj.Value;
                if (chain.Count < 3)
                    continue;
                if (chain.Count == 3 && chain[0].CompareTo(chain[2]) == 0)
                    continue;
                var index = 0;
                bendPropsDictionary.Add(obj.Key, new List<BendProperty>()); 
                
                while (index < chain.Count - 2)
                {
                    ExtractBend(ref index, chain, out Bend b);
                    int peakIndx = b.PeakIndex();
                    
                        var bendProps = new BendProperty
                        {
                            StartPoint = b.NodeList[0],
                            PeakPoint = b.NodeList[peakIndx],
                            Orientation = Orientation(b.NodeList[0], b.NodeList[1], b.NodeList[2])
                        };
                        bendPropsDictionary[obj.Key].Add(bendProps);
                    
                }
            }
            return bendPropsDictionary;
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
                var orient = Orientation(b.NodeList[^2],
                    b.NodeList[^1], chain[index]);
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

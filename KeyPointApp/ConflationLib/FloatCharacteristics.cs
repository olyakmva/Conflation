using AlgorithmsLibrary;
using SupportLib;
using VectorLib;

namespace ConflationLib
{
    public class FloatCharacteristics
    {
        private readonly MapData _mapData1;
        private readonly MapData _mapData2;
        private double _lengthBetweenPoints;
        public  int PointRange { get; set; } = 2;   
        public Dictionary<int,List<PointVectorRelation>> map1Vectors = new Dictionary<int,List<PointVectorRelation>>();
        public Dictionary<int, List<PointVectorRelation>> map2Vectors = new Dictionary<int, List<PointVectorRelation>>();
        public List<KeyPoint> keyPoints = new List<KeyPoint>();
        public double AngleBetweenVectors { get; set; } = 0.2;
        public FloatCharacteristics(MapData mapA, MapData map2, double length)
        {
            _mapData1 = mapA;
            _mapData2 = map2;
            _lengthBetweenPoints = length;
            //Run();
        }
        public void Run(bool byLengthAndVector)
        {
            var sw = new StreamWriter("result.txt");
            sw.WriteLine("Map1;point;vectorX; vectorY;Length;");
            foreach (var obj1 in _mapData1.MapObjDictionary)
            {
                var points1 = obj1.Value;
                var list = GetVectors(points1);
               
                var pointVectorList = new List<PointVectorRelation>();
                int i = 1;
                foreach(var vector in list)
                {
                    sw.Write(obj1.Key.ToString() + ";");
                    sw.Write(points1[i] + ";");
                    sw.WriteLine(vector.ToString());
                    pointVectorList.Add(new PointVectorRelation { Vector = vector, Point = points1[i] });
                    i++;
                }
                map1Vectors.Add(obj1.Key, pointVectorList);
            }
            sw.WriteLine("Map2;point;vectorX; vectorY;Length;");
            foreach (var obj2 in _mapData2.MapObjDictionary)
                {
                    var points2 = obj2.Value;
                    var list = GetVectors(points2);
                    var pointVectorList = new List<PointVectorRelation>();
                                  
                    int i = 1;
                    foreach (var vector in list)
                    {
                        sw.Write(obj2.Key.ToString() + ";");
                        sw.Write(points2[i] + ";");
                        sw.WriteLine(vector.ToString());
                        pointVectorList.Add(new PointVectorRelation { Vector = vector, Point = points2[i] });
                        i++;
                    }
                    map2Vectors.Add(obj2.Key, pointVectorList);
            }
            sw.WriteLine("PointMap1;vector1X;vector1Y;vector1Len;PointMap2;vector2X;vector2Y;vector2Len;Angle;Distance;");
            foreach( var pair1 in map1Vectors)
            {
                var pointVectors1 = pair1.Value;
                foreach (var pv in pointVectors1)
                {
                    foreach (var pair2 in map2Vectors)
                    {
                        var pointVectors2 = pair2.Value;
                        foreach (var pv2 in pointVectors2)
                        {
                            if (pv.Point.DistanceToVertex(pv2.Point) > _lengthBetweenPoints)
                                continue;
                            if(!byLengthAndVector)
                            {
                                var kp = new KeyPoint
                                {
                                    PointVector1 = pv,
                                    PointVector2 = pv2,
                                    AngleBetweenVectors = pv.Vector.GetAngle(pv2.Vector)
                                };
                                keyPoints.Add(kp);
                                sw.WriteLine(kp);
                            }
                            else if(pv.Vector.GetAngle(pv2.Vector)< AngleBetweenVectors )
                            {
                                var kp = new KeyPoint
                                {
                                    PointVector1 = pv,
                                    PointVector2 = pv2,
                                    AngleBetweenVectors = pv.Vector.GetAngle(pv2.Vector)
                                };
                                keyPoints.Add(kp);
                                sw.WriteLine(kp);
                            }

                        }
                    }
                }
            }            
            sw.Close();     
        }
        private List<Vector> GetVectors(List<MapPoint> points)
        {
            var list = new List<Vector>();
            if (points.Count > PointRange)
            {
                for (var i = 1; i < PointRange; i++)
                {
                    var ab = new Vector(points[i], points[i - 1]);
                    var bc = new Vector(points[i], points[i + 1]);
                    list.Add(ab + bc);
                }
            }
            for(int i=PointRange; i<points.Count-PointRange; i++) 
            { 
                var ab = new Vector(points[i],points[i-PointRange]);
                var bc = new Vector(points[i], points[i + PointRange]);
                list.Add(ab + bc);
            }
            if (points.Count - PointRange > 0)
            {
                for (var i = points.Count - PointRange; i < points.Count - 1; i++)
                {
                    var ab = new Vector(points[i], points[i - 1]);
                    var bc = new Vector(points[i], points[i + 1]);
                    list.Add(ab + bc);
                }
            }
            return list;
        }
      

        private Characteristic GetCharacteristic(int index, List<MapPoint> points)
        {
            double len = 0;
            if (index > PointRange && index + PointRange < points.Count)
            {
                for (var i = index - PointRange; i < index + PointRange - 1; i++)
                {
                    len += points[i].DistanceToVertex(points[i + 1]);
                }
                var list = new List<double>();
                var angles = new List<double>();
                var osX = new Line() { A = 0, B = 1, C = 0 };
                for (var i = index - PointRange; i < index + PointRange - 1; i++)
                {
                    list.Add(points[i].DistanceToVertex(points[i + 1]) / len);
                    var line = new Line(points[i], points[i + 1]);
                    angles.Add(line.GetAngle(osX));
                }
                return new Characteristic(list, angles, points);
            }
            if(index>PointRange)
            {
                for (var i = index - PointRange; i < points.Count - 1; i++)
                {
                    len += points[i].DistanceToVertex(points[i + 1]);
                }
                var list = new List<double>();
                var angles = new List<double>();
                var osX = new Line() { A = 0, B = 1, C = 0 };
                for (var i = index - PointRange; i < points.Count - 1; i++)
                {
                    list.Add(points[i].DistanceToVertex(points[i + 1]) / len);
                    var line = new Line(points[i], points[i + 1]);
                    angles.Add(line.GetAngle(osX));
                }
                return new Characteristic(list, angles, points);
            }
            else
            {
                for (var i = 0; i < index + PointRange - 1; i++)
                {
                    len += points[i].DistanceToVertex(points[i + 1]);
                }
                var list = new List<double>();
                var angles = new List<double>();
                var osX = new Line() { A = 0, B = 1, C = 0 };
                for (var i = 0; i < index + PointRange - 1; i++)
                {
                    list.Add(points[i].DistanceToVertex(points[i + 1]) / len);
                    var line = new Line(points[i], points[i + 1]);
                    angles.Add(line.GetAngle(osX));
                }
                return new Characteristic(list, angles, points);
            }
        }
    }
}

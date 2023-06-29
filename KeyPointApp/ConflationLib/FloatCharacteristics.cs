using AlgorithmsLibrary;
using System.IO;
using SupportLib;

namespace ConflationLib
{
    public class SomeFeachure
    {
        public int MapObjId1 { get; set; }
        public int MapObjId2 { get; set; }
        public int KeyPointCount { get; set; }

    }

    public class FloatCharacteristics
    {
        private readonly MapData _mapData1;
        private readonly MapData _mapData2;
        private double _lengthBetweenPoints;
        public List<SomeFeachure> SomeFeachures { get; set; }
        const int PointRange = 2;
        const double AngleValue = 200;
        const double DistanceValue =0.5 ;

        public FloatCharacteristics(MapData mapA, MapData map2, double length)
        {
            _mapData1 = mapA;
            _mapData2 = map2;
            _lengthBetweenPoints = length;
            SomeFeachures = new List<SomeFeachure>();
            Run();
        }
        private void Run()
        {
            var sw = new StreamWriter("result.txt");
            foreach (var obj1 in _mapData1.MapObjDictionary)
            {
                
                var points1 = obj1.Value;
               
                for (int q = 0; q < points1.Count; q++)
                {
                    sw.Write(obj1.Key.ToString() + ";");
                    var ch= GetCharacteristic(q, points1);
                    sw.Write(points1[q] + ";");
                    sw.WriteLine(ch);
                }
            }
                foreach (var obj2 in _mapData2.MapObjDictionary)
                {
                    var points2 = obj2.Value;
                   
                    for (var i = 0; i < points2.Count; i++)
                    {
                        sw.Write(obj2.Key.ToString() + ";");
                        var ch = GetCharacteristic(i, points2);
                        sw.Write(points2[i] + ";");
                        sw.WriteLine(ch);
                    }
                }
            sw.Close();              
        }

        private SomeFeachure GetSomeFeachure(List<MapPoint> points1, int i, List<MapPoint> points2, int j)
        {
           
            var list1 = new List<Characteristic>();
            var list2 = new List<Characteristic>();
            for( int q=i; q < points1.Count; q++ )
            {
                list1.Add(GetCharacteristic(q, points1));
            }
            for(int r=j; r < points2.Count; r++ )
            {
                list2.Add(GetCharacteristic(r, points2));
            }
            
            //int keyPointNumber = 0;
            //foreach (var c1 in list1)
            //    foreach(var c2 in list2)
            //    {
            //        var (dist, angle) = c1.DistanceTo(c2);
            //        if (dist < DistanceValue && angle < AngleValue)
            //            keyPointNumber++;
            //    }
            var sf = new SomeFeachure() { KeyPointCount = 0};
            return sf;
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

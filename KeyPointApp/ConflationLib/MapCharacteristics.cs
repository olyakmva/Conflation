using AlgorithmsLibrary;
using SupportLib;

namespace ConflationLib
{
    public class MapCharacteristics
    {
        private readonly Map _map;
        public Dictionary<int, Characteristic> mapObjCharacteristics;
        public MapCharacteristics(Map map)
        {
            _map = map;
            mapObjCharacteristics = new Dictionary<int, Characteristic>();
            Run();
        }
        private void Run()
        {
            foreach (var mapData in _map.MapLayers)
            {
                foreach (var obj in mapData.MapObjDictionary)
                {
                    var points = obj.Value;

                    double len = 0;
                    for (var i = 0; i < points.Count - 1; i++)
                    {
                        len += points[i].DistanceToVertex(points[i + 1]);
                    }
                    var list = new List<double>();
                    var angles = new List<double>();
                    var osX = new Line() { A = 0, B = 1, C = 0 };
                    for (var i = 0; i < points.Count - 1; i++)
                    {
                        list.Add(points[i].DistanceToVertex(points[i + 1]) / len);
                        var line = new Line(points[i], points[i + 1]);
                        angles.Add(line.GetAngle(osX));
                    }
                    var objCharacteristic = new Characteristic(list, angles,points);
                    mapObjCharacteristics.Add(obj.Key, objCharacteristic);
                }
            }
        }

    }
}

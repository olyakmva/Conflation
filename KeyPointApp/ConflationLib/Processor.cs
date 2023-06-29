namespace ConflationLib
{
    public class Processor
    {
        MapCharacteristics  map1, map2;
        public List<CompareResult> compareResults;
        public Processor(MapCharacteristics firstMap, MapCharacteristics secondMap)
        {
            map1 = firstMap;
            map2 = secondMap;
            compareResults = new List<CompareResult>();
        }
        public void Process()
        {
            foreach (var pair1 in map1.mapObjCharacteristics)
            {
                foreach (var pair2 in map2.mapObjCharacteristics)
                {
                    var (distance, angle) = pair1.Value.DistanceTo(pair2.Value);
                    var res = new CompareResult()
                    { 
                        Distance = distance, 
                        Angle = angle,
                        Id1 =pair1.Key, 
                        Id2 =pair2.Key
                    };
                    compareResults.Add(res);
                }
            }
            SaveTo("result.txt");

        }
        private void SaveTo(string fileName)
        {
            var sw = new StreamWriter(fileName);
            foreach(var item in compareResults)
            {
                sw.WriteLine(item);
            }
            sw.Close();
        }

    }
    public class CompareResult
    {
        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public double Distance { get; set; }
        public double Angle { get; set; }

        public override string ToString()
        {
            return $"{Id1};{Id2};{Distance};{Angle};";
        }
    }
}
//using ComparisonLib;
//using DotSpatial.Data;
using SupportLib;

var sr = new StreamReader("real.txt");
var realRate = new MapDataRelation();
string line;
while ((line = sr.ReadLine()) != null)
{
    var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
    int id1 = int.Parse(strs[0]);
    int id2 = int.Parse(strs[1]);
    if (strs.Length >= 3)
    {
        var persent = int.Parse(strs[2]);
        if (!realRate.pairs.ContainsKey(id1))
        {
            realRate.pairs.Add(id1, new Relation(id2, persent));
        }
    }
    else if (id2 == -1)
    {
        realRate.pairs.Add(id1, new Relation(id2, 0));
    }

}
sr.Close();
List<Rate> accList = new List<Rate>();
sr = new StreamReader("rate.txt");
while ((line = sr.ReadLine()) != null)
{
    var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
    int id1 = int.Parse(strs[0]);
    int id2 = int.Parse(strs[1]);
    int p = (int)Math.Truncate(double.Parse(strs[2]));
    var rate = new Rate(id1, id2, p);
    accList.Add(rate);
}
Console .ReadLine ();
//string path = "g:\\Каталог Оли\\Nauka\\MapGeneralization\\Grant2022\\Conflation\\KeyPointApp\\Data\\Voronez";
////получить данные
//var mapDatas = new List<MapData>();
//string shapeFileName = Path.Combine(path,"hdrlin500_utf_merg.shp");
//var inputShape = FeatureSet.Open(shapeFileName);

//var inputMap = Converter.ToMapData(inputShape);
//mapDatas.Add(inputMap);
//shapeFileName = Path.Combine(path, "hdrlin1000utfmerg.shp");
//inputShape = FeatureSet.Open(shapeFileName);
//inputMap = Converter.ToMapData(inputShape);
//mapDatas.Add(inputMap);

//// запустить каждый алгоритм
//var compAlgm = new ComparisionAlgorithm
//{
//    PointSimilarityMeasure = 100.0,
//    AngleSimilarityMeasure = 0.01
//};
//foreach(var pair in mapDatas[0].MapObjDictionary)
//{
//    var map1 = new MapData();
//    map1.MapObjDictionary.Add(pair.Key ,pair.Value);
//    foreach (var pair2 in mapDatas[1].MapObjDictionary)
//    {
//        var map2 = new MapData();
//        map2.MapObjDictionary.Add(pair2.Key ,pair2.Value);
//        var result = compAlgm.InfringementDetectionAlgorithmForLine(map1, map2);
//        if(result.RepetitionRate >10 )
//        {
//            var s = string.Format("{0}; {1} ; {2}", pair.Key, pair2.Key, result);
//            Save("rate.txt", s);
//        }
//    }
//}

//void Save(string fileName, string s)
//{
//    using var sw = new StreamWriter(fileName, true);
//    sw.WriteLine(s);
//}
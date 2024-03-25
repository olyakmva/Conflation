//using ComparisonLib;
//using DotSpatial.Data;
using AlgorithmsLibrary;
using SupportLib;

var realRate = GetFromFile("real.txt");
var accList = Get("rate.txt");
accList.Sort();
int idCount = (from t in accList select t.Id1).Distinct().Count();
Console.WriteLine(idCount);
Console.WriteLine(realRate.pairs.Count);
int truePositive = 0, trueNegative = 0, falsePositive = 0, falseNegative = 0;
int positiveLimit = 75, negativeLimit = 30, persentLimit=20;
foreach (var pair in realRate.pairs  )
{
    string info = string.Format("{0};{1}", pair.Key, pair.Value);
    var items = accList.FindAll(i => i.Id1 == pair.Key);
    if(items.Count==0)
    {
        Save("noRelation.txt", info); continue;
    }
    if(pair.Value.Id == -1)
    {
        foreach (var item in items)
        {
            if (item.Persent <= negativeLimit)
            {
                trueNegative++;
            }
            else falsePositive++;
            info += item.ToString(); 
        }
        Save ("falsePos.txt",info); continue;
    }
    var trueItem = items.FindAll(i => i.Id2 == pair.Value.Id); 
    if(trueItem.Count==0 )
    {
        foreach(var item in items)
            info+= item.ToString();
        Save("noRelation.txt", info); continue;
    }
    foreach(var item in trueItem)
    {
        if(Math.Abs(item.Persent- pair.Value.Persent) <=persentLimit )
        {
            truePositive++;
            info+= item.ToString();
            Save("true.txt", info); 
        }
    }
    items = items.Except(trueItem).ToList();
    if (items.Count > 0)
    {
        foreach (var item in items)
        {
            info += item.ToString();   
            if(item.Persent <= negativeLimit)
                falseNegative ++;
            else falsePositive ++;
        }
        Save("falsePos.txt", info);
    }
    accList.RemoveAll(i => i.Id1 == pair.Key); 
}
double fscore = (2.0 * truePositive) / (1.0 * falseNegative + falsePositive + 2.0 * truePositive);
Console.WriteLine(fscore);
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

void Save(string fileName, string s)
{
    using var sw = new StreamWriter(fileName, true);
    sw.WriteLine(s);
}
MapDataRelation GetFromFile(string fileName)
{
    var sr = new StreamReader(fileName );
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
    return realRate;

}
List<Rate> Get(string fileName)
{
    List<Rate> accList = new List<Rate>();
    var sr = new StreamReader("rate.txt");
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
        int id1 = int.Parse(strs[0]);
        int id2 = int.Parse(strs[1]);
        int p = (int)Math.Truncate(double.Parse(strs[2]));
        var rate = new Rate(id1, id2, p);
        accList.Add(rate);
    }
    sr.Close();
    return accList;
}
void RateFirst(List<Rate> accList, MapDataRelation realRate)
{
    int truePositive = 0, trueNegative = 0, falsePositive = 0, falseNegative = 0;
    int positiveLimit = 75, negativeLimit = 30;
    foreach (var item in accList)
    {
        if (realRate.pairs.ContainsKey(item.Id1))
        {
            var relation = realRate.pairs[item.Id1];
            string info = string.Format("{0} {1}", item, relation);
            if (item.Id2 == relation.Id)
            {
                if (item.Persent >= relation.Persent)
                {
                    truePositive++;
                    Save("tp.txt", info);
                }
                else if (((double)item.Persent / relation.Persent) * 100 >= positiveLimit)
                {
                    truePositive++;
                    Save("tp.txt", info);
                }
                else
                {
                    falsePositive++;
                    Save("fp.txt", info);
                }

            }
            else if (relation.Id == -1)
            {
                if (item.Persent >= positiveLimit)
                {
                    falsePositive++;
                    Save("fp.txt", info);
                }
                else if (item.Persent <= negativeLimit)
                {
                    trueNegative++;
                    Save("tn.txt", info);
                }
                else
                {
                    Console.Write(item);
                    Console.WriteLine(relation.ToString());
                    Save("no.txt", info);
                }
            }
            else if (item.Id2 != relation.Id)
            {
                if (item.Persent >= positiveLimit)
                {
                    falsePositive++;
                    Save("fp.txt", info);
                }
                else
                {
                    falseNegative++;
                    Save("fn.txt", info);
                }
            }
        }
        else
        {
            string no = string.Format("net dannix {0}", item);
            Save("no.txt", no);
        }
    }
    double fscore = (2.0 * truePositive) / (1.0 * falseNegative + falsePositive + 2.0 * truePositive);
    Console.WriteLine(fscore);
}

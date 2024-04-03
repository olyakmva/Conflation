using ComparisonLib;
using DotSpatial.Data;
using SupportLib;
using System.Text;

var realRate = GetFromFile("real.txt");
var accList = Get("rate.txt");
accList.Sort();
var ids = (from t in accList select t.Id1).Distinct().ToList();
Console.WriteLine(ids.Count);
var ids2  = (from t in realRate select t.Id1).Distinct().ToList();
Console.WriteLine(ids2.Count);
var diff = (from k in ids2 select k).Except(ids).ToList();
string diffId = string.Join(' ', diff);
Console.WriteLine(diffId);
int truePositive = 0, trueNegative = 0, falsePositive = 0, falseNegative = 0;
int positiveLimit = 75, negativeLimit = 30, persentLimit = 20;
var nomatch = (from p in realRate where p.Id2 == -1 select p.Id1).ToList();

foreach(var id in nomatch)
{
    var items = accList.FindAll(i=>i.Id1== id);
    if(items.Count==0)
    {
        trueNegative++;
        continue;
    }
    foreach(var item in items)
    {
        if (item.Persent < negativeLimit)
        {
            trueNegative++;
        }
        else
        {
            falsePositive++;
            Save("fpos.txt", item.ToString());
        }
    }
    accList.RemoveAll(i => i.Id1 == id);
}
var match = (from p in realRate where p.Id2 != -1 select p).ToList();

foreach (var rate in match  )
{
    string info = string.Format("{0}", rate.ToString());
    var items = accList.FindAll(i => i.Id1 == rate.Id1);
    if(items.Count==0)
    {
        Save("noRelation.txt", info); continue;
    }
    
    var trueItem = items.FindAll(i => i.Id2 == rate.Id2);
    if (trueItem.Count != 0)
    {
        foreach (var item in trueItem)
        {
            truePositive++;
            info += item.ToString();
        }
        Save("true.txt", info);
    }   
    accList.RemoveAll(i => i.Id1 == rate.Id1 && i.Id2==rate.Id2); 
}

foreach (var item in accList)
{
    if (item.Persent < negativeLimit)
    {
        trueNegative++;
    }
    else
    {
        falsePositive++;
        Save("fpos.txt", item.ToString());
    }
}

double fscore = (2.0 * truePositive) / (1.0 * falseNegative + falsePositive + 2.0 * truePositive);
Console.WriteLine(fscore);
Console .ReadLine ();


void Save(string fileName, string s)
{
    string dir = Path.Combine(Environment.CurrentDirectory, "output",fileName);   
    using var sw = new StreamWriter(dir, true);
    sw.WriteLine(s);
}
List<Rate> GetFromFile(string fileName)
{
    var sr = new StreamReader(fileName );
    var realRate = new List<Rate>();
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
        int id1 = int.Parse(strs[0]);
        int id2 = int.Parse(strs[1]);
        int persent = 0;
        if (strs.Length >= 3)
        {
            persent = int.Parse(strs[2]);         
        }
        realRate.Add(new Rate(id1, id2, persent));      
    }
    sr.Close();
    return realRate;
}
List<Rate> Get(string fileName)
{
    List<Rate> accList = new List<Rate>();
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    var sr = new StreamReader("rate.txt", Encoding.GetEncoding(1251));
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
        int id1 = int.Parse(strs[0]);
        int id2 = int.Parse(strs[1]);
        int p = (int)Math.Truncate(double.Parse(strs[2]));
        var rate = new Rate(id1, id2, p);
        if (strs.Length >= 4) 
        {
            rate.Name1 = strs[3]; 
        }
        if (strs.Length >= 5)
        {
            rate.Name2 = strs[4];
        }
        accList.Add(rate);
    }
    sr.Close();
    return accList;
}
Dictionary<int,string> GetNames(string fileName)
{
    var sr = new StreamReader(fileName);
    var names = new Dictionary<int,string>();
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
        int id1 = int.Parse(strs[0]);
        if(strs.Length >=2)
            names.Add(id1, strs[1]);
        else
        {
            names.Add(id1, string.Empty);
        }
        
    }
    sr.Close();
    return names;
}

void OtherAlgorithms()
{
    string path = "g:\\Каталог Оли\\Nauka\\MapGeneralization\\Grant2022\\Conflation\\KeyPointApp\\Data\\Voronez";
    //получить данные
    var mapDatas = new List<MapData>();
    string shapeFileName = Path.Combine(path, "hdrlin500_utf_merg.shp");
    var inputShape = FeatureSet.Open(shapeFileName);

    var inputMap = Converter.ToMapData(inputShape);
    mapDatas.Add(inputMap);
    shapeFileName = Path.Combine(path, "hdrlin1000utfmerg.shp");
    inputShape = FeatureSet.Open(shapeFileName);
    inputMap = Converter.ToMapData(inputShape);
    mapDatas.Add(inputMap);

    // запустить каждый алгоритм
    var compAlgm = new ComparisionAlgorithm
    {
        PointSimilarityMeasure = 100.0,
        AngleSimilarityMeasure = 0.01
    };
    foreach (var pair in mapDatas[0].MapObjDictionary)
    {
        var map1 = new MapData();
        map1.MapObjDictionary.Add(pair.Key, pair.Value);
        foreach (var pair2 in mapDatas[1].MapObjDictionary)
        {
            var map2 = new MapData();
            map2.MapObjDictionary.Add(pair2.Key, pair2.Value);
            var result = compAlgm.InfringementDetectionAlgorithmForLine(map1, map2);
            if (result.RepetitionRate > 10)
            {
                var s = string.Format("{0}; {1} ; {2}", pair.Key, pair2.Key, result);
                Save("rate.txt", s);
            }
        }
    }
}

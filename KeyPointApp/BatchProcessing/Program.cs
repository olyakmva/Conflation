using AlgorithmsLibrary;
using ConflationLib;
using DotSpatial.Data;
using SupportLib;
using System.Text;
// получаем данные
string path = "g:\\Каталог Оли\\Nauka\\MapGeneralization\\Data\\ClipUfa";
//получить данные
var mapDatas = new List<MapData>();
string shapeFileName = Path.Combine(path, "hdrlin1000.shp");
var inputShape = FeatureSet.Open(shapeFileName);
var inputMap = Converter.ToMapData(inputShape);
mapDatas.Add(inputMap);

shapeFileName = Path.Combine(path, "hdrlin500.shp");
inputShape = FeatureSet.Open(shapeFileName);
inputMap = Converter.ToMapData(inputShape);
mapDatas.Add(inputMap);
ISimplificationAlgm[] algms = new ISimplificationAlgm[]
{
    //new DouglasPeuckerAlgm(),
    //new SleeveFitAlgm(),
    new VisWhyattAlgmWithTolerance()
};

Func<List<double>, double>[] funcs1 = new Func<List<double>, double>[3];
funcs1[0] = new Func<List<double>, double>(GetMin);
funcs1[1] = new Func<List<double>, double>(Median);
funcs1[2] = new Func<List<double>, double>(FirstLast);



Func<List<double>, double>[] funcs2 = new Func<List<double>, double>[4];
funcs2[0] = new Func<List<double>, double>(GetMax);
funcs2[1] = new Func<List<double>, double>(Median);
funcs2[2] = new Func<List<double>, double>(DoubleMax);
funcs2[3] = new Func<List<double>, double>(DoubleMedian);

#region 

//// запустить каждый алгоритм
////ISimplificationAlgm[] algms = new ISimplificationAlgm[]
////{
////   new DouglasPeuckerAlgmWithCriterion(new PointPercentCriterion()),
////   new SleeveFitWithCriterion(new PointPercentCriterion()),
////   new VisWhyattAlgmWithPercent()
////};
//var reducedValues1 = new double[] { 1.25, 1.5, 2, 3, 4 };
////var reducedValues2 = new double[,] { { 2, 2.5, 3, 3.5 },
////                                    {3,3.5, 4, 4.5 },
////                                    { 4,4.5, 5, 5.5 },
////    { 5,6,7, 8 },{ 7,8,9,10 } };

//var paramValues = new List<int[]>
// {
//     new[]  { 50, 100, 150, 200 },
//     new[]  { 40, 80, 150, 200 },
//     new [] {1022588,3220780,10634603,28301575 }
// };
#endregion
string description = "n;1000k_times;500k_times;algName;Fscore;TP;TN;FP;FN;";
Save("res26_3.txt", description);
foreach (var algm in algms)
{
    var bendCharacterists = new BendCharacteristics();
    var dictionary = bendCharacterists.GetBendsCharacteristics(mapDatas[0]);

    algm.Options = new SimplificationAlgmParameters()
    {
         PointNumberGap=2.0
          
    };

    for(int i=0; i< funcs1.Length; i++)
    { 
        var in1 = mapDatas[0].Clone();
        foreach (var pair in in1.MapObjDictionary)
        {
            var pointsList = pair.Value;
            if (dictionary.ContainsKey(pair.Key))
            {
                var bends = dictionary[pair.Key];
                var heightList = (from b in bends select b.Height).ToList();
                heightList.Sort();
                algm.Options.Tolerance = Math.Pow(funcs1[i](heightList),2)/2;
                algm.Run(pointsList);
                string s = string.Format("id={0}  param = {1}", pair.Key, algm.Options.Tolerance);
                //Console.WriteLine(s);
                Save("resultVW.txt", s);
            }
        }
        Save("resultVW.txt", "--------------------------");

        var bendCharacterists2 = new BendCharacteristics();
        var dictionary2 = bendCharacterists2.GetBendsCharacteristics(mapDatas[1]);
        for (int j = 0; j < funcs2.Length; j++)
        {
            var in2 = mapDatas[1].Clone();
            foreach (var pair in in2.MapObjDictionary)
            {
                var pointsList = pair.Value;
                if (dictionary2.ContainsKey(pair.Key))
                {
                    var bends = dictionary2[pair.Key];
                    var heightList = (from b in bends select b.Height).ToList();
                    heightList.Sort();
                    algm.Options.Tolerance = Math.Pow(funcs2[j].Invoke(heightList),2)/2;
                    algm.Run(pointsList);
                    string s = string.Format("id={0}  param = {1}", pair.Key, algm.Options.Tolerance);
                   // Console.WriteLine(s);
                    Save("resultVW2.txt", s);
                }
            }
            Save("resultVW2.txt", "--------------------------");
            // bends
            double maxDistanceBetweenPoints = 800.0;
            var bendCharacteristics = new BendCharacteristics(in1, in2, maxDistanceBetweenPoints);
            bendCharacteristics.Run();
            bendCharacteristics.Save("rate.txt");
            var f = GetFscore();
            var s1 = string.Format("{0};{1};{2};{3};{4};{5};{6};", in1.Count, in2.Count, i,j, algm.ToString(), f.Item1, f.Item2);
            Save("res26_3.txt", s1);
        } 
    }
}

Console .ReadLine ();


double GetMin(List<double> values)
 {
    if (values.Count > 0)
    {
        return values[0];
    }
    return 0;
 }

double GetMax(List<double> values)
{
    if (values.Count > 0)
    {
        return values[^1];
    }
    return 0;
}

double GetAverage(List<double> values)
{
    if (values.Count > 0)
    {
        return values.Average();
    }
    return 0;
}

double FirstLast(List<double> values)
{
    if (values.Count > 0)
    {
        return (values[0] + values[^1])/2;
    }
    return 0;
}

double Median(List<double> values)
{
    if (values.Count > 0)
    {
        return values[values.Count/2] ;
    }
    return 0;
}

double DoubleMax(List<double> values)
{
    return 2*GetMax(values);
}

double DoubleMedian(List<double> values)
{
    return 2*Median(values);
}


(double,string)  GetFscore()
{
    var realRate = GetFromFile("real.txt");
    var accList = Get("rate.txt");
    accList.Sort();
    
    var ids = (from t in accList select t.Id1).Distinct().ToList();
    Console.WriteLine(ids.Count);
    var ids2 = (from t in realRate select t.Id1).Distinct().ToList();
    Console.WriteLine(ids2.Count);
    //var diff = (from k in ids2 select k).Except(ids).ToList();
    //string diffId = string.Join(' ', diff);
    //Console.WriteLine(diffId);
    int truePositive = 0, trueNegative = 0, falsePositive = 0, falseNegative = 0;
    int positiveLimit = 75, negativeLimit = 24, persentLimit = 20;
    var nomatch = (from p in realRate where p.Id2 == -1 select p.Id1).ToList();

    foreach (var id in nomatch)
    {
        var items = accList.FindAll(i => i.Id1 == id);
        if (items.Count == 0)
        {
            trueNegative++;
            continue;
        }
        foreach (var item in items)
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

    foreach (var rate in match)
    {
        string info = string.Format("{0}", rate.ToString());
        var items = accList.FindAll(i => i.Id1 == rate.Id1);
        if (items.Count == 0)
        {
            falseNegative++;
            Save("noRelation.txt", info); continue;
        }

        var trueItem = items.FindAll(i => i.Id2 == rate.Id2).FirstOrDefault();
        if (trueItem != null)
        {
            truePositive++;
            info += trueItem.ToString();
            Save("true.txt", info);
        }
        accList.RemoveAll(i => i.Id1 == rate.Id1 && i.Id2 == rate.Id2);
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
    string l = string.Format("{0};{1};{2};{3};", truePositive, trueNegative, falsePositive, falseNegative);
    double fscore = (2.0 * truePositive) / (1.0 * falseNegative + falsePositive + 2.0 * truePositive);
    Console.WriteLine(fscore);
    return (fscore,l);
}

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
    var sr = new StreamReader(fileName, Encoding.GetEncoding(1251));
    string line= sr.ReadLine();
    line = sr.ReadLine();
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

List<Rate> Get2(string fileName)
{
    List<Rate> accList = new List<Rate>();
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    var sr = new StreamReader(fileName, Encoding.GetEncoding(1251));
    string line = sr.ReadLine();
    line = sr.ReadLine();
    while ((line = sr.ReadLine()) != null)
    {
        var strs = line.Split(new char[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
        int id1 = int.Parse(strs[0]);
        int id2 = int.Parse(strs[1]);
        int p = 0;
        if (strs.Length >= 3)
        {
            p = (int)Math.Truncate(double.Parse(strs[2]));
        }
         
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




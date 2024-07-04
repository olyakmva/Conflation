using AlgorithmsLibrary;
using DotSpatial.Data;
using FrequencyAnalysisLib;
using SupportLib;


#region InputMap
//string path = "..\\..\\..\\..\\Data\\Gus";
//string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Init16K";
//string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Plus25";
//var mapDatas = new List<MapData>();
//string shapeFileName = Path.Combine(path, "hdrlin500.shp");
//string shapeFileName = Path.Combine(path, "hdrlin500_utf_m4.shp");
//var inputShape = FeatureSet.Open(shapeFileName);
//var inputMap = Converter.ToMapData(inputShape);

//Map m = new Map();
//m.Add(inputMap);
//FreqAlg alg = new FreqAlg(m);
//alg.Process();
//var watermark = alg.Watermark;
//foreach (var elem in watermark)
//{
//    Console.Write(elem+" ");
//}
//Console.WriteLine();
//alg.FrequencyRationing();
//foreach (var elem in watermark)
//{
//    Console.Write(elem + " ");
//}
//Console.WriteLine();
#endregion

//#region InputMapFromAllFolderAndSimplify

//string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Comparer1";
//var mapDatas = new List<MapData>();
//var files = Directory.GetFiles(path, "*.shp");
//Map m = new Map();

//ISimplificationAlgm algm = new DouglasPeuckerAlgmWithCriterion(new PointPercentCriterion());
//var p = new SimplificationAlgmParameters();
//p.Tolerance = 100;
//p.RemainingPercent = 4.0; //будет в два раза меньше точек - на 50%, если поставить 4.0 будет в 4 раза меньше точек
//algm.Options = p;
//algm.Options.PointNumberGap = 2.0;

//Map simplifying_map = new Map();

//foreach (var file in files)
//{
//    var tmp = file.Split("\\");
//    string shapeFileName = Path.Combine(path, tmp[tmp.Length - 1]);
//    var inputShape = FeatureSet.Open(shapeFileName);
//    var inputMap = Converter.ToMapData(inputShape);
//    m.Add(inputMap);

//    var simplifyingMapData = inputMap.Clone();
//    algm.Run(simplifyingMapData);
//    simplifying_map.Add(simplifyingMapData);
//}
//FreqAlg alg = new FreqAlg(m, 0.075);
//alg.Process();
//var watermark = alg.Watermark;
//foreach (var elem in watermark)
//{
//    Console.Write(elem + " ");
//}
//Console.WriteLine();
//alg.FrequencyRationing();
//foreach (var elem in watermark)
//{
//    Console.Write(elem + " ");
//}

//FreqAlg simplifying_alg = new FreqAlg(simplifying_map, 0.075);
//simplifying_alg.Process();
//var simplifying_watermark = simplifying_alg.Watermark;

//Console.WriteLine();
//foreach (var elem in simplifying_watermark)
//{
//    Console.Write(elem + " ");
//}
//Console.WriteLine();
//simplifying_alg.FrequencyRationing();
//foreach (var elem in simplifying_watermark)
//{
//    Console.Write(elem + " ");
//}
//Console.WriteLine();

//DataAnalysis d = new DataAnalysis();

//Console.WriteLine();
//Console.WriteLine("Result:");
//Console.WriteLine(d.Analysis(watermark, simplifying_watermark));
//Console.WriteLine("OutParam " + algm.Options.OutParam);

//#endregion

#region InputMapFromAllFolderAddinObject
string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Init16K";
var mapDatas = new List<MapData>();
var files = Directory.GetFiles(path, "*.shp");
Map m = new Map();

foreach (var file in files)
{
    var tmp = file.Split("\\");
    string shapeFileName = Path.Combine(path, tmp[tmp.Length - 1]);
    var inputShape = FeatureSet.Open(shapeFileName);
    var inputMap = Converter.ToMapData(inputShape);
    m.Add(inputMap);
}
FreqAlg alg = new FreqAlg(m, 0.075);
alg.Process();
var watermark = alg.Watermark;
foreach (var elem in watermark)
{
    Console.Write(elem + " ");
}
Console.WriteLine();
alg.FrequencyRationing();
foreach (var elem in watermark)
{
    Console.Write(elem + " ");
}
Console.WriteLine();
path = "..\\..\\..\\..\\Data\\DataForDescriptor\\ToCompare2";
mapDatas = new List<MapData>();
files = Directory.GetFiles(path, "*.shp");
Map newmap = new Map();

foreach (var file in files)
{
    var tmp = file.Split("\\");
    string shapeFileName = Path.Combine(path, tmp[tmp.Length - 1]);
    var inputShape = FeatureSet.Open(shapeFileName);
    var inputMap = Converter.ToMapData(inputShape);
    newmap.Add(inputMap);
}
FreqAlg newalg = new FreqAlg(newmap, 0.075);
newalg.Process();
var newwatermark = newalg.Watermark;
foreach (var elem in newwatermark)
{
    Console.Write(elem + " ");
}
Console.WriteLine();
newalg.FrequencyRationing();
foreach (var elem in newwatermark)
{
    Console.Write(elem + " ");
}

DataAnalysis d = new DataAnalysis();
Console.WriteLine();
Console.WriteLine("Result:");
Console.WriteLine(d.Analysis(watermark, newwatermark));
#endregion

#region OffsetInputMap
//var offset_mapdata = inputMap.MultiplyOffsetMapData(-100000000, 0.5);
//Map offset_map = new Map();
//offset_map.Add(offset_mapdata);
//FreqAlg offset_alg = new FreqAlg(offset_map);
//offset_alg.Process();
//var offset_watermark = offset_alg.Watermark;
//foreach (var elem in offset_watermark)
//{
//    Console.Write(elem + " ");
//}
//Console.WriteLine();
//offset_alg.FrequencyRationing();
//foreach (var elem in offset_watermark)
//{
//    Console.Write(elem + " ");
//}
#endregion

Console.ReadKey();

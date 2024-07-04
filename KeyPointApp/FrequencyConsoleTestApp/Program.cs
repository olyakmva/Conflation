using AlgorithmsLibrary;
using DotSpatial.Data;
using FrequencyAnalysisLib;
using SupportLib;


#region InputMap
//string path = "..\\..\\..\\..\\Data\\Gus";
//string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Init16K";
//string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Plus25";
//var mapDatas = new List<MapData>();
string shapeFileName = Path.Combine(path, "hdrlin500_utf_merg.shp");
var inputShape = FeatureSet.Open(shapeFileName);
var inputMap = Converter.ToMapData(inputShape);
#region InputMap
Map m = new Map();
m.Add(inputMap);
FreqAlg alg = new FreqAlg(m);
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

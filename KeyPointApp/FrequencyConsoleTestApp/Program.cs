using DotSpatial.Data;
using FrequencyAnalysisLib;
using SupportLib;

string path = "D:\\Курсовая 5 курс\\Conflation\\KeyPointApp\\Data\\Voronez";
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
    Console.Write(elem+" ");
}
Console.WriteLine();
alg.FrequencyRationing();
foreach (var elem in watermark)
{
    Console.Write(elem + " ");
}
Console.WriteLine();
#endregion

#region OffsetInputMap
var offset_mapdata = inputMap.MultiplyOffsetMapData(-100000000, 0.5);
Map offset_map = new Map();
offset_map.Add(offset_mapdata);
FreqAlg offset_alg = new FreqAlg(offset_map);
offset_alg.Process();
var offset_watermark = offset_alg.Watermark;
foreach (var elem in offset_watermark)
{
    Console.Write(elem + " ");
}
Console.WriteLine();
offset_alg.FrequencyRationing();
foreach (var elem in offset_watermark)
{
    Console.Write(elem + " ");
}
#endregion

DataAnalysis d = new DataAnalysis();

Console.WriteLine();
Console.WriteLine(d.Analysis(watermark, offset_watermark));

Console.ReadKey();

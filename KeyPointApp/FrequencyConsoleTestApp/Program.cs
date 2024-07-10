using AlgorithmsLibrary;
using DotSpatial.Data;
using FrequencyAnalysisLib;
using SupportLib;
using System.Runtime.Intrinsics.X86;

while (true)
{
    //Console.WriteLine("Введите отрезок интервала");
    //var q = Console.ReadLine();
    //var x = double.Parse(q);
    //var arr = new double[] { 1.25, 1.5, 2, 4 };
    var array = new double[] { 0.2, 0.1, 0.07, 0.05, 0.02, 0.01 };
    for (int c = 0; c < 6; c++)
    {
        #region InputMap
        ////string path = "..\\..\\..\\..\\Data\\Voronez";
        string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\ToCompare3";
        //string path = "..\\..\\..\\..\\Data\\DataForDescriptor\\Plus25";
        var mapDatas = new List<MapData>();
        string shapeFileName = Path.Combine(path, "admpol1000.shp");
        var inputShape = FeatureSet.Open(shapeFileName);
        var inputMap = Converter.ToMapData(inputShape);
        #endregion

        Map m = new Map();
        m.Add(inputMap);
        FreqAlg alg = new FreqAlg(m, array[c]);
        alg.Process();
        var watermark = alg.Watermark;
        alg.FrequencyRationing();

        #region OffsetInputMap
        //var offset_mapdata = inputMap.MultiplyOffsetMapData(100000000, 2);
        //Map offset_map = new Map();
        //offset_map.Add(offset_mapdata);
        //QuadFreqAlg offset_alg = new QuadFreqAlg(offset_map);
        //offset_alg.Process();
        //var offset_watermark = offset_alg.Watermark;

        //offset_alg.FrequencyRationing();

        //DataAnalysis d = new DataAnalysis();
        //Console.WriteLine("Result:");
        //Console.WriteLine(d.Analysis(watermark, offset_watermark));
        #endregion

        #region Plus
        //path = "..\\..\\..\\..\\Data\\DataForDescriptor\\500k\\plus75";
        //mapDatas = new List<MapData>();
        //var files = Directory.GetFiles(path, "*.shp");
        //Map newmap = new Map();

        //foreach (var file in files)
        //{
        //    var tmp = file.Split("\\");
        //    string newshapeFileName = Path.Combine(path, tmp[tmp.Length - 1]);
        //    var newinputShape = FeatureSet.Open(newshapeFileName);
        //    var newinputMap = Converter.ToMapData(newinputShape);
        //    newmap.Add(newinputMap);
        //}
        //QuadFreqAlg newalg = new QuadFreqAlg(newmap,0.07);
        //newalg.Process();
        //var newwatermark = newalg.Watermark;
        //newalg.FrequencyRationing();

        //var cnt = 0;
        //for (int i = 0; i < newwatermark.GetLength(0); i++)
        //{
        //    for (int j = 0; j < newwatermark.GetLength(1); j++)
        //    {
        //        if (watermark[i, j] == 0)
        //        {
        //            cnt++;
        //        }
        //    }
        //}

        //var cnt1 = 0;
        //for (int i = 0; i < newwatermark.GetLength(0); i++)
        //{
        //    for (int j = 0; j < newwatermark.GetLength(1); j++)
        //    {
        //        if (newwatermark[i, j] == 0)
        //        {
        //            cnt1++;
        //        }
        //    }
        //}
        //Console.WriteLine($"Cnt:= {cnt}");
        //Console.WriteLine($"Cnt1:= {cnt1}");

        //DataAnalysis d = new DataAnalysis();
        //Console.WriteLine("Result:");
        //Console.WriteLine(d.Analysis(watermark, newwatermark));
        #endregion

        #region Simplification
        //ISimplificationAlgm algm = new DouglasPeuckerAlgmWithCriterion(new PointPercentCriterion());
        //var p = new SimplificationAlgmParameters();
        //p.Tolerance = 100;
        //p.RemainingPercent = arr[c]; //будет в два раза меньше точек - на 50%, если поставить 4.0 будет в 4 раза меньше точек
        //algm.Options = p;
        //algm.Options.PointNumberGap = 2.0;
        //var files = Directory.GetFiles(path, "*.shp");

        //Map simplifying_map = new Map();

        //foreach (var file in files)
        //{
        //    var tmp = file.Split("\\");
        //    string newshapeFileName = Path.Combine(path, tmp[tmp.Length - 1]);
        //    var newinputShape = FeatureSet.Open(newshapeFileName);
        //    var newinputMap = Converter.ToMapData(newinputShape);

        //    var simplifyingMapData = newinputMap.Clone();
        //    algm.Run(simplifyingMapData);
        //    simplifying_map.Add(simplifyingMapData);
        //}

        //FreqAlg newalg = new FreqAlg(simplifying_map, x);
        //newalg.Process();
        //var newwatermark = newalg.Watermark;
        //newalg.FrequencyRationing();

        //DataAnalysis d = new DataAnalysis();
        //Console.WriteLine($"Result: Simplify:{arr[c]} Split:{x}");
        //Console.WriteLine(d.Analysis(watermark, newwatermark));
        #endregion

        path = "..\\..\\..\\..\\Data\\DataForDescriptor\\ToCompare4";
        mapDatas = new List<MapData>();
        var files = Directory.GetFiles(path, "*.shp");
        Map newmap = new Map();

        foreach (var file in files)
        {
            var tmp = file.Split("\\");
            string newshapeFileName = Path.Combine(path, tmp[tmp.Length - 1]);
            var newinputShape = FeatureSet.Open(newshapeFileName);
            var newinputMap = Converter.ToMapData(newinputShape);
            newmap.Add(newinputMap);
        }
        FreqAlg newalg = new FreqAlg(newmap, array[c]);
        newalg.Process();
        var newwatermark = newalg.Watermark;
        newalg.FrequencyRationing();

        DataAnalysis d = new DataAnalysis();
        Console.WriteLine($"Result: Split:{array[c]}");
        Console.WriteLine(d.Analysis(watermark, newwatermark));
    }
    Console.ReadKey();
    Console.Clear();
}
// прочитать файлы из заданного каталога
// для каждого файла сделать копию
// копию упростить с помощью одного из алгоритмов в цикле? но параметр алгоритма надо задать
// сравнить процент схожести  исходного и упрощенного
//сохранить результат
using AlgorithmsLibrary;
using ComparisonLib;
using DotSpatial.Data;
using SupportLib;


var dataPath1 = "C:\\Users\\Есения\\OneDrive\\Рабочий стол\\500";
var dataPath2 = "C:\\Users\\Есения\\OneDrive\\Рабочий стол\\1000";
DirectoryInfo dir1 = new DirectoryInfo(dataPath1);
DirectoryInfo dir2 = new DirectoryInfo(dataPath2);
var dataFiles1 = dir1.GetFiles("*.shp");
var dataFiles2 = dir2.GetFiles("*.shp");


for (int j = 0; j < dataFiles1.Length; j++)
{
    var inputShape1 = FeatureSet.Open(dataFiles1[j].FullName);
    var inputMap1 = Converter.ToMapData(inputShape1);
    var inputShape2 = FeatureSet.Open(dataFiles2[j].FullName);
    var inputMap2 = Converter.ToMapData(inputShape2);

    Console.WriteLine(dataFiles1[j].Name);
    Console.WriteLine(dataFiles2[j].Name);
    var compAlgm = new OnlyPolygonComparison();
    Console.WriteLine(compAlgm.AlgorithmForPolygon(inputMap1, inputMap2));
}
Console.ReadKey();

// ==================================ниже программа сравнения береговых линий========================

//ISimplificationAlgm[] algms =
//    {
//                    new DouglasPeuckerAlgmWithCriterion( new PointPercentCriterion()),
//                    new SleeveFitWithCriterion(new PointPercentCriterion()),
//                    new VisWhyattAlgmWithPercent(),
//                    new LiOpenshawWithCriterion(new PointPercentCriterion())
//    };
//    var paramValues = new List<int[]>
//    {
//                    new[] { 589, 1392, 3033, 5500 },
//                    new[] { 611,1323,2500, 5500 },
//                    new []{1022588,3220780,10634603,28301575 },
//                    new[] { 3539,7244,17117,35000}
//    };
//var ReducedValues = new double[] { 2 };
//string applicationPath = Environment.CurrentDirectory;
////var dataPath = Path.Combine(applicationPath, "data\\coast");
//var dataPath = "C:\\MyKursovaya\\Conflation\\KeyPointApp\\Data\\Coasts";
//DirectoryInfo dir = new DirectoryInfo(dataPath);
//var dataFiles = dir.GetFiles("*.shp");


//var saveFilePath = applicationPath;
//var sw = new StreamWriter(saveFilePath + "\\time1.txt", true);
//foreach (var file in dataFiles)
//{
//    var inputShape = FeatureSet.Open(file.FullName);
//    var inputMap = Converter.ToMapData(inputShape);

//    Console.WriteLine(file.Name);
//    var st = new System.Diagnostics.Stopwatch();
//    int i = 0;

//    foreach (var algm in algms)
//    {
//        int j = 0;
//        foreach (var k in ReducedValues)
//        {

//            var param = new SimplificationAlgmParameters
//            {
//                Tolerance = paramValues[i][j],
//                OutScale = paramValues[i][j],
//                PointNumberGap = 1,
//                RemainingPercent = k
//            };
//            j++;
//            var map = inputMap.Clone();
//            algm.Options = param;
//            st.Start();
//            algm.Run(map);
//            var compAlgm = new ComparisionAlgorithm
//            {
//                PointSimilarityMeasure = 1.0,
//                AngleSimilarityMeasure = 0.05
//            };
//            var result = compAlgm.InfringementDetectionAlgorithmForLine(inputMap, map);
//            st.Stop();
//            sw.Write("{0}; {1}; {2} ", file.Name, st.ElapsedMilliseconds, result);
//            st.Reset();
//            Console.WriteLine("{0} {1}",inputMap.Count, map.Count);
//        }
//        i++;
//        sw.WriteLine();
//    }
//}
//sw.Close();
using AlgorithmsLibrary;
using FrequencyAnalysisLib;
using NetTopologySuite.Geometries;
using SupportLib;
using System.Data;
using System.Runtime.CompilerServices;

namespace FrequencyAnalysisLibTestProject
{
    public class QuadAlgTest
    {
        [Fact]
        public void FreqAlgNotNullAfterCreating()
        {
            Map map = new Map();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            Assert.NotNull(freqAlg);
        }
        [Fact]
        public void FreqAlgMapIsNotNullAfterCreating()
        {
            Map map = new Map();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            Assert.NotNull(freqAlg.Curr_map);
        }
        [Fact]
        public void FreqAlgContainsMapAfterCreating()
        {
            Map map = new Map();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            Assert.Equal(freqAlg.Curr_map, map);
        }

        [Fact]
        public void GetProperSquareFromMap()
        {
            var map = CreateMap();
            double minX = 0, minY = 0, maxX = 5, maxY = 4;
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            MPoint LowLeft = new MPoint(minX, minY);
            MPoint UpLeft = new MPoint(minX, maxY);
            MPoint UpRight = new MPoint(maxX, maxY);
            MPoint LowRight = new MPoint(maxX, minY);
            Rectangle expected = new Rectangle(LowLeft, UpLeft, UpRight, LowRight);
            Assert.True(freqAlg.CellRectangle.RectangleEquals(expected));
        }

        [Fact]
        public void GetProperStepFromDefaultConstructor()
        {
            Map map = new Map();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            double expected = 0.1;
            Assert.Equal(expected, freqAlg.Curr_Step);
        }

        [Fact]
        public void GetProperStepFromStepEqualsPointFive()
        {
            Map map = new Map();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map, 0.5);
            double expected = 0.5;
            Assert.Equal(expected, freqAlg.Curr_Step);
        }
        [Fact]
        public void CountObjectsInCreateMapEqualsTwoAfterCreatingFreqAlg()
        {
            var map = CreateMap();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            int expected = 2;
            int count = freqAlg.Curr_map.GetMapObjItems().Count;
            Assert.Equal(expected, count);
        }

        [Theory]
        [InlineData(10, 0.1)]
        [InlineData(5, 0.2)]
        [InlineData(8, 0.125)]
        [InlineData(100, 0.01)]
        [InlineData(2, 0.5)]
        public void WatermarkHasCorrectLength(int expected, double step)
        {
            Map map = new Map();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map, step);
            Assert.Equal(expected, freqAlg.Watermark.GetLength(0));
            Assert.Equal(expected, freqAlg.Watermark.GetLength(1));
        }

        [Theory]
        [InlineData(3, 0.6, 0.75)]
        [InlineData(2, 0.4, 0.5)]
        [InlineData(0.5, 0.1, 0.125)]
        [InlineData(0.25, 0.05, 0.0625)]
        [InlineData(0.125, 0.025, 0.03125)]
        public void RecalculateOfCoordinatesCheck(double x, double expected_x, double expected_y)
        {
            Map map = CreateMap();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            MPoint point = new MPoint(x, x);
            MPoint newpoint = freqAlg.RecalculateOfCoordinates(point);
            Assert.Equal(expected_x, newpoint.X);
            Assert.Equal(expected_y, newpoint.Y);
        }
        [Fact]
        public void CountOfObjectsInMPointsMustBeEleven()
        {
            int expected = 11;
            Map map = CreateMap();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            freqAlg.Process();
            Assert.Equal(freqAlg.MPoints.Count, expected);
        }

        [Fact]
        public void ListOfFrequencуCheckInProcessMethodWithoutValues()
        {
            Map map = CreateMap();
            QuadFreqAlg freqAlg = new QuadFreqAlg(map);
            freqAlg.Process();
            List<double> expected = new List<double>() { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1 };
            for (int i = 0; i < freqAlg.FrequenciesX.Count - 1; i++)
            {
                Assert.Equal(expected[i], freqAlg.FrequenciesX[i].Start, 10);
                Assert.Equal(expected[i + 1], freqAlg.FrequenciesX[i].Finish, 10);

                Assert.Equal(expected[i], freqAlg.FrequenciesY[i].Start, 10);
                Assert.Equal(expected[i + 1], freqAlg.FrequenciesY[i].Finish, 10);
            }

        }

        //[Fact]
        //public void TestWatermarkInProcessMethodWithValues()
        //{
        //    double[,] expected = 
        //    { 
        //        { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1},
        //        { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 1, 0, 1, 1},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
        //        { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0} 
        //    };
        //    Map map = CreateMap();
        //    QuadFreqAlg freqAlg = new QuadFreqAlg(map);
        //    freqAlg.Process();
        //    Assert.Equal(freqAlg.Watermark, expected);
        //}
        //[Fact]
        //public void CheckFrequencyRationingWithValues()
        //{
        //    double[,] expected =
        //    {
        //        { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1},
        //        { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 1, 0, 1, 1},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
        //        { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        //        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        //    };
        //    Map map = CreateMap();
        //    QuadFreqAlg freqAlg = new QuadFreqAlg(map);
        //    freqAlg.Process();
        //    freqAlg.FrequencyRationing();
        //    Assert.Equal(expected, freqAlg.Watermark);
        //}

        [Fact]
        public void CheckDataAnalysisWithoutValues()
        {
            double[,] expected =
            {
                { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1},
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 1, 0, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            DataAnalysis tmp = new DataAnalysis();
            double expected_per = 100;

            Assert.Equal(tmp.Analysis(expected, expected), expected_per);
        }

        [Fact]
        public void CheckDataAnalysisWithValues()
        {
            Map map = CreateMap();
            FreqAlg alg = new FreqAlg(map);
            alg.Process();

            DataAnalysis tmp = new DataAnalysis();
            double expected = 100;

            Assert.Equal(tmp.Analysis(alg.Watermark, alg.Watermark), expected);
        }

        private static Map CreateMap()
        {
            Map map = new Map();
            var md1 = new MapData(GeometryType.Line);
            int objId = 1;
            const int objWeight = 1;
            md1.MapObjDictionary.Add(objId, new List<MapPoint>()
            {
                new MapPoint(0, 0, objId, objWeight),
                new MapPoint(1, 3, objId, objWeight),
                new MapPoint(3.5, 1.5, objId, objWeight),
                new MapPoint(5, 0.5, objId, objWeight),
                new MapPoint(5, 1.5, objId, objWeight)
            });
            map.Add(md1);
            var md2 = new MapData(GeometryType.Line);
            objId = 2;
            md2.MapObjDictionary.Add(objId, new List<MapPoint>
            {
                new MapPoint(2, 4, objId, objWeight),
                new MapPoint(4.5, 1.5, objId, objWeight),
                new MapPoint(4.4, 0.5, objId, objWeight),
                new MapPoint(2.5, 1, objId, objWeight),
                new MapPoint(3.5, 2.5, objId, objWeight),
                new MapPoint(2.9, 2.8, objId, objWeight)
            });
            map.Add(md2);
            return map;
        }
    }
}
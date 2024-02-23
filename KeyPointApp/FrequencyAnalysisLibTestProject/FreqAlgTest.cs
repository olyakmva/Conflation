using AlgorithmsLibrary;
using FrequencyAnalysisLib;
using NetTopologySuite.Geometries;
using SupportLib;
using System.Data;
using System.Runtime.CompilerServices;

namespace FrequencyAnalysisLibTestProject
{
    public class FreqAlgTest
    {
        [Fact]
        public void FreqAlgNotNullAfterCreating()
        {
            Map map = new Map();
            FreqAlg freqAlg = new FreqAlg(map);
            Assert.NotNull(freqAlg);
        }
        [Fact]
        public void FreqAlgMapIsNotNullAfterCreating()
        {
            Map map = new Map();
            FreqAlg freqAlg = new FreqAlg(map);
            Assert.NotNull(freqAlg.Curr_map);
        }
        [Fact]
        public void FreqAlgContainsMapAfterCreating()
        {
            Map map = new Map();
            FreqAlg freqAlg = new FreqAlg(map);
            Assert.Equal(freqAlg.Curr_map, map);
        }

        [Fact]
        public void GetProperSquareFromMap()
        {
            var map = CreateMap();
            double minX = 0, minY = 0, maxX = 5.5, maxY = 3;
            FreqAlg freqAlg = new FreqAlg(map);
            MPoint LowLeft = new MPoint(minX, minY);
            MPoint UpLeft = new MPoint(minX, maxY);
            MPoint UpRight = new MPoint(maxX, maxY);
            MPoint LowRight = new MPoint(maxX, minY);
            Rectangle expected = new Rectangle(LowLeft, UpLeft, UpRight, LowRight);
            Assert.True(freqAlg.CellRectangle.RectangleEquals(expected));
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
                new MapPoint(5.5, 0.5, objId, objWeight),
                new MapPoint(5, 1.5, objId, objWeight)
            });
            map.Add(md1);
            var md2 = new MapData(GeometryType.Line);
            objId = 2;
            md2.MapObjDictionary.Add(objId, new List<MapPoint>
            {
                new MapPoint(5, 3, objId, objWeight),
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
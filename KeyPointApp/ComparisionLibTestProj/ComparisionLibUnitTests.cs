using ComparisonLib;
using SupportLib;

namespace ComparisionLibTestProj
{
    public class ComparisionLibUnitTests
    {
        [Fact]
        public void RightNumberOfRepeatedPositions()
        {
            var md1 = new MapData() { Geometry = GeometryType.Line };
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
           
            var md2 = new MapData() { Geometry = GeometryType.Line };
            objId = 2;
            md2.MapObjDictionary.Add(objId, new List<MapPoint>
            {
                new MapPoint(0, 0, objId, objWeight),
                new MapPoint(0.4, 1.5, objId, objWeight),
                new MapPoint(1, 3, objId, objWeight),
                new MapPoint(2.5, 1, objId, objWeight),
                new MapPoint(3.5, 1.5, objId, objWeight),
                new MapPoint(5, 1.5, objId, objWeight)
            });
            var algm = new ComparisionAlgorithm();

            var NumberOfRepeatedPositions =algm.CountOfRepeatedLinesPositions(md1.GetMapObjItems(), md2.GetMapObjItems());
            double expected = 1;
           
            Assert.True( Math.Abs(expected - NumberOfRepeatedPositions) < double.Epsilon);

        }
    }
}
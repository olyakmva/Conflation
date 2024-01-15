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
        [Fact]
        public void RightNumberOfRepeatedPoints()
        {
            var md1 = new MapData() { Geometry = GeometryType.Point };
            int objId = 1;
            const int objWeight = 1;
            md1.MapObjDictionary.Add(objId, new List<MapPoint>()
            {
                new MapPoint(0, 0, objId, objWeight),
                new MapPoint(10, 20, objId, objWeight),
                new MapPoint(50, 0, objId, objWeight),                
                new MapPoint(7, 10.5, objId, objWeight)
            });

            var md2 = new MapData() { Geometry = GeometryType.Point };
            objId = 2;
            md2.MapObjDictionary.Add(objId, new List<MapPoint>
            {
                new MapPoint(15, 25, objId, objWeight),
                new MapPoint(0, 0, objId, objWeight),
                new MapPoint(37, 25, objId, objWeight),
                new MapPoint(100, 7, objId, objWeight),
            });
            var algm = new ComparisionAlgorithm();

            var NumberOfRepeatedPoints = algm.CountOfRepeatedPoints(md1.GetAllVertices(), md2.GetAllVertices());
            double expected = 2;

            Assert.True(Math.Abs(expected - NumberOfRepeatedPoints) < double.Epsilon);
        }
        [Fact]
        public void RightNumberOfRepeatedCenters()
        {
            int objId = 1;
            const int objWeight = 1;
            var list1 = new List<MapPoint>()
            {
                new MapPoint(10, 20, objId, objWeight),                
                new MapPoint(50, 0, objId, objWeight),
                new MapPoint(170, 10.5, objId, objWeight)
            };
            
            objId = 2;
            var list2 = new List<MapPoint>()
            {
                new MapPoint(0, 0, objId, objWeight),
                new MapPoint(37, 25, objId, objWeight),
                new MapPoint(100, 7, objId, objWeight),
            };

            var algm = new ComparisionAlgorithm();
            double expected = 0;
            var NumberOfRepeatedCentres = algm.CountRepeatedMeanCenters(list1, list2);
            Assert.True(Math.Abs(expected - NumberOfRepeatedCentres) < double.Epsilon);

            list1.Add(new MapPoint(0, 0, 1, objWeight));
            list2.Add(new MapPoint(10, 20, 2, objWeight));
            expected = 2;
            NumberOfRepeatedCentres = algm.CountRepeatedMeanCenters(list1, list2);
            Assert.True(Math.Abs(expected - NumberOfRepeatedCentres) < double.Epsilon);
        }
        [Fact]
        public void RightNumberOfRepeatedAngles()
        {           
            var list1 = new List<double>()
            {
                0,
                10.5,
                90,
                164
            };
            var list2 = new List<double>()
            {
               91,
               15,
               35
            };
            var algm = new ComparisionAlgorithm();
            double expected = 0;
            var NumberOfRepeatedCentres = algm.CountRepeatedIncludedAngles(list1, list2);
            Assert.True(Math.Abs(expected - NumberOfRepeatedCentres) < double.Epsilon);

            list2.Add(0.5);
            expected = 1;
            NumberOfRepeatedCentres = algm.CountRepeatedIncludedAngles(list1, list2);
            Assert.True(Math.Abs(expected - NumberOfRepeatedCentres) < double.Epsilon);
        }
        [Fact]
        public void RightCalculatingOfAngle()
        {
            var algm = new ComparisionAlgorithm();
            int objId = 1;
            const int objWeight = 1;
            var expected = 225;
            var real = algm.IncludedAngleCalculator(new MapPoint(0, 0,objId, objWeight), new MapPoint(1, 0, objId, objWeight), new MapPoint(2, 1, objId, objWeight));
            Assert.True(Math.Abs(expected - real) < double.Epsilon);
            real = algm.IncludedAngleCalculator(new MapPoint(0, 0, objId, objWeight), new MapPoint(1, 0, objId, objWeight), new MapPoint(2, -1, objId, objWeight));
            Assert.True(Math.Abs(expected - real) < double.Epsilon);
            real = algm.IncludedAngleCalculator(new MapPoint(2, -1, objId, objWeight), new MapPoint(1, 0, objId, objWeight), new MapPoint(0, 0, objId, objWeight));
            Assert.True(Math.Abs(expected - real) < double.Epsilon);
        }
        [Fact]
        public void RightNumberOfRepeatedCentres()
        {
            var md1 = new MapData() { Geometry = GeometryType.Polygon };
            const int objWeight = 1;
            md1.MapObjDictionary.Add(1, new List<MapPoint>()
            {
                new MapPoint(0, 0, 1, objWeight),
                new MapPoint(10, 3, 1, objWeight),
                new MapPoint(1, 8, 1, objWeight),
               
            });
            md1.MapObjDictionary.Add(2, new List<MapPoint>()
            {
                new MapPoint(0, 0, 2, objWeight),
                new MapPoint(5, 13, 2, objWeight),
                new MapPoint(11, 8, 2, objWeight),

            });           

            var algm = new ComparisionAlgorithm();
            double expected = 2;
            var NumberOfRepeatedCentres = algm.GetPolygonCenters(md1.GetMapObjItems());
            Assert.True(Math.Abs(expected - NumberOfRepeatedCentres.Count) < double.Epsilon);

        }
    }
}
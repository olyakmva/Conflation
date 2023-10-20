using SupportLib;

namespace ConflationLib
{
    public class KeyPoint
    {
        public PointVectorRelation PointVector1 {  get; set; }
        public PointVectorRelation PointVector2 { get; set; }

        public double AngleBetweenVectors { get; set; }
        public double LengthBetweenPoints
        {
            get
            {
                SupportLib.MapPoint point = PointVector1.StartPoint;
                return point.DistanceToVertex(PointVector2.StartPoint);
            }
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2:f2};{3:f2};",PointVector1,PointVector2,AngleBetweenVectors,LengthBetweenPoints);
        }
    }

    public class MapKeyPoint
    {
        public MapPoint Point1 { get; set; }
        public MapPoint Point2 { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Point1, Point2);
        }
    }

}

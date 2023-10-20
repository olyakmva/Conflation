using SupportLib;
using VectorLib;

namespace ConflationLib
{
    public class PointVectorRelation
    {
        public MapPoint StartPoint { get; set; }
        public Vector Vector { get; set; }
        public MapPoint PeakPoint { get; set; }
        public override string ToString()
        {
            return string.Format("{0};{1};{2}",StartPoint, PeakPoint,Vector);
        }
    }
}

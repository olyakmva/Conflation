using SupportLib;
using VectorLib;

namespace ConflationLib
{
    public class PointVectorRelation
    {
        public MapPoint Point { get; set; }
        public Vector Vector { get; set; }
        public override string ToString()
        {
            return string.Format("{0};{1}",Point,Vector);
        }
    }
}

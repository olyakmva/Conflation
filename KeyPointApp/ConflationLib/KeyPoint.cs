namespace ConflationLib
{
    public class KeyPoint
    {
        public PointVectorRelation PointVector1 {  get; set; }
        public PointVectorRelation PointVector2 { get; set; }

        public double AngleBetweenVectors { get; set; }
        public double LengthBetweenPoints 
        { 
            get { return PointVector1.Point.DistanceToVertex(PointVector2.Point); }
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2:f2};{3:f2};",PointVector1,PointVector2,AngleBetweenVectors,LengthBetweenPoints);
        }
    }
}

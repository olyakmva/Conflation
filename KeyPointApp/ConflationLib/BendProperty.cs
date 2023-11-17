using SupportLib;

namespace ConflationLib
{
    public class BendProperty
    {
        public MapPoint StartPoint { get; set; }
        public MapPoint PeakPoint { get; set; }
        public bool Orientation { get; set; }
        public override string ToString()
        {
            return string.Format("{0};{1};{2}", StartPoint, PeakPoint, Orientation);
        }
    }
}

namespace ComparisonLib
{
    public class LineMapComparison //: RLineString = [RLSF, RLSA, RLSC]; RT = max [RLSF, RLSA, RLSC];
    {
        public double RepetitionRate { get; set; }
        public double LineFeatureRepetitionRate { get; set; }
        public double LineFeatureIncludedAngleRepetitionRate { get; set; }
        public double LineFeatureVertexRepetitionRate { get; set; }


        public LineMapComparison(double Rt, double RLSF, double RLSA, double RLSC)
        {
            RepetitionRate = Rt;
            LineFeatureRepetitionRate = RLSF;
            LineFeatureIncludedAngleRepetitionRate = RLSA;
            LineFeatureVertexRepetitionRate = RLSC;
        }
    }
}

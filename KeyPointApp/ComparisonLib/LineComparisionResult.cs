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
            RepetitionRate = Math.Round(Rt,1);
            LineFeatureRepetitionRate = Math.Round(RLSF,1);
            LineFeatureIncludedAngleRepetitionRate = Math.Round(RLSA, 1);
            LineFeatureVertexRepetitionRate = Math.Round(RLSC, 1);
        }
        public override string ToString()
        {
            return $"RepetitionRate={RepetitionRate};LineFeatureRepetitionRate={LineFeatureRepetitionRate};"+
                $"LineIncludedAngleRepetitionRate={LineFeatureIncludedAngleRepetitionRate};"+
                $"LineVertexRepetitionRate={LineFeatureVertexRepetitionRate};";
        }
    }
}

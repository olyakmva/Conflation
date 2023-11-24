namespace ComparisonLib
{
    public class PolygonMapComparison //:RPolygon = [RPNF, RPNA, RPNC]; RT = max [RPNF, RPNA, RPNC];
    {
        public double RepetitionRate;
        public double PolygonFeatureRepetitionRate;
        public double PolygonFeatureIncludedAngleRepetitionRate;
        public double PolygonFeatureVertexRepetitionRate;

        public PolygonMapComparison(double Rt, double RPNF, double RPNA, double RPNC)
        {
            RepetitionRate = Rt;
            PolygonFeatureRepetitionRate = RPNF;
            PolygonFeatureIncludedAngleRepetitionRate = RPNA;
            PolygonFeatureVertexRepetitionRate = RPNC;
        }
    }
}

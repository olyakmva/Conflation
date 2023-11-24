namespace ComparisonLib
{
    public class PointMapComparison //RPoint = [RPTF, RPTA]; RT = max [RPTF, RPTA]
    {
        public double RepetitionRate; //RT is the repetition rate of vector data нужно ли содержание этой характеристики?
        public double PointFeatureRepetitionRate; //Rptf
        public double PointFeatureIncludedAngleRepetitionRate; //Rpta 
        public PointMapComparison(double Rt, double Rptf, double Rpta)
        {
            RepetitionRate = Rt;
            PointFeatureRepetitionRate = Rptf;
            PointFeatureIncludedAngleRepetitionRate = Rpta;
        }

    }
}

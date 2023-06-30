using AlgorithmsLibrary;
using SupportLib;

namespace ConflationLib
{
    public class Characteristic
    {
        public int Count { get; private set; }
        public List<double> WeightedLength { get;  set; }
        public List<double> Angles { get;  set; }
        public List<double> TempWeightedLength { get; set; }
        public List<double> TempAngles { get; set; }
        public List<MapPoint> Points { get; set; }
        public Characteristic() 
        { 
            WeightedLength = new List<double>();
            Angles = new List<double>();
            Count = 0;
            TempWeightedLength= new List<double>();
            TempAngles = new List<double>();
            Points = new List<MapPoint>();
        }
        public Characteristic( List<double> weightedLength, List<double> angles,List<MapPoint> pts)
        {
            Count = weightedLength.Count;
            WeightedLength = weightedLength;
            Angles = angles;
            Points = pts;
            TempWeightedLength = new List<double>();
            TempAngles = new List<double>();
        }

        public (double,double) DistanceTo(Characteristic other)
        {
            double distance = 0;
            double angle = 0;
            if(Count== other.Count)
            {
                for(int i = 0; i < Count; i++) 
                {
                    distance += Math.Pow((WeightedLength[i] - other.WeightedLength[i]), 2);
                    angle += Math.Pow(Angles[i] - other.Angles[i], 2);
                }
                distance =Math.Round( Math.Sqrt(distance),3);
                angle = Math.Round(Math.Sqrt(angle),3);
                return (distance, angle);
            }
            if(Count> other.Count)
            {
                DecreaseCount(Count-other.Count);
                for (int i = 0; i < other.Count; i++)
                {
                    distance += Math.Pow((TempWeightedLength[i] - other.WeightedLength[i]), 2);
                    angle += Math.Pow(TempAngles[i] - other.Angles[i], 2);
                }
                distance = Math.Round(Math.Sqrt(distance), 3);
                angle = Math.Round(Math.Sqrt(angle), 3);
                return (distance, angle);
            }
            other.DecreaseCount(other.Count - Count);
            for (int i = 0; i < Count; i++)
            {
                distance += Math.Pow(WeightedLength[i] - other.TempWeightedLength[i], 2);
                angle += Math.Pow(Angles[i] - other.TempAngles[i], 2);
            }
            distance = Math.Round(Math.Sqrt(distance), 3);
            angle = Math.Round(Math.Sqrt(angle), 3);
            return (distance, angle);
        }
        public void DecreaseCount( int gap)
        {
            TempAngles= new List<double>();
            TempAngles.AddRange(Angles);
            TempWeightedLength= new List<double>();
            TempWeightedLength.AddRange(WeightedLength);
            var pts = new List<MapPoint>();
            pts.AddRange(Points);
            for(int j = 0; j<gap; j++)
            {
                double min = TempWeightedLength.Min();
                int index = TempWeightedLength.FindIndex(x => x == min);
                if (index == pts.Count - 1)
                    pts.RemoveAt(index - 1);
                else pts.RemoveAt(index + 1);
                double len = 0;
                for (var i = 0; i < pts.Count - 1; i++)
                {
                    len += pts[i].DistanceToVertex(pts[i + 1]);
                }
                TempWeightedLength.Clear();
                TempAngles.Clear();
                var osX = new Line() { A = 0, B = 1, C = 0 };
                for (var i = 0; i < pts.Count - 1; i++)
                {
                    TempWeightedLength.Add(pts[i].DistanceToVertex(pts[i + 1]) / len);
                    var line = new Line(pts[i], pts[i + 1]);
                    TempAngles.Add(line.GetAngle(osX));
                }
            }

        }
        public override string ToString()
        {
            string res = "";
            foreach (var l in WeightedLength)
            {
                res += Math.Round(l,2) + ";";
            }
            foreach(var a in Angles)
                res+= Math.Round(a, 2)+";";
            return res;
        }
    }
}

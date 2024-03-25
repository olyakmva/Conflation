namespace SupportLib
{
    [Serializable]
    public  class MapDataRelation
    {
        public Dictionary<int, Relation > pairs = new Dictionary<int, Relation>();
       
        public MapDataRelation ()
        {
        }
    }
    [Serializable]
    public class Relation
    {
        public int Id { get; set; }
        public int Persent { get; set; }
        public Relation (int id, int persent)
        {
            Id = id;
            Persent = persent;
        }
        public Relation ()
        { 
        }
        public override string ToString()
        {
            return $" {Id} ;{Persent};";
        }
    }
    public class Rate:IComparable<Rate>
    {
        public int Id1 { get; set; } = 0;
        public int Id2 { get; set; } = 0;
        public int Persent { get; set; } = 0;
        public Rate(int id, int id2, int persent)
        {
            Id1 = id;
            Id2 = id2;
            Persent = persent;
        }
        public override string ToString()
        {
            return $"{Id1}; {Id2} ;{Persent}; ";
        }
        public int CompareTo(Rate? other)
        {
            if(other == null) return 1;
            if(other.Id1 != Id1) return Id1.CompareTo(other.Id1);
            return Persent.CompareTo(other.Persent);
        }
    }
}

using DotSpatial.Data;
using NetTopologySuite.Geometries;


namespace SupportLib
{
    public static class Converter
    {
        public static MapData? ToMapData(IFeatureSet fSet)
        {
            var list = fSet.Features;
            if (list.Count == 0)
                return null;
               
            var map = new MapData();
            foreach (var item in list)
            {
                var shape = item.ToShape();
                var points = new List<MapPoint>();
                for(var t=0; t< shape.Vertices.Length;t+=2)
                {
                    var p = new MapPoint(shape.Vertices[t], shape.Vertices[t+1], item.Fid, 1.0);
                    points.Add(p);
                }
                map.MapObjDictionary.Add(item.Fid, points);
            }
            return map;
        }

        public static IFeatureSet ToShape(MapData map)
        {
            
            FeatureType featureType = FeatureType.Line;
            FeatureSet fs = new(featureType);
            foreach (var pairList in map.MapObjDictionary)
            {
                Coordinate[] coord = new Coordinate[pairList.Value.Count];
                for (int i = 0; i < pairList.Value.Count; i++)
                {
                    coord[i] = new Coordinate(pairList.Value[i].X, pairList.Value[i].Y);
                }
                var f = new Feature(featureType,coord);
                fs.Features.Add(f);
            }
            return fs;
        }
    }
}

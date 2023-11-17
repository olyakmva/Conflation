﻿using SupportLib;
using AlgorithmsLibrary;
using VectorLib;
using System.Runtime.Serialization.Formatters;
using System.Drawing;
using System.Numerics;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.NTSExtension;
using NetTopologySuite.Triangulate.QuadEdge;
using System.Security.Cryptography;

namespace ComparisonLib
{
    public class PointMapComparison //RPoint = [RPTF, RPTA]; RT = max [RPTF, RPTA];
    {
        double repetitionRate; //RT is the repetition rate of vector data нужно ли содержание этой характеристики?
        double pointFeatureRepetitionRate; //Rptf
        double pointFeatureIncludedAngleRepetitionRate; //Rpta 
        public PointMapComparison(double Rt, double Rptf, double Rpta)
        {
            repetitionRate = Rt;
            pointFeatureRepetitionRate = Rptf;
            pointFeatureIncludedAngleRepetitionRate = Rpta;
        }

    }
    public class LineMapComparison //: RLineString = [RLSF, RLSA, RLSC]; RT = max [RLSF, RLSA, RLSC];
    {
        double repetitionRate;
        double lineFeatureRepetitionRate;
        double lineFeatureIncludedAngleRepetitionRate;
        double lineFeatureVertexRepetitionRate;
        //double repetitionRate; //RT

        public LineMapComparison(double Rt, double RLSF,double RLSA, double RLSC)
        {
            repetitionRate = Rt;
            lineFeatureRepetitionRate = RLSF;
            lineFeatureIncludedAngleRepetitionRate= RLSA;
            lineFeatureVertexRepetitionRate= RLSC;
        }
    }
    public class PolygonMapComparison //:RPolygon = [RPNF, RPNA, RPNC]; RT = max [RPNF, RPNA, RPNC];
    {
        double repetitionRate;
        double polygonFeatureRepetitionRate;
        double polygonFeatureIncludedAngleRepetitionRate;
        double polygonFeatureVertexRepetitionRate;
        //double repetitionRate; //RT

        public PolygonMapComparison(double Rt,double RPNF, double RPNA, double RPNC)
        {
            repetitionRate=Rt;
            polygonFeatureRepetitionRate = RPNF;
            polygonFeatureIncludedAngleRepetitionRate = RPNA;
            polygonFeatureVertexRepetitionRate = RPNC;
        }
    }
    public class CompAlg
    {        
        public double AzAngleCalculator(MapPoint mp1, MapPoint mp2) //azimuth angle calculation s in different quadrants
        {
            double alfa = Math.Atan((mp2.Y - mp1.Y) / (mp2.X - mp1.X)); //calculating angle between the line segment()two points P and the x-axis
            if (mp1.X >= mp2.X)
                return 270 - alfa; // when P is in the sec ond and third quadrants
            else return 90 - alfa; // 
        }
        public double IncludedAngleCalculator(MapPoint mp1, MapPoint mp2, MapPoint mp3) //calculating included angle for line consisting of 2 segments(tree points)
        {
            double segmentAngle1 = AzAngleCalculator(mp1, mp2);
            double segmentAngle2 = AzAngleCalculator(mp2, mp3);
            if (segmentAngle1 > segmentAngle2)
                return segmentAngle1 - segmentAngle2 + 180;
            else return segmentAngle2 - segmentAngle1 + 180;
        }
        public MapPoint MeanCenterCalculator(MapObjItem item)//calculating center coordinates of polygon by list of points
        {
            List<MapPoint> points = item.Points;
            double x = 0;
            double y = 0;
            foreach (MapPoint mp in points) //передалать метод на создание точки!!!!
            {
                x += mp.X;
                y += mp.Y;
            }
            x = x / points.Count();
            y = y / points.Count();
            return new MapPoint(x, y,item.Id,1);//в ответ передаю новую точку, индекс оставляю и вес присвоила по умолчанию 0 и 1
        }
        public PointMapComparison InfringementDetectionAlgorithmForPoint (MapData md1, MapData md2) //Do this method if geometry of map is Point
        {            
            
            List<MapPoint> PointsOFMap1 = md1.GetAllVertices();  // Dr
            List<MapPoint> PointsOFMap2 = md2.GetAllVertices();  // Dt

            double MinCapacityOfPoints = Math.Min(PointsOFMap1.Count, PointsOFMap2.Count);               //Fbc Feature cardinality
            double NumberOfRepeatedFeatures = CountOfRepeatedPoints(PointsOFMap1, PointsOFMap2);        //Frc Number of repeated features 

            List<double> ValueOfAngles1 = FormingAnglesByPoints(PointsOFMap1);   //Ar 
            List<double> ValueOfAngles2 = FormingAnglesByPoints(PointsOFMap2);   //At
            double MinCapacityOfAngles = Math.Min(ValueOfAngles1.Count, ValueOfAngles2.Count);                       //Abc  Angle cardinality
            double NumberOfRepeatedIncludedAngles = CountRepeatedIncludedAngles(ValueOfAngles1, ValueOfAngles1);  //Farc Number of repeated included angles

            double PointFeatureRepetitionRate = (NumberOfRepeatedFeatures / MinCapacityOfPoints) * 100; //Rptf =(Frc/Fbc)*100%
            double PointFeatureIncludedAngleRepetitionRate = (NumberOfRepeatedIncludedAngles / MinCapacityOfAngles) * 100; //Rpta =(Farc/Abc)*100%
            double RepetitionRate = Math.Max(PointFeatureRepetitionRate, PointFeatureIncludedAngleRepetitionRate);
            return new PointMapComparison(RepetitionRate, PointFeatureRepetitionRate, PointFeatureIncludedAngleRepetitionRate);
        }
        public LineMapComparison InfringementDetectionAlgorithmForLine(MapData md1, MapData md2) //Do this method if geometry of map is Line
        {
            //List<MapObjItem> MapItems1 = md1.GetMapObjItems(); //Dr         working with objItems(not points) because we need sttart and end position
            //List<MapObjItem> MapItems2 = md2.GetMapObjItems(); //Dt

            double MinCapacityOfLines = Math.Min(md1.MapObjDictionary.Count, md2.MapObjDictionary.Count);   //Fbc
            double NumberOfRepeatedPositions = CountOfRepeatedLinesPositions(md1.GetMapObjItems(),md2.GetMapObjItems()); //Frc - Number of repeated features

            List<MapPoint> PointsOFMap1 = md1.GetAllVertices();  // Cr
            List<MapPoint> PointsOFMap2 = md2.GetAllVertices();  // Ct
            
            List<double> ValueOfAngles1 = FormingAnglesByPoints(md1.GetMapObjItems());   //Ar 
            List<double> ValueOfAngles2 = FormingAnglesByPoints(md1.GetMapObjItems());   //At
            double MinCapacityOfAngles = Math.Min(ValueOfAngles1.Count, ValueOfAngles2.Count);                       //Abc  Angle cardinality
            double NumberOfRepeatedIncludedAngles = CountRepeatedIncludedAngles(ValueOfAngles1, ValueOfAngles1);  //Farc Number of repeated included angles

            double MinCapacityOfPoints = Math.Min(PointsOFMap1.Count, PointsOFMap2.Count);               //Cbc - smaller cardinality
            double NumberOfRepeatedVertices = CountOfRepeatedPoints(PointsOFMap1, PointsOFMap2);        //Fcrc - Number of repeated vertices

            double LineFeatureRepetitionRate = (NumberOfRepeatedPositions / MinCapacityOfLines) * 100; //Rlsf=(Frc/Fbc)*100%
            double LineFeatureIincludedAngleRepetitionRate = (NumberOfRepeatedIncludedAngles/ MinCapacityOfAngles)*100;//Rlsa=(Farc/Abc)*100%
            double LineFeatureVertexRepetitionRate = (NumberOfRepeatedVertices / MinCapacityOfPoints) * 100;//Rlsc=(Fcrc/Cbc)*100%
            double RepetitionRate = Math.Max(LineFeatureRepetitionRate,Math.Max(LineFeatureIincludedAngleRepetitionRate, LineFeatureVertexRepetitionRate));
            return (new LineMapComparison(RepetitionRate, LineFeatureRepetitionRate, LineFeatureIincludedAngleRepetitionRate, LineFeatureVertexRepetitionRate));
        }
        public PolygonMapComparison InfringementDetectionAlgorithmForPolygon(MapData md1, MapData md2) //Do this method if geometry of map is Polygon
        {
            //List<MapObjItem> MapItems1 = md1.GetMapObjItems(); //Dr      
            //List<MapObjItem> MapItems2 = md2.GetMapObjItems(); //Dt

            List<MapPoint> MeanCenters1 = GetPolygonCenters(md1.GetMapObjItems()); //Mr
            List<MapPoint> MeanCenters2 = GetPolygonCenters(md2.GetMapObjItems()); //Mt
            double MinCapacityOfCenters = Math.Min(MeanCenters1.Count, MeanCenters2.Count); //Mbc
            double NumberOfRepeatedMeanCenters = CountRepeatedMeanCenters(MeanCenters1, MeanCenters2) ;//Frc

            List<MapPoint> PointsOFMap1 = md1.GetAllVertices();  // Cr
            List<MapPoint> PointsOFMap2 = md2.GetAllVertices();  // Ct
            
            double MinCapacityOfPoints = Math.Min(PointsOFMap1.Count, PointsOFMap2.Count); //Cbc
            double NumberOfRepeatedVertices = CountOfRepeatedPoints(PointsOFMap1, PointsOFMap2); //Fcrc

            List<double> ValueOfAngles1 = FormingAnglesByPoints(PointsOFMap1);   //Ar 
            List<double> ValueOfAngles2 = FormingAnglesByPoints(PointsOFMap2);   //At
            double MinCapacityOfAngles = Math.Min(ValueOfAngles1.Count, ValueOfAngles2.Count);                       //Abc  Angle cardinality
            double NumberOfRepeatedIncludedAngles = CountRepeatedIncludedAngles(ValueOfAngles1, ValueOfAngles1);  //Farc Number of repeated included angles


            double PolygonFeatureRepetitionRate = (NumberOfRepeatedMeanCenters / MinCapacityOfCenters) * 100; //Rlsf=(Frc/Mbc)*100%
            double PolygonFeatureIincludedAngleRepetitionRate = (NumberOfRepeatedIncludedAngles / MinCapacityOfAngles) * 100; //Rpna=(Farc/Abc)*100%
            double PolygonFeatureVertexRepetitionRate = (NumberOfRepeatedVertices / MinCapacityOfPoints) * 100;//Rpnc=(Fcrc/Cbc)*100%
            double RepetitionRate = Math.Max(PolygonFeatureRepetitionRate, Math.Max(PolygonFeatureIincludedAngleRepetitionRate, PolygonFeatureVertexRepetitionRate));
            return new PolygonMapComparison(RepetitionRate, PolygonFeatureRepetitionRate, PolygonFeatureIincludedAngleRepetitionRate, PolygonFeatureVertexRepetitionRate);
        }
        public double CountRepeatedIncludedAngles(List<double> ValueOfAngles1, List<double> ValueOfAngles2) //counting repeted angles from lists of two maps
        {
            double count = 0;
            
            for(int i = 0; i<ValueOfAngles1.Count; i++)
            {
                for(int j =0; j<ValueOfAngles2.Count; j++)
                {
                    if (ValueOfAngles1[i]== ValueOfAngles2[j])
                        count++;
                }
            }
            return count;
        }
        public double CountOfRepeatedPoints(List<MapPoint> ObjItems1, List<MapPoint> ObjItems2) //counting repered points 
        {
            double count = 0;
            //foreach (MapObjItem obj1 in ObjItems1)
            
                //foreach (MapObjItem obj2 in ObjItems2)
                
                    //if (obj1.Id == obj2.Id)//нужно ли это условие или просто сравниваем все точки вподряд
                    
                        foreach (MapPoint mp1 in ObjItems1)
                        {
                            foreach (MapPoint mp2 in ObjItems2)
                            {
                                if (mp1.Equals(mp2))  //сравнение только по координатам через equels
                                    count++;
                            }
                        }
                    
                
            
            return count;
        }
        public double CountRepeatedMeanCenters(List<MapPoint> centers1, List<MapPoint> centers2) //counting repered mean centers of polygon
        {
            double count = 0;
            for (int i = 0; i < centers1.Count; i++)
            {
                for (int j = 0; j < centers2.Count; j++)
                {
                    if (centers1[i].Equals(centers2[j]))
                        count++;
                }
            }
            return count;
        }
        public double CountOfRepeatedLinesPositions(List<MapObjItem> MapItems1, List<MapObjItem> MapItems2)
        {
            double count = 0;
            foreach(MapObjItem mpObj1 in MapItems1)
            {
                foreach(MapObjItem mpObj2 in MapItems2)
                {
                    if ((mpObj1.Points[0].Equals(mpObj2.Points[0])) && (mpObj1.Points[mpObj1.Points.Count-1].Equals(mpObj2.Points[mpObj2.Points.Count - 1])))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public List<MapPoint> GetPolygonCenters(List<MapObjItem> MapItems)
        {
            List<MapPoint> centers = new List<MapPoint>();
            foreach (MapObjItem mpObj in MapItems)
            {
                centers.Add(MeanCenterCalculator(mpObj));
            }
            return centers;
        }
        public List<double> FormingAnglesByPoints(List<MapObjItem> mapObjs) //helpes to form angles between points from list
        {
            List<double> ValueOfAngles = new List<double>();
            foreach (MapObjItem mpObj in mapObjs)
            {
                for (int i = 0; i < mpObj.Points.Count; i++) //глупый перебор всевозможных углов между точками, можно сделать быстрее?
                {
                    for (int j = i + 1; j < mpObj.Points.Count - 1; j++) // соединение точек внутри одного объекта или карты в целом
                    {
                        for (int k = j + 1; k < mpObj.Points.Count - 2; k++)
                        {
                            ValueOfAngles.Add(IncludedAngleCalculator(mpObj.Points[i], mpObj.Points[j], mpObj.Points[k]));
                        }
                    }
                }
            }
            
            return ValueOfAngles;
        }
    }
}
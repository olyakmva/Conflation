using AlgorithmsLibrary;
using SupportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysisLib
{
    abstract public class BaseFreqAlg
    {
        private Map curr_map;

        private Rectangle cellRectangle;

        private double curr_step;

        private List<MPoint> mPoints = new List<MPoint>();

        public BaseFreqAlg() { }
        public BaseFreqAlg(Map map) : this(map, 0.1) { }
        public BaseFreqAlg(Map map, double step)
        {
            curr_map = map;
            cellRectangle = GetRectangle(map);
            curr_step = step;
            int n = (int)(1 / step);
        }

        public Map Curr_map
        {
            get
            {
                return curr_map;
            }
            set
            {
                curr_map = value;
            }
        }

        public Rectangle CellRectangle
        {
            get
            {
                return cellRectangle;
            }
            set 
            { 
                cellRectangle = value; 
            }
        }

        public double Curr_Step
        {
            get
            {
                return curr_step;
            }
            set
            {
                curr_step = value;
            }
        }

        public List<MPoint> MPoints
        {
            get
            {
                return mPoints;
            }
            set
            {
                mPoints = value;
            }
        }
        abstract public MPoint RecalculateOfCoordinates(MPoint point);

        abstract public void Process();

        abstract public void FrequencyRationing();

        public List<MPoint> AddingMPointsToList()
        {
            var result = new List<MPoint>();
            var objects = curr_map.GetMapObjItems();
            foreach (var obj in objects)
            {
                for (int i = 0; i < obj.Points.Count; i++)
                {
                    var point = obj.Points[i];
                    MPoint mpoint = new MPoint(point.X, point.Y);
                    MPoint new_mpoint = RecalculateOfCoordinates(mpoint);
                    result.Add(new_mpoint);
                }
            }
            return result;
        }
        abstract public Rectangle GetRectangle(Map m);
    }
}

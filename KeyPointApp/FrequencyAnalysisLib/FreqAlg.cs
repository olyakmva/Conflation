using AlgorithmsLibrary;
using SupportLib;
using System.Transactions;

namespace FrequencyAnalysisLib
{
    public class FreqAlg
    {
        private Map curr_map;

        private Rectangle cellRectangle;

        public FreqAlg(Map map)
        {
            curr_map = map;
            cellRectangle = GetRectangle(map);
        }
        public Map Curr_map 
        { 
            get 
            { 
                return curr_map;
            } 
        }

        public Rectangle CellRectangle
        {
            get 
            { 
                return cellRectangle;
            }
        }

        private Rectangle GetRectangle(Map m)
        {
            double xmin = m.Xmin; double xmax = m.Xmax;
            double ymin = m.Ymin; double ymax = m.Ymax;
            MPoint LowLeft = new MPoint(xmin, ymin);
            MPoint UpLeft = new MPoint(xmin, ymax);
            MPoint UpRight = new MPoint(xmax, ymax);
            MPoint LowRight = new MPoint(xmax,ymin);
            return new Rectangle(LowLeft,UpLeft, UpRight, LowRight);
        }
    }
}
using SupportLib;
using System.Transactions;

namespace FrequencyAnalysisLib
{
    public class FreqAlg
    {
        private Map curr_map;
        public FreqAlg(Map map)
        {
            curr_map = map;
        }
        public Map Curr_map 
        { 
            get 
            { 
                return curr_map;
            } 
        }
    }
}
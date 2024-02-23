using FrequencyAnalysisLib;
using SupportLib;
using System.Data;

namespace FrequencyAnalysisLibTestProject
{
    public class FreqAlgTest
    {
        [Fact]
        public void FreqAlgNotNullAfterCreating()
        {
            Map map = new Map();
            FreqAlg freqAlg = new FreqAlg(map);
            Assert.NotNull(freqAlg);
        }
        [Fact]
        public void FreqAlgContainsMapAfterCreating()
        {
            Map map = new Map();
            FreqAlg freqAlg = new FreqAlg(map);
            Assert.Equal(freqAlg.Curr_map, map);
        }

    }
}
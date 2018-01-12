using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatformCompatibility.Support
{
    public class ExecutationStateCriteria
    {
        public MyNode NodeCref { get; set; }
        public MyNode NodeCn { get; set; }


        /*CompatibilityCriteria*/
        public double CompatibilityCriteriaGUIScreenshotSimilarityPercentage { get; set; }
        public double CompatibilityCriteriaGUIScreenshotOCRSimilarityPercentage { get; set; }
        public double CompatibilityCriteriaTextSimilarityHybridPercentage { get; set; }
        public double CompatibilityCriteriaTextSimilarityTotalPercentage { get; set; }

        public double CompatibilityCriteriaRuntimeDifferencePercentage { get; set; }
        public bool CompatibilityCriteriaRuntimeFaster { get; set; }


        /*EquivalenceCriteria*/

        public bool EquivalenceCriteriaElementNegativelyPositioning { get; set; }
    }
}

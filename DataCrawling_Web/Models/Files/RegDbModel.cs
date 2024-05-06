using System.Collections.Generic;

namespace DataCrawling_Web.Models.Files
{
    public class RegDbModel
    {
        public int offerType { get; set; }
        public string hidStatType { get; set; }
        public string hidPlanType { get; set; }
        public string hidPeriodType { get; set; }
        public string lb_Url { get; set; }
        public string txtContent { get; set; }
        public string etcContent { get; set; }
        public List<string> FileList { get; set; }
        public List<int> FileIdx { get; set; }
    }
}
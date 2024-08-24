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

    public class OfferDBModel
    {
        public string IDX { get; set; }
        public string O_TYPE { get; set; }
        public string STAT_TYPE { get; set; }
        public string PLAN_TYPE { get; set; }
        public string PERIOD_TYPE { get; set; }
        public string U_URL { get; set; }
        public string CONTENT { get; set; }
        public string ETC { get; set; }
        public string DOC_TYPE { get; set; }
        public string Origin_FileName { get; set; }
        public int PROGRESS { get; set; }
    }

    public class OfferViewModel
    {
        public string IDX { get; set; }
        public string O_TYPE { get; set; }
        public string STAT_TYPE { get; set; }
        public string PLAN_TYPE { get; set; }
        public string PERIOD_TYPE { get; set; }
        public string U_URL { get; set; }
        public string CONTENT { get; set; }
        public string ETC { get; set; }
        public string PROGRESS_Stat { get; set; }
        public List<FileListModel> FileList { get; set; }
    }

    public class FileListModel
    {
        public string DOC_TYPE { get; set; }
        public string Origin_FileName { get; set; }
    }
}
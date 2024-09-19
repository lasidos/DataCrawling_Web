using DataCrawling_Web.BSL.Common;
using System;

namespace DataCrawling_Web.Models.Admin
{
    public class ContentInfoModel
    {
        public int OrderNo { get; set; }
        public string SECTOR { get; set; }
        public string CODE { get; set; }
        public int IDX { get; set; }
        public string PORT_IMG { get; set; }
        public string TITLE { get; set; }
        public string CONTENT_BODY { get; set; }
        public int VISIBLE { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public DateTime E_DATE { get; set; }
        public int READ_CNT { get; set; }

        public string Get_UserName
        {
            get
            {
                return Utility.Decrypt_AES(User_Name);
            }
        }
    }
}
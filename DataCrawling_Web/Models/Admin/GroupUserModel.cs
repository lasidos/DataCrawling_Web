using System;

namespace DataCrawling_Web.Models.Admin
{
    public class GroupInfoModel
    {
        public int GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public class GroupUserModel
    {
        public int IDX { get; set; }
        public int OrderNo { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime RegistDate { get; set; }
        public string LastLoginDateST { get; set; }
        public string RegistDateST { get; set; }
        public int GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
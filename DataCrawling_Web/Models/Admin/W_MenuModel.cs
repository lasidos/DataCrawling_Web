namespace DataCrawling_Web.Models.Admin
{
    public class W_MenuModel
    {
        public int Menu_Idx { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_URL { get; set; }
        public int Order_No { get; set; }
        public int Login_Stat { get; set; }
        public int Display_Stat { get; set; }
        public string M_Id { get; set; }
        public int? Parent_Id { get; set; }
        public int Menu_Type { get; set; }
        public int Menu_Level { get; set; }
    }
}
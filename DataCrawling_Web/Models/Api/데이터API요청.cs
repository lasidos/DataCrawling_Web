namespace DataCrawling_Web.Models.Api
{
    public class 데이터API요청: BaseModel
    {
        public int No { get; set; }
        public int 데이터API_No { get; set; }
        public string 신청자 { get; set; }

        public string 진행상태 { get; set; }

    }
}
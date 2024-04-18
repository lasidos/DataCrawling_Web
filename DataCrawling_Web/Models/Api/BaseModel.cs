using System;

namespace DataCrawling_Web.Models.Api
{
    public class BaseModel
    {
        public string 생성자 { get; set; }
        public string 수정자 { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime UptDate { get; set; }
    }
}
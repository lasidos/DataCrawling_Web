using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Models.Api
{

    //데이터 API
    public class DataApiModel : BaseModel
    {
        public int No { get; set; }
        // 공공 / 마이플랫폼
        [Required]
        public string Type { get; set; }
        // 이름
        [Required]
        public string Name { get; set; }
        // 설명
        public string Description { get; set; }
        //유료/무료 
        public Boolean IsFree { get; set; }
        public string 담당자 { get; set; }
        public string 프로토콜 { get; set; }
        public string HTTP메서드 { get; set; }

        public string 요청URL { get; set; }
        // json/xml/csv/text ...
        public DataFormat[] 제공타입 { get; set; }

        public UrlQueryParameter[] Query { get; set; }

    }


}
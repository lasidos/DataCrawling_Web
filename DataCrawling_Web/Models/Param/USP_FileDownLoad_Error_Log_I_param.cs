using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Models.Param
{
    public class USP_FileDownLoad_Error_Log_I_param
    {
        public string Referer { get; set; }
        public string MoveUrl { get; set; }
        public string UserIP { get; set; }
        public string ServerIP { get; set; }
        public string CONTENT { get; set; }
    }
}
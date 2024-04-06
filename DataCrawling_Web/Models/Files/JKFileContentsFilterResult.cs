using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Models.Files
{
    public class JKFileContentsFilterResult
    {
        public bool Result { get; set; }
        public int InvalidCount { get; set; }
        public string[] FileNames { get; set; }
        public string ErrorMessage { get; set; }
    }
}
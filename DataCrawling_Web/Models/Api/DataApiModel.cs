using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Models.Api
{// Data API Model
    public class DataApiModel : BaseModel
    {
        public int Id { get; set; }
        // Public / My Platform
        [Required]
        public string Type { get; set; }
        // Name
        [Required]
        public string Name { get; set; }
        // Description
        public string Description { get; set; }
        // Paid/Free 
        public bool IsFree { get; set; }
        public string Manager { get; set; }
        public string Protocol { get; set; }
        public string HttpMethod { get; set; }

        public string RequestUrl { get; set; }
        // json/xml/csv/text ...
        public DataFormat[] ProvidedTypes { get; set; }

        public UrlQueryParameter[] QueryParameters { get; set; }

    }



}
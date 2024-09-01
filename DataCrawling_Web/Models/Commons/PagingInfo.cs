using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Models.Commons
{
    public class PagingInfo
    {
        // 현재 페이지
        public int CurrentPage { get; set; }

        // 전체 아이템 수
        public int TotalItems { get; set; }

        // 페이지당 아이템 수
        public int ItemsPerPage { get; set; }

        // 전체 페이지 수
        public int TotalPages => ItemsPerPage == 0 ? 0 : (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
using System.Collections.Generic;

namespace DataCrawling_Web.Models
{
    public class ListViewModel
    {
        public string ID { get; set; }
        public int ItemIdx { get; set; }

        public bool ShowAll { get; set; } // 전체보기 텍스트 표시 여부

        public int Width { get; set; }

        public int Height { get; set; }
        public int SelectID { get; set; }

        public IEnumerable<ListViewItem> GroupInfo { get; set; }
    }

    public class ListViewItem
    {
        public int Idx { get; set; }
        public string Name { get; set; }
    }
}
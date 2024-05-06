using System.ComponentModel.DataAnnotations;

namespace DataCrawling_Web.Models.Api
{
    public class UrlQueryParameter
    {
        //검색어. UTF-8로 인코딩되어야 합니다.
        [Required]
        public string[] Query { get; set; }
        //한 번에 표시할 검색 결과 개수(기본값: 10, 최댓값: 100)
        public string Display { get; set; }
        //검색 시작 위치(기본값: 1, 최댓값: 1000)
        public int Start { get; set; }
        //검색 결과 정렬 방법
        //- sim: 정확도순으로 내림차순 정렬(기본값)
        //- date: 날짜순으로 내림차순 정렬
        public string Sort { get; set; }
    }

}
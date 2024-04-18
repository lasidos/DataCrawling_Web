namespace DataCrawling_Web.Models.Api
{
    public class ErrorInfo
    {
        public string ErrorCode { get; set; }
        public int HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Description { get; set; }
    }

}
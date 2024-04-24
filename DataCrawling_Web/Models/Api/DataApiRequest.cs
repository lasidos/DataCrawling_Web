namespace DataCrawling_Web.Models.Api
{
    // Data API Request
    public class DataApiRequest : BaseModel
    {
        public int Id { get; set; }
        public int DataApiId { get; set; }
        public string Applicant { get; set; }

        public string ProgressStatus { get; set; }

    }

}
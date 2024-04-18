namespace DataCrawling_Web.Models.Api
{
    public static class ErrorCodes
    {
        public static ErrorInfo SE01 => new ErrorInfo
        {
            ErrorCode = "SE01",
            HttpStatusCode = 400,
            ErrorMessage = "Incorrect query request",
            Description = "API 요청 URL의 프로토콜, 파라미터 등에 오류가 있는지 확인합니다."
        };

        public static ErrorInfo SE02 => new ErrorInfo
        {
            ErrorCode = "SE02",
            HttpStatusCode = 400,
            ErrorMessage = "Invalid display value",
            Description = "display 파라미터의 값이 허용 범위의 값(1~100)인지 확인합니다."
        };

        public static ErrorInfo SE03 => new ErrorInfo
        {
            ErrorCode = "SE03",
            HttpStatusCode = 400,
            ErrorMessage = "Invalid start value",
            Description = "start 파라미터의 값이 허용 범위의 값(1~1000)인지 확인합니다."
        };

        public static ErrorInfo SE04 => new ErrorInfo
        {
            ErrorCode = "SE04",
            HttpStatusCode = 400,
            ErrorMessage = "Invalid sort value",
            Description = "sort 파라미터의 값에 오타가 있는지 확인합니다."
        };

        public static ErrorInfo SE06 => new ErrorInfo
        {
            ErrorCode = "SE06",
            HttpStatusCode = 400,
            ErrorMessage = "Malformed encoding",
            Description = "검색어를 UTF-8로 인코딩합니다."
        };

        public static ErrorInfo SE05 => new ErrorInfo
        {
            ErrorCode = "SE05",
            HttpStatusCode = 404,
            ErrorMessage = "Invalid search api",
            Description = "API 요청 URL에 오타가 있는지 확인합니다."
        };

        public static ErrorInfo SE99 => new ErrorInfo
        {
            ErrorCode = "SE99",
            HttpStatusCode = 500,
            ErrorMessage = "System Error",
            Description = "서버 내부에 오류가 발생했습니다. '개발자 포럼'에 오류를 신고해 주십시오."
        };
    }

}
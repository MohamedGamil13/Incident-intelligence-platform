namespace Incident_intelligence_platform.DTOs
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public dynamic? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }


        public static ApiResponse SuccessResponse(dynamic data, string message = "Operation succeeded", int statusCode = 200)
        {
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = null
            };
        }


        public static ApiResponse FailureResponse(string message, IEnumerable<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                Data = default,
                Errors = errors ?? new List<string>()
            };
        }
    }
}



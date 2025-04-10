namespace TaskMasterAPI.Models.Responses
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponse(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }

    public class AuthResponse : ApiResponse
    {
        public string Token { get; set; }

        public AuthResponse(bool success, string message, string token, object data = null)
            : base(success, message, data)
        {
            Token = token;
        }
    }

}

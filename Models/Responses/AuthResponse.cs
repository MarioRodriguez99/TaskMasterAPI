using TaskMasterAPI.Models.Responses;

public class AuthResponse : ApiResponse
{
    public string Token { get; set; }

    // Constructor para respuestas exitosas
    public AuthResponse(bool success, string message, string token = null, object data = null)
        : base(success, message, data)
    {
        Token = token;
    }

    // Constructor para errores
    public AuthResponse(bool success, string message)
        : this(success, message, null, null) { }
}
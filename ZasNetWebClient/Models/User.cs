namespace ZasNetWebClient.Models;

public class LoginRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiredTime { get; set; }
    public string UserId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}

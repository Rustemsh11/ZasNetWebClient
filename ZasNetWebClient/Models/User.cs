namespace ZasNetWebClient.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public User? User { get; set; }
    public string Message { get; set; } = string.Empty;
}

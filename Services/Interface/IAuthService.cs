using TimDongDoi.API.DTOs.Auth;
using TimDongDoi.API.Models;


namespace TimDongDoi.API.Services.Interfaces
{
    public interface IAuthService
    {
        // Xử lý logic đăng ký người dùng mới
        Task<User> Register(RegisterRequest request);

        // Xử lý logic đăng nhập và trả về JWT Token
        Task<string> Login(LoginRequest request);

        // Logic nội bộ: Tạo token
        string CreateToken(User user);
    }
}
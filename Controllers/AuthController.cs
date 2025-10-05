using Microsoft.AspNetCore.Mvc;
using TimDongDoi.API.DTOs.Auth;
using TimDongDoi.API.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace TimDongDoi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng ký người dùng mới.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var user = await _authService.Register(request);
                return StatusCode(201, new { 
                    Message = "Đăng ký thành công", 
                    UserEmail = user.Email 
                });
            }
            catch (InvalidOperationException ex)
            {
                // Xử lý lỗi Email đã tồn tại
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                // Ghi log lỗi vào hệ thống
                return StatusCode(500, new { Message = "Đăng ký thất bại do lỗi server." });
            }
        }

        /// <summary>
        /// Đăng nhập và lấy JWT Token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                string token = await _authService.Login(request);
                return Ok(new { Token = token, Message = "Đăng nhập thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Lỗi mật khẩu không đúng
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception)
            {
                // Ghi log lỗi vào hệ thống
                return StatusCode(500, new { Message = "Đăng nhập thất bại do lỗi server." });
            }
        }
    }
}

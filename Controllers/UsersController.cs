using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimDongDoi.API.DTOs.User;
using TimDongDoi.API.Services.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net;

namespace TimDongDoi.API.Controllers
{
    [Authorize] // Bảo vệ tất cả các endpoint trong Controller này
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Lấy hồ sơ người dùng của chính mình.
        /// GET /api/Users/profile/me
        /// </summary>
        [HttpGet("profile/me")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            try
            {
                // Lấy User ID từ JWT Token đã được xác thực (qua UserService)
                int userId = _userService.GetUserIdFromClaims(); 
                
                var profile = await _userService.GetUserProfile(userId);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "JWT Token không hợp lệ hoặc thiếu thông tin người dùng." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết tại đây
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Lỗi server khi lấy hồ sơ.", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ người dùng của chính mình.
        /// PUT /api/Users/profile/me
        /// </summary>
        [HttpPut("profile/me")]
        public async Task<IActionResult> UpdateCurrentUserProfile(UserUpdateDto updateDto)
        {
            // Kiểm tra ModelState (xác thực DTO)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                int userId = _userService.GetUserIdFromClaims(); 
                
                var updatedProfile = await _userService.UpdateUserProfile(userId, updateDto);
                return Ok(new { Message = "Cập nhật hồ sơ thành công.", Profile = updatedProfile });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "JWT Token không hợp lệ hoặc thiếu thông tin người dùng." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Lỗi server khi cập nhật hồ sơ.", Detail = ex.Message });
            }
        }
    }
}
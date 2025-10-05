using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Data;
using TimDongDoi.API.DTOs.User;
using TimDongDoi.API.Models;
using TimDongDoi.API.Services.Interfaces;
using System.Security.Claims;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace TimDongDoi.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Hàm ánh xạ từ Model Entity sang DTO
        private UserProfileDto MapToUserProfileDto(User user)
        {
            if (user == null) 
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName ?? string.Empty,
                Role = user.Role ?? "User",
                Phone = user.Phone,
                Avatar = user.Avatar,
                AboutMe = user.AboutMe,
                Address = user.Address,
                Birthday = user.Birthday,
                Gender = user.Gender,
                JobTitle = user.JobTitle,
                SalaryExpectation = user.SalaryExpectation,
                CvFile = user.CvFile,
                CreatedAt = user.CreatedAt,
                
                // Map Skills
                Skills = user.UserSkills
                    .Select(us => new UserSkillDto
                    {
                        Id = us.Id,
                        SkillName = us.Skill?.Name ?? "N/A", 
                        Level = us.Level,
                        YearsExperience = us.YearsExperience
                    }).ToList(),
                
                // Map Experiences (Đã sửa lỗi chuyển đổi ngày tháng)
                Experiences = user.UserExperiences
                    .Select(ue => new UserExperienceDto
                    {
                        Id = ue.Id,
                        CompanyName = ue.CompanyName,
                        Position = ue.Position,
                        // Chuyển đổi an toàn từ DateOnly/DB Date sang DateTime?
                        StartDate = ue.StartDate.HasValue 
                            ? DateTime.SpecifyKind(ue.StartDate.Value.ToDateTime(new TimeOnly(0, 0)), DateTimeKind.Utc) 
                            : null,
                        EndDate = ue.EndDate.HasValue 
                            ? DateTime.SpecifyKind(ue.EndDate.Value.ToDateTime(new TimeOnly(0, 0)), DateTimeKind.Utc) 
                            : null, 
                        IsCurrent = ue.IsCurrent
                    }).ToList(),
                
                // Map Educations
                Educations = user.UserEducations
                    .Select(ue => new UserEducationDto
                    {
                        Id = ue.Id,
                        SchoolName = ue.SchoolName,
                        Major = ue.Major,
                        Degree = ue.Degree,
                        StartYear = ue.StartYear,
                        EndYear = ue.EndYear
                    }).ToList()
            };
        }

        // Lấy ID người dùng từ JWT Token
        public int GetUserIdFromClaims()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("JWT Token không hợp lệ hoặc thiếu User ID trong Claims.");
            }
            return userId;
        }

        // Lấy hồ sơ người dùng (Eager Loading)
        public async Task<UserProfileDto> GetUserProfile(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserSkills)
                    .ThenInclude(us => us.Skill) 
                .Include(u => u.UserExperiences)
                .Include(u => u.UserEducations)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng.");
            }

            return MapToUserProfileDto(user);
        }

        // Cập nhật hồ sơ
        public async Task<UserProfileDto> UpdateUserProfile(int userId, UserUpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng.");
            }

            // Ánh xạ DTO vào Model Entity
            user.FullName = updateDto.FullName;
            user.Phone = updateDto.Phone;
            user.AboutMe = updateDto.AboutMe;
            user.Address = updateDto.Address;
            user.JobTitle = updateDto.JobTitle;
            user.SalaryExpectation = updateDto.SalaryExpectation;
            user.Birthday = updateDto.Birthday; 
            user.Gender = updateDto.Gender;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetUserProfile(user.Id);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Data;
using TimDongDoi.API.DTOs.Application;
using TimDongDoi.API.Models;
using TimDongDoi.API.Services.Interfaces;

namespace TimDongDoi.API.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ApplicationService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ============================================
        // UC13: ỨNG VIÊN ỨNG TUYỂN
        // ============================================

        /// <summary>
        /// User apply job
        /// </summary>
        public async Task<ApplicationDto> ApplyJob(int userId, int jobId, ApplyJobRequest request, IFormFile? cvFile)
        {
            // Kiểm tra user tồn tại
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Role != "user")
            {
                throw new UnauthorizedAccessException("Chỉ tài khoản User mới có thể ứng tuyển");
            }

            // Kiểm tra job tồn tại và đang mở
            var job = await _context.Jobs
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng");
            }

            if (job.Status != "open")
            {
                throw new InvalidOperationException("Tin tuyển dụng này đã đóng hoặc chưa được công khai");
            }

            // Kiểm tra deadline
            if (job.Deadline.HasValue)
            {
                var deadline = job.Deadline.Value.ToDateTime(TimeOnly.MinValue);
                if (deadline < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Tin tuyển dụng đã hết hạn nộp hồ sơ");
                }
            }

            // Kiểm tra đã ứng tuyển chưa
            var existingApplication = await _context.Applications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.JobId == jobId);

            if (existingApplication != null)
            {
                throw new InvalidOperationException("Bạn đã ứng tuyển vị trí này rồi");
            }

            // Upload CV nếu có
            string? cvFileUrl = null;
            if (cvFile != null && cvFile.Length > 0)
            {
                cvFileUrl = await UploadCvFile(userId, cvFile);
            }
            else if (!string.IsNullOrEmpty(user.CvFile))
            {
                // Dùng CV có sẵn trong profile
                cvFileUrl = user.CvFile;
            }

            // Tạo application
            var application = new TimDongDoi.API.Models.Application
            {
                UserId = userId,
                JobId = jobId,
                CoverLetter = request.CoverLetter,
                CvFile = cvFileUrl,
                Status = "pending",
                AppliedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // Reload để lấy relationships
            return await GetApplicationById(userId, application.Id);
        }

        // ============================================
        // UC14: USER QUẢN LÝ ĐƠN ỨNG TUYỂN
        // ============================================

        /// <summary>
        /// User xem danh sách đơn ứng tuyển của mình
        /// </summary>
        public async Task<List<ApplicationDto>> GetMyApplications(int userId, string? status, int page = 1, int pageSize = 20)
        {
            var query = _context.Applications
                .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                .Where(a => a.UserId == userId);

            // Filter theo status
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var applications = await query
                .OrderByDescending(a => a.AppliedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return applications.Select(a => MapToDto(a, includeJobInfo: true)).ToList();
        }

        /// <summary>
        /// User xem chi tiết đơn ứng tuyển
        /// </summary>
        public async Task<ApplicationDto> GetApplicationById(int userId, int applicationId)
        {
            var application = await _context.Applications
                .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId);

            if (application == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn ứng tuyển");
            }

            return MapToDto(application, includeJobInfo: true);
        }

        /// <summary>
        /// User rút đơn ứng tuyển
        /// </summary>
        public async Task WithdrawApplication(int userId, int applicationId)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId);

            if (application == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn ứng tuyển");
            }

            // Chỉ rút được khi status = pending
            if (application.Status != "pending")
            {
                throw new InvalidOperationException("Chỉ có thể rút đơn khi đang ở trạng thái 'Chờ xử lý'");
            }

            _context.Applications.Remove(application);

            // Xóa CV file nếu có (optional)
            if (!string.IsNullOrEmpty(application.CvFile))
            {
                DeleteFile(application.CvFile);
            }

            await _context.SaveChangesAsync();
        }

        // ============================================
        // HELPER METHODS
        // ============================================

        /// <summary>
        /// Upload CV file
        /// </summary>
        private async Task<string> UploadCvFile(int userId, IFormFile file)
        {
            // Validate file
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Chỉ chấp nhận file PDF, DOC, DOCX");
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB
            {
                throw new InvalidOperationException("Kích thước file không được vượt quá 5MB");
            }

            // Tạo tên file unique
            var fileName = $"cv_{userId}_{Guid.NewGuid()}{extension}";
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "cvs");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/cvs/{fileName}";
        }

        /// <summary>
        /// Delete file from disk
        /// </summary>
        private void DeleteFile(string fileUrl)
        {
            try
            {
                var filePath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
                // Silent fail
            }
        }

        /// <summary>
        /// Map entity to DTO
        /// </summary>
        private ApplicationDto MapToDto(TimDongDoi.API.Models.Application application, bool includeJobInfo = false, bool includeUserInfo = false)
        {
            var dto = new ApplicationDto
            {
                Id = application.Id,
                UserId = application.UserId,
                JobId = application.JobId,
                CoverLetter = application.CoverLetter,
                CvFile = application.CvFile,
                Status = application.Status ?? "pending",
                RejectReason = application.RejectReason,
                AppliedAt = application.AppliedAt,
                UpdatedAt = application.UpdatedAt
            };

            // Include job info (cho User xem đơn của mình)
            if (includeJobInfo && application.Job != null)
            {
                dto.Job = new JobBasicDto
                {
                    Id = application.Job.Id,
                    Title = application.Job.Title,
                    Location = application.Job.Location,
                    Type = application.Job.Type,
                    Level = application.Job.Level,
                    SalaryMin = application.Job.SalaryMin,
                    SalaryMax = application.Job.SalaryMax,
                    Status = application.Job.Status ?? "open",
                    Company = application.Job.Company != null ? new CompanyBasicInfo
                    {
                        Id = application.Job.Company.Id,
                        Name = application.Job.Company.Name,
                        Logo = application.Job.Company.Logo
                    } : null
                };
            }

            // Include user info (cho Company xem ứng viên)
            if (includeUserInfo && application.User != null)
            {
                dto.User = new UserBasicDto
                {
                    Id = application.User.Id,
                    FullName = application.User.FullName,
                    Email = application.User.Email,
                    Phone = application.User.Phone,
                    Avatar = application.User.Avatar,
                    JobTitle = application.User.JobTitle,
                    SalaryExpectation = application.User.SalaryExpectation
                };
            }

            return dto;
        }

        // ============================================
// THÊM VÀO CUỐI CLASS ApplicationService
// SAU PHẦN MapToDto()
// ============================================

// ============================================
// UC29-33: COMPANY QUẢN LÝ ỨNG VIÊN
// ============================================

/// <summary>
/// Company xem danh sách ứng viên của một job
/// </summary>
public async Task<List<ApplicationDto>> GetJobApplications(int companyUserId, int jobId, string? status, int page = 1, int pageSize = 20)
{
    // Kiểm tra user có phải company không và job có thuộc company này không
    var job = await VerifyCompanyOwnsJob(companyUserId, jobId);

    var query = _context.Applications
        .Include(a => a.User)
        .Include(a => a.Job)
        .Where(a => a.JobId == jobId);

    // Filter theo status
    if (!string.IsNullOrWhiteSpace(status))
    {
        query = query.Where(a => a.Status == status);
    }

    var applications = await query
        .OrderByDescending(a => a.AppliedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return applications.Select(a => MapToDto(a, includeUserInfo: true)).ToList();
}

/// <summary>
/// Company xem chi tiết một ứng viên
/// </summary>
public async Task<ApplicationDto> GetApplicationForCompany(int companyUserId, int applicationId)
{
    var application = await _context.Applications
        .Include(a => a.User)
        .Include(a => a.Job)
            .ThenInclude(j => j.Company)
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == applicationId);

    if (application == null)
    {
        throw new KeyNotFoundException("Không tìm thấy đơn ứng tuyển");
    }

    // Kiểm tra job có thuộc company này không
    await VerifyCompanyOwnsJob(companyUserId, application.JobId);

    return MapToDto(application, includeUserInfo: true);
}

/// <summary>
/// Company update status application
/// </summary>
public async Task<ApplicationDto> UpdateApplicationStatus(int companyUserId, int applicationId, UpdateApplicationStatusRequest request)
{
    var application = await _context.Applications
        .Include(a => a.User)
        .Include(a => a.Job)
        .FirstOrDefaultAsync(a => a.Id == applicationId);

    if (application == null)
    {
        throw new KeyNotFoundException("Không tìm thấy đơn ứng tuyển");
    }

    // Kiểm tra quyền
    await VerifyCompanyOwnsJob(companyUserId, application.JobId);

    // Validate status
    var validStatuses = new[] { "pending", "reviewing", "interview", "accepted", "rejected" };
    if (!validStatuses.Contains(request.Status.ToLower()))
    {
        throw new InvalidOperationException("Status không hợp lệ");
    }

    // Nếu reject thì phải có lý do
    if (request.Status.ToLower() == "rejected" && string.IsNullOrWhiteSpace(request.RejectReason))
    {
        throw new InvalidOperationException("Vui lòng nhập lý do từ chối");
    }

    // Update
    application.Status = request.Status.ToLower();
    application.RejectReason = request.RejectReason;
    application.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return MapToDto(application, includeUserInfo: true);
}

/// <summary>
/// Company accept ứng viên (shortcut)
/// </summary>
public async Task<ApplicationDto> AcceptApplication(int companyUserId, int applicationId)
{
    var request = new UpdateApplicationStatusRequest
    {
        Status = "accepted",
        RejectReason = null
    };

    return await UpdateApplicationStatus(companyUserId, applicationId, request);
}

/// <summary>
/// Company reject ứng viên (shortcut)
/// </summary>
public async Task<ApplicationDto> RejectApplication(int companyUserId, int applicationId, string reason)
{
    var request = new UpdateApplicationStatusRequest
    {
        Status = "rejected",
        RejectReason = reason
    };

    return await UpdateApplicationStatus(companyUserId, applicationId, request);
}

// ============================================
// STATISTICS
// ============================================

/// <summary>
/// Company xem thống kê ứng viên của job
/// </summary>
public async Task<ApplicationStatsDto> GetJobApplicationStats(int companyUserId, int jobId)
{
    // Kiểm tra quyền
    await VerifyCompanyOwnsJob(companyUserId, jobId);

    var applications = await _context.Applications
        .Where(a => a.JobId == jobId)
        .AsNoTracking()
        .ToListAsync();

    return new ApplicationStatsDto
    {
        Total = applications.Count,
        Pending = applications.Count(a => a.Status == "pending"),
        Reviewing = applications.Count(a => a.Status == "reviewing"),
        Interview = applications.Count(a => a.Status == "interview"),
        Accepted = applications.Count(a => a.Status == "accepted"),
        Rejected = applications.Count(a => a.Status == "rejected")
    };
}

// ============================================
// HELPER: VERIFY COMPANY OWNS JOB
// ============================================

/// <summary>
/// Kiểm tra company có sở hữu job này không
/// </summary>
private async Task<Job> VerifyCompanyOwnsJob(int companyUserId, int jobId)
{
    var user = await _context.Users
        .Include(u => u.Company)
        .FirstOrDefaultAsync(u => u.Id == companyUserId);

    if (user == null || user.Role != "company" || user.Company == null)
    {
        throw new UnauthorizedAccessException("Chỉ tài khoản Company mới có thể thực hiện thao tác này");
    }

    var job = await _context.Jobs
        .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == user.Company.Id);

    if (job == null)
    {
        throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng hoặc bạn không có quyền truy cập");
    }

    return job;
}
    }
}
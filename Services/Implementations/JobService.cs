using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Data;
using TimDongDoi.API.DTOs.Job;
using TimDongDoi.API.Models;
using TimDongDoi.API.Services.Interfaces;

namespace TimDongDoi.API.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly AppDbContext _context;

        public JobService(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // UC23 & UC24: CRUD JOBS
        // ============================================

        /// <summary>
        /// Company tạo job mới
        /// </summary>
        public async Task<JobDetailDto> CreateJob(int userId, CreateJobRequest request)
        {
            // Kiểm tra user có phải company không
            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Role != "company" || user.Company == null)
            {
                throw new UnauthorizedAccessException("Chỉ tài khoản Company mới có thể tạo tin tuyển dụng");
            }

            // Tạo job mới
            var newJob = new Job
            {
                CompanyId = user.Company.Id,
                Title = request.Title,
                Description = request.Description,
                Requirements = request.Requirements,
                Benefits = request.Benefits,
                Type = request.Type,
                Level = request.Level,
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                SalaryCurrency = request.SalaryCurrency,
                Location = request.Location,
                LocationType = request.LocationType,
                Positions = request.Positions,
                Deadline = DateOnly.FromDateTime(request.Deadline),
                Status = request.Status,
                Views = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync();

            // Thêm skills nếu có
            if (request.Skills != null && request.Skills.Any())
            {
                foreach (var skillRequest in request.Skills)
                {
                    var jobSkill = new JobSkill
                    {
                        JobId = newJob.Id,
                        SkillId = skillRequest.SkillId,
                        IsRequired = skillRequest.IsRequired,
                        Level = skillRequest.Level
                    };
                    _context.JobSkills.Add(jobSkill);
                }
                await _context.SaveChangesAsync();
            }

            // Reload để lấy đầy đủ relationships
            return await GetJobById(newJob.Id, userId);
        }

        /// <summary>
        /// Company lấy danh sách jobs của mình
        /// </summary>
        public async Task<List<JobDto>> GetMyJobs(int userId, string? status, int page = 1, int pageSize = 20)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            var query = _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.JobSkills)
                    .ThenInclude(js => js.Skill)
                .Include(j => j.Applications)
                .Where(j => j.CompanyId == user.Company.Id);

            // Filter theo status nếu có
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(j => j.Status == status);
            }

            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return jobs.Select(j => MapToJobDto(j)).ToList();
        }

        /// <summary>
        /// Lấy chi tiết job theo ID (Public)
        /// </summary>
      /// <summary>
/// Lấy chi tiết job theo ID (Public)
/// </summary>
public async Task<JobDetailDto> GetJobById(int jobId, int? userId = null)
{
    var job = await _context.Jobs
        .Include(j => j.Company)
            .ThenInclude(c => c.User)
        .Include(j => j.JobSkills)
            .ThenInclude(js => js.Skill)
        // .Include(j => j.Applications) // 👈 Comment nếu chưa config Application
        .AsNoTracking()
        .FirstOrDefaultAsync(j => j.Id == jobId);

    if (job == null)
    {
        throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng");
    }

    // ✅ FIX: Tăng view count ĐỒNG BỘ (await)
    await IncrementViewCount(jobId);

    var dto = MapToJobDetailDto(job);

    // Nếu user đã login, check IsSaved và HasApplied
    if (userId.HasValue)
    {
        dto.IsSaved = await _context.SavedJobs
            .AnyAsync(sj => sj.UserId == userId.Value && sj.JobId == jobId);

        // ⚠️ Comment nếu chưa config Application
        // dto.HasApplied = await _context.Applications
        //     .AnyAsync(a => a.UserId == userId.Value && a.JobId == jobId);
        dto.HasApplied = false; // Tạm thời hard code
    }

    return dto;
}

        /// <summary>
        /// Company cập nhật job
        /// </summary>
        public async Task<JobDetailDto> UpdateJob(int userId, int jobId, UpdateJobRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == user.Company.Id);

            if (job == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng hoặc bạn không có quyền chỉnh sửa");
            }

            // Cập nhật thông tin
            job.Title = request.Title;
            job.Description = request.Description;
            job.Requirements = request.Requirements;
            job.Benefits = request.Benefits;
            job.Type = request.Type;
            job.Level = request.Level;
            job.SalaryMin = request.SalaryMin;
            job.SalaryMax = request.SalaryMax;
            job.SalaryCurrency = request.SalaryCurrency;
            job.Location = request.Location;
            job.LocationType = request.LocationType;
            job.Positions = request.Positions;
            job.Deadline = DateOnly.FromDateTime(request.Deadline);
            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetJobById(jobId, userId);
        }

        /// <summary>
        /// Company xóa job
        /// </summary>
        public async Task DeleteJob(int userId, int jobId)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            var job = await _context.Jobs
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == user.Company.Id);

            if (job == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng hoặc bạn không có quyền xóa");
            }

            // Kiểm tra có applications nào đang pending không
            var hasPendingApplications = job.Applications.Any(a => a.Status == "pending" || a.Status == "reviewing");

            if (hasPendingApplications)
            {
                throw new InvalidOperationException("Không thể xóa tin tuyển dụng khi còn hồ sơ ứng tuyển đang chờ xử lý. Vui lòng đóng tin tuyển dụng thay vì xóa.");
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }

        // ============================================
        // HELPER METHODS: MAP ENTITY TO DTO
        // ============================================

        private JobDto MapToJobDto(Job job)
        {
            return new JobDto
            {
                Id = job.Id,
                CompanyId = job.CompanyId,
                Title = job.Title,
                Type = job.Type,
                Level = job.Level,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                SalaryCurrency = job.SalaryCurrency,
                Location = job.Location,
                LocationType = job.LocationType,
                Deadline = job.Deadline?.ToDateTime(TimeOnly.MinValue),
                Status = job.Status ?? "open",
                Views = job.Views ?? 0,
                CreatedAt = job.CreatedAt,
                Company = job.Company != null ? new CompanyBasicDto
                {
                    Id = job.Company.Id,
                    Name = job.Company.Name,
                    Logo = job.Company.Logo,
                    Industry = job.Company.Industry,
                    VerificationStatus = job.Company.VerificationStatus
                } : null,
                Skills = job.JobSkills?.Select(js => new JobSkillDto
                {
                    Id = js.Id,
                    JobId = js.JobId,
                    SkillId = js.SkillId,
                    SkillName = js.Skill?.Name ?? "",
                    SkillIcon = js.Skill?.Icon,
                    IsRequired = js.IsRequired ?? true,
                    Level = js.Level
                }).ToList() ?? new List<JobSkillDto>(),
                TotalApplications = job.Applications?.Count ?? 0
            };
        }

        private JobDetailDto MapToJobDetailDto(Job job)
        {
            var deadline = job.Deadline?.ToDateTime(TimeOnly.MinValue);
            var daysRemaining = deadline.HasValue ? (deadline.Value - DateTime.UtcNow).Days : 0;

            return new JobDetailDto
            {
                Id = job.Id,
                CompanyId = job.CompanyId,
                Title = job.Title,
                Description = job.Description,
                Requirements = job.Requirements,
                Benefits = job.Benefits,
                Type = job.Type,
                Level = job.Level,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                SalaryCurrency = job.SalaryCurrency,
                Location = job.Location,
                LocationType = job.LocationType,
                Positions = job.Positions ?? 1,
                Deadline = deadline,
                Status = job.Status ?? "open",
                Views = job.Views ?? 0,
                CreatedAt = job.CreatedAt,
                UpdatedAt = job.UpdatedAt,
                Company = job.Company != null ? new CompanyBasicDto
                {
                    Id = job.Company.Id,
                    Name = job.Company.Name,
                    Logo = job.Company.Logo,
                    Industry = job.Company.Industry,
                    VerificationStatus = job.Company.VerificationStatus
                } : null,
                Skills = job.JobSkills?.Select(js => new JobSkillDto
                {
                    Id = js.Id,
                    JobId = js.JobId,
                    SkillId = js.SkillId,
                    SkillName = js.Skill?.Name ?? "",
                    SkillIcon = js.Skill?.Icon,
                    IsRequired = js.IsRequired ?? true,
                    Level = js.Level
                }).ToList() ?? new List<JobSkillDto>(),
                Stats = new JobStatsDto
                {
                    TotalApplications = job.Applications?.Count ?? 0,
                    PendingApplications = job.Applications?.Count(a => a.Status == "pending") ?? 0,
                    AcceptedApplications = job.Applications?.Count(a => a.Status == "accepted") ?? 0,
                    Views = job.Views ?? 0,
                    DaysRemaining = daysRemaining > 0 ? daysRemaining : 0
                }
            };
        }

       // ============================================
// THÊM VÀO CUỐI CLASS JobService (SAU MapToJobDetailDto)
// ============================================

// ============================================
// UC25: SAO CHÉP TIN TUYỂN DỤNG
// ============================================

/// <summary>
/// Sao chép job để tạo job mới tương tự
/// </summary>
public async Task<JobDetailDto> CloneJob(int userId, int jobId)
{
    var user = await _context.Users
        .Include(u => u.Company)
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null || user.Company == null)
    {
        throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
    }

    var originalJob = await _context.Jobs
        .Include(j => j.JobSkills)
        .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == user.Company.Id);

    if (originalJob == null)
    {
        throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng hoặc bạn không có quyền sao chép");
    }

    // Clone job
    var clonedJob = new Job
    {
        CompanyId = originalJob.CompanyId,
        Title = originalJob.Title + " (Copy)",
        Description = originalJob.Description,
        Requirements = originalJob.Requirements,
        Benefits = originalJob.Benefits,
        Type = originalJob.Type,
        Level = originalJob.Level,
        SalaryMin = originalJob.SalaryMin,
        SalaryMax = originalJob.SalaryMax,
        SalaryCurrency = originalJob.SalaryCurrency,
        Location = originalJob.Location,
        LocationType = originalJob.LocationType,
        Positions = originalJob.Positions,
        Deadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)), // Deadline mới: 30 ngày sau
        Status = "draft", // Mặc định là draft
        Views = 0,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    _context.Jobs.Add(clonedJob);
    await _context.SaveChangesAsync();

    // Clone skills
    foreach (var originalSkill in originalJob.JobSkills)
    {
        var clonedSkill = new JobSkill
        {
            JobId = clonedJob.Id,
            SkillId = originalSkill.SkillId,
            IsRequired = originalSkill.IsRequired,
            Level = originalSkill.Level
        };
        _context.JobSkills.Add(clonedSkill);
    }
    await _context.SaveChangesAsync();

    return await GetJobById(clonedJob.Id, userId);
}

// ============================================
// UC26: ĐÓNG/MỞ TIN TUYỂN DỤNG
// ============================================

/// <summary>
/// Đóng tin tuyển dụng
/// </summary>
public async Task CloseJob(int userId, int jobId)
{
    var job = await GetJobByUserAndId(userId, jobId);
    job.Status = "closed";
    job.UpdatedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync();
}

/// <summary>
/// Mở lại tin tuyển dụng
/// </summary>
public async Task ReopenJob(int userId, int jobId)
{
    var job = await GetJobByUserAndId(userId, jobId);
    
    // Kiểm tra deadline còn hợp lệ không
    if (job.Deadline.HasValue && job.Deadline.Value.ToDateTime(TimeOnly.MinValue) < DateTime.UtcNow)
    {
        throw new InvalidOperationException("Không thể mở lại tin tuyển dụng đã hết hạn. Vui lòng cập nhật deadline trước.");
    }

    job.Status = "open";
    job.UpdatedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync();
}

/// <summary>
/// Publish job từ draft sang open
/// </summary>
public async Task PublishJob(int userId, int jobId)
{
    var job = await GetJobByUserAndId(userId, jobId);

    if (job.Status != "draft")
    {
        throw new InvalidOperationException("Chỉ có thể publish tin tuyển dụng ở trạng thái draft");
    }

    // Kiểm tra deadline còn hợp lệ không
    if (job.Deadline.HasValue && job.Deadline.Value.ToDateTime(TimeOnly.MinValue) < DateTime.UtcNow)
    {
        throw new InvalidOperationException("Deadline đã hết hạn. Vui lòng cập nhật deadline trước khi publish.");
    }

    job.Status = "open";
    job.UpdatedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync();
}

// ============================================
// UC27: THỐNG KÊ TIN TUYỂN DỤNG
// ============================================

/// <summary>
/// Lấy thống kê chi tiết của job
/// </summary>
public async Task<JobStatsDto> GetJobStats(int userId, int jobId)
{
    var user = await _context.Users
        .Include(u => u.Company)
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null || user.Company == null)
    {
        throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
    }

    var job = await _context.Jobs
        .Include(j => j.Applications)
        .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == user.Company.Id);

    if (job == null)
    {
        throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng");
    }

    var deadline = job.Deadline?.ToDateTime(TimeOnly.MinValue);
    var daysRemaining = deadline.HasValue ? (deadline.Value - DateTime.UtcNow).Days : 0;

    return new JobStatsDto
    {
        TotalApplications = job.Applications.Count,
        PendingApplications = job.Applications.Count(a => a.Status == "pending"),
        AcceptedApplications = job.Applications.Count(a => a.Status == "accepted"),
        Views = job.Views ?? 0,
        DaysRemaining = daysRemaining > 0 ? daysRemaining : 0
    };
}

// ============================================
// QUẢN LÝ SKILLS CỦA JOB
// ============================================

/// <summary>
/// Lấy danh sách skills của job
/// </summary>
public async Task<List<JobSkillDto>> GetJobSkills(int jobId)
{
    var jobSkills = await _context.JobSkills
        .Include(js => js.Skill)
        .Where(js => js.JobId == jobId)
        .AsNoTracking()
        .ToListAsync();

    return jobSkills.Select(js => new JobSkillDto
    {
        Id = js.Id,
        JobId = js.JobId,
        SkillId = js.SkillId,
        SkillName = js.Skill?.Name ?? "",
        SkillIcon = js.Skill?.Icon,
        IsRequired = js.IsRequired ?? true,
        Level = js.Level
    }).ToList();
}

/// <summary>
/// Thêm skill vào job
/// </summary>
public async Task<JobSkillDto> AddJobSkill(int userId, int jobId, AddJobSkillRequest request)
{
    var job = await GetJobByUserAndId(userId, jobId);

    // Kiểm tra skill đã tồn tại trong job chưa
    var existingSkill = await _context.JobSkills
        .FirstOrDefaultAsync(js => js.JobId == jobId && js.SkillId == request.SkillId);

    if (existingSkill != null)
    {
        throw new InvalidOperationException("Skill này đã được thêm vào job");
    }

    // Kiểm tra skill có tồn tại không
    var skill = await _context.Skills.FindAsync(request.SkillId);
    if (skill == null)
    {
        throw new KeyNotFoundException("Không tìm thấy skill");
    }

    var jobSkill = new JobSkill
    {
        JobId = jobId,
        SkillId = request.SkillId,
        IsRequired = request.IsRequired,
        Level = request.Level
    };

    _context.JobSkills.Add(jobSkill);
    await _context.SaveChangesAsync();

    return new JobSkillDto
    {
        Id = jobSkill.Id,
        JobId = jobSkill.JobId,
        SkillId = jobSkill.SkillId,
        SkillName = skill.Name,
        SkillIcon = skill.Icon,
        IsRequired = jobSkill.IsRequired ?? true,
        Level = jobSkill.Level
    };
}

/// <summary>
/// Xóa skill khỏi job
/// </summary>
public async Task RemoveJobSkill(int userId, int jobId, int skillId)
{
    var job = await GetJobByUserAndId(userId, jobId);

    var jobSkill = await _context.JobSkills
        .FirstOrDefaultAsync(js => js.JobId == jobId && js.SkillId == skillId);

    if (jobSkill == null)
    {
        throw new KeyNotFoundException("Không tìm thấy skill trong job");
    }

    _context.JobSkills.Remove(jobSkill);
    await _context.SaveChangesAsync();
}

// ============================================
// HELPER: GET JOB BY USER AND ID
// ============================================

private async Task<Job> GetJobByUserAndId(int userId, int jobId)
{
    var user = await _context.Users
        .Include(u => u.Company)
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null || user.Company == null)
    {
        throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
    }

    var job = await _context.Jobs
        .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == user.Company.Id);

    if (job == null)
    {
        throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng hoặc bạn không có quyền truy cập");
    }

    return job;
}

// ============================================
// THÊM VÀO CUỐI CLASS JobService (SAU GetJobByUserAndId)
// ============================================

// ============================================
// SEARCH & FILTER JOBS (PUBLIC)
// ============================================

/// <summary>
/// Tìm kiếm jobs
/// </summary>
public async Task<List<JobDto>> SearchJobs(
    string? keyword,
    string? location,
    string? type,
    string? level,
    int? salaryMin,
    int? salaryMax,
    string? skills,
    int page = 1,
    int pageSize = 20)
{
    var query = _context.Jobs
        .Include(j => j.Company)
        .Include(j => j.JobSkills)
            .ThenInclude(js => js.Skill)
        .Include(j => j.Applications)
        .Where(j => j.Status == "open"); // Chỉ lấy jobs đang mở

    // Filter theo keyword (title, description)
    if (!string.IsNullOrWhiteSpace(keyword))
    {
        keyword = keyword.ToLower();
        query = query.Where(j =>
            j.Title.ToLower().Contains(keyword) ||
            (j.Description != null && j.Description.ToLower().Contains(keyword))
        );
    }

    // Filter theo location
    if (!string.IsNullOrWhiteSpace(location))
    {
        query = query.Where(j => j.Location != null && j.Location.ToLower().Contains(location.ToLower()));
    }

    // Filter theo type
    if (!string.IsNullOrWhiteSpace(type))
    {
        query = query.Where(j => j.Type == type);
    }

    // Filter theo level
    if (!string.IsNullOrWhiteSpace(level))
    {
        query = query.Where(j => j.Level == level);
    }

    // Filter theo salary range
    if (salaryMin.HasValue)
    {
        query = query.Where(j => j.SalaryMax == null || j.SalaryMax >= salaryMin.Value);
    }

    if (salaryMax.HasValue)
    {
        query = query.Where(j => j.SalaryMin == null || j.SalaryMin <= salaryMax.Value);
    }

    // Filter theo skills (comma-separated skill IDs)
    if (!string.IsNullOrWhiteSpace(skills))
    {
        var skillIds = skills.Split(',').Select(int.Parse).ToList();
        query = query.Where(j => j.JobSkills.Any(js => skillIds.Contains(js.SkillId)));
    }

    // Phân trang
    var jobs = await query
        .OrderByDescending(j => j.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return jobs.Select(j => MapToJobDto(j)).ToList();
}

/// <summary>
/// Lấy danh sách jobs nổi bật
/// </summary>
public async Task<List<JobDto>> GetFeaturedJobs(int limit = 10)
{
    var jobs = await _context.Jobs
        .Include(j => j.Company)
        .Include(j => j.JobSkills)
            .ThenInclude(js => js.Skill)
        .Include(j => j.Applications)
        .Where(j => j.Status == "open")
        .OrderByDescending(j => j.Views) // Sắp xếp theo views
        .ThenByDescending(j => j.CreatedAt) // Sau đó theo thời gian tạo
        .Take(limit)
        .AsNoTracking()
        .ToListAsync();

    return jobs.Select(j => MapToJobDto(j)).ToList();
}

/// <summary>
/// Lấy danh sách jobs của một company
/// </summary>
public async Task<List<JobDto>> GetJobsByCompany(int companyId, int page = 1, int pageSize = 20)
{
    var jobs = await _context.Jobs
        .Include(j => j.Company)
        .Include(j => j.JobSkills)
            .ThenInclude(js => js.Skill)
        .Include(j => j.Applications)
        .Where(j => j.CompanyId == companyId && j.Status == "open") // Chỉ lấy jobs đang mở
        .OrderByDescending(j => j.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return jobs.Select(j => MapToJobDto(j)).ToList();
}

// ============================================
// SAVED JOBS (USER LƯU TIN)
// ============================================

/// <summary>
/// User lưu tin tuyển dụng
/// </summary>
public async Task SaveJob(int userId, int jobId)
{
    // Kiểm tra job có tồn tại không
    var job = await _context.Jobs.FindAsync(jobId);
    if (job == null)
    {
        throw new KeyNotFoundException("Không tìm thấy tin tuyển dụng");
    }

    // Kiểm tra đã lưu chưa
    var existingSave = await _context.SavedJobs
        .FirstOrDefaultAsync(sj => sj.UserId == userId && sj.JobId == jobId);

    if (existingSave != null)
    {
        throw new InvalidOperationException("Bạn đã lưu tin tuyển dụng này rồi");
    }

    var savedJob = new SavedJob
    {
        UserId = userId,
        JobId = jobId,
        CreatedAt = DateTime.UtcNow
    };

    _context.SavedJobs.Add(savedJob);
    await _context.SaveChangesAsync();
}

/// <summary>
/// User bỏ lưu tin
/// </summary>
public async Task UnsaveJob(int userId, int jobId)
{
    var savedJob = await _context.SavedJobs
        .FirstOrDefaultAsync(sj => sj.UserId == userId && sj.JobId == jobId);

    if (savedJob == null)
    {
        throw new KeyNotFoundException("Bạn chưa lưu tin tuyển dụng này");
    }

    _context.SavedJobs.Remove(savedJob);
    await _context.SaveChangesAsync();
}

/// <summary>
/// Lấy danh sách jobs đã lưu của user
/// </summary>
public async Task<List<JobDto>> GetSavedJobs(int userId, int page = 1, int pageSize = 20)
{
    var savedJobs = await _context.SavedJobs
        .Include(sj => sj.Job)
            .ThenInclude(j => j.Company)
        .Include(sj => sj.Job)
            .ThenInclude(j => j.JobSkills)
                .ThenInclude(js => js.Skill)
        .Include(sj => sj.Job)
            .ThenInclude(j => j.Applications)
        .Where(sj => sj.UserId == userId)
        .OrderByDescending(sj => sj.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return savedJobs.Select(sj => MapToJobDto(sj.Job)).ToList();
}

// ============================================
// HELPER: TĂNG VIEW COUNT
// ============================================

/// <summary>
/// Tăng view count khi user xem job
/// </summary>
public async Task IncrementViewCount(int jobId)
{
    try
    {
        var job = await _context.Jobs.FindAsync(jobId);
        if (job != null)
        {
            job.Views = (job.Views ?? 0) + 1;
            await _context.SaveChangesAsync();
        }
    }
    catch
    {
        // Silent fail - không throw exception để không ảnh hưởng đến request chính
    }
}
    }
}
using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Data;
using TimDongDoi.API.DTOs.Company;
using TimDongDoi.API.Models;
using TimDongDoi.API.Services.Interfaces;

namespace TimDongDoi.API.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CompanyService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ============================================
        // UC20: QUẢN LÝ THÔNG TIN CÔNG TY (CRUD)
        // ============================================

        /// <summary>
        /// Lấy thông tin công ty của user hiện tại
        /// </summary>
        public async Task<CompanyDetailDto> GetMyCompany(int userId)
        {
            var company = await _context.Companies
                .Include(c => c.User)
                .Include(c => c.CompanyLocations)
                .Include(c => c.Jobs)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty. Vui lòng tạo thông tin công ty trước.");
            }

            return MapToDetailDto(company);
        }

        /// <summary>
        /// Lấy thông tin công ty theo ID (Public)
        /// </summary>
        public async Task<CompanyDetailDto> GetCompanyById(int companyId)
        {
            var company = await _context.Companies
                .Include(c => c.User)
                .Include(c => c.CompanyLocations)
                .Include(c => c.Jobs)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy công ty");
            }

            return MapToDetailDto(company);
        }

        /// <summary>
        /// Tạo thông tin công ty mới
        /// </summary>
        public async Task<CompanyDetailDto> CreateCompany(int userId, CreateCompanyRequest request)
        {
            // Kiểm tra user có role company không
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Role != "company")
            {
                throw new UnauthorizedAccessException("Chỉ tài khoản Company mới có thể tạo thông tin công ty");
            }

            // Kiểm tra đã có company chưa
            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingCompany != null)
            {
                throw new InvalidOperationException("Bạn đã có thông tin công ty. Vui lòng cập nhật thay vì tạo mới.");
            }

            var newCompany = new Company
            {
                UserId = userId,
                Name = request.Name,
                Description = request.Description,
                Website = request.Website,
                Industry = request.Industry,
                Size = request.Size,
                FoundedYear = request.FoundedYear,
                VerificationStatus = "pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();

            // Reload để lấy đầy đủ relationships
            return await GetMyCompany(userId);
        }

        /// <summary>
        /// Cập nhật thông tin công ty
        /// </summary>
        public async Task<CompanyDetailDto> UpdateCompany(int userId, UpdateCompanyRequest request)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            // Cập nhật thông tin
            company.Name = request.Name;
            company.Description = request.Description;
            company.Website = request.Website;
            company.Industry = request.Industry;
            company.Size = request.Size;
            company.FoundedYear = request.FoundedYear;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload với đầy đủ relationships
            return await GetMyCompany(userId);
        }

        /// <summary>
        /// Xóa thông tin công ty
        /// </summary>
        public async Task DeleteCompany(int userId)
        {
            var company = await _context.Companies
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            // Kiểm tra có job đang active không
            var hasActiveJobs = company.Jobs.Any(j => j.Status == "active");

            if (hasActiveJobs)
            {
                throw new InvalidOperationException("Không thể xóa công ty khi còn tin tuyển dụng đang hoạt động. Vui lòng đóng tất cả tin tuyển dụng trước.");
            }

            // Xóa logo nếu có
            if (!string.IsNullOrEmpty(company.Logo))
            {
                DeleteFile(company.Logo);
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        // ============================================
        // HELPER METHOD: MAP ENTITY TO DTO
        // ============================================
        private CompanyDetailDto MapToDetailDto(Company company)
        {
            return new CompanyDetailDto
            {
                Id = company.Id,
                UserId = company.UserId,
                Name = company.Name,
                Logo = company.Logo,
                Description = company.Description,
                Website = company.Website,
                Industry = company.Industry,
                Size = company.Size,
                FoundedYear = company.FoundedYear,
                VerificationStatus = company.VerificationStatus ?? "pending",
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt,
                Representative = company.User != null ? new UserBasicDto
                {
                    Id = company.User.Id,
                    FullName = company.User.FullName,
                    Email = company.User.Email,
                    Phone = company.User.Phone
                } : null,
                Locations = company.CompanyLocations?.Select(l => new CompanyLocationDto
                {
                    Id = l.Id,
                    CompanyId = l.CompanyId,
                    Address = l.Address ?? "",
                    City = l.City,
                    Country = l.Country,
                    IsHeadquarter = l.IsHeadquarter ?? false,
                    CreatedAt = l.CreatedAt
                }).ToList() ?? new List<CompanyLocationDto>(),
                Stats = new CompanyStatsDto
                {
                    TotalJobs = company.Jobs?.Count ?? 0,
                    ActiveJobs = company.Jobs?.Count(j => j.Status == "active") ?? 0,
                    TotalApplications = 0, // TODO: Tính sau khi có Applications
                    TotalLocations = company.CompanyLocations?.Count ?? 0
                }
            };
        }

        // TIẾP TỤC Ở PART 2...

        // ============================================
        // THÊM VÀO CUỐI CLASS CompanyService
        // SAU PHẦN MapToDetailDto()
        // ============================================

        // ============================================
        // UC21: UPLOAD LOGO/ẢNH CÔNG TY
        // ============================================

        /// <summary>
        /// Upload logo công ty
        /// </summary>
        public async Task<UploadFileResponse> UploadLogo(int userId, IFormFile file)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            // Validate file
            ValidateImageFile(file, 5); // Max 5MB

            // Upload file
            var fileName = $"logo_{company.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fileUrl = await SaveFile(file, "logos", fileName);

            // Xóa logo cũ nếu có
            if (!string.IsNullOrEmpty(company.Logo))
            {
                DeleteFile(company.Logo);
            }

            // Cập nhật database
            company.Logo = fileUrl;
            company.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new UploadFileResponse
            {
                Success = true,
                Url = fileUrl,
                FileName = fileName,
                FileSize = file.Length,
                Message = "Upload logo thành công"
            };
        }

        /// <summary>
        /// Upload ảnh công ty (banner, văn phòng...)
        /// </summary>
        public async Task<UploadFileResponse> UploadImage(int userId, IFormFile file)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            // Validate file
            ValidateImageFile(file, 10); // Max 10MB cho ảnh banner

            // Upload file
            var fileName = $"company_{company.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fileUrl = await SaveFile(file, "company-images", fileName);

            return new UploadFileResponse
            {
                Success = true,
                Url = fileUrl,
                FileName = fileName,
                FileSize = file.Length,
                Message = "Upload ảnh thành công"
            };
        }

        // ============================================
        // UC22: XÁC MINH DOANH NGHIỆP
        // ============================================

        /// <summary>
        /// Gửi yêu cầu xác minh công ty
        /// </summary>
        public async Task<CompanyVerificationDto> SubmitVerification(int userId, SubmitVerificationRequest request, IFormFile document)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            // Validate file
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(document.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Chỉ chấp nhận file PDF hoặc ảnh (.pdf, .jpg, .jpeg, .png)");
            }

            if (document.Length > 10 * 1024 * 1024) // 10MB
            {
                throw new InvalidOperationException("Kích thước file không được vượt quá 10MB");
            }

            // Upload document
            var fileName = $"verification_{company.Id}_{Guid.NewGuid()}{extension}";
            var documentUrl = await SaveFile(document, "verifications", fileName);

            // Tạo verification request
            var verification = new CompanyVerification
            {
                CompanyId = company.Id,
                DocumentType = request.DocumentType,
                DocumentUrl = documentUrl,
                Status = "pending",
                Notes = request.Notes,
                SubmittedAt = DateTime.UtcNow
            };

            _context.CompanyVerifications.Add(verification);
            await _context.SaveChangesAsync();

            return new CompanyVerificationDto
            {
                Id = verification.Id,
                CompanyId = verification.CompanyId,
                CompanyName = company.Name,
                DocumentType = verification.DocumentType ?? "",
                DocumentUrl = verification.DocumentUrl,
                Status = verification.Status ?? "pending",
                Notes = verification.Notes,
                SubmittedAt = verification.SubmittedAt
            };
        }

        /// <summary>
        /// Lấy trạng thái xác minh của công ty
        /// </summary>
        public async Task<List<CompanyVerificationDto>> GetMyVerifications(int userId)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
            }

            var verifications = await _context.CompanyVerifications
                .Include(v => v.VerifiedByNavigation)
                .Where(v => v.CompanyId == company.Id)
                .OrderByDescending(v => v.SubmittedAt)
                .AsNoTracking()
                .ToListAsync();

            return verifications.Select(v => new CompanyVerificationDto
            {
                Id = v.Id,
                CompanyId = v.CompanyId,
                CompanyName = company.Name,
                VerifiedBy = v.VerifiedBy,
                VerifiedByName = v.VerifiedByNavigation?.FullName,
                DocumentType = v.DocumentType ?? "",
                DocumentUrl = v.DocumentUrl,
                Status = v.Status ?? "pending",
                Notes = v.Notes,
                SubmittedAt = v.SubmittedAt,
                VerifiedAt = v.VerifiedAt
            }).ToList();
        }

        /// <summary>
        /// [ADMIN] Lấy danh sách yêu cầu xác minh chờ duyệt
        /// </summary>
        public async Task<List<CompanyVerificationDto>> GetPendingVerifications(int page = 1, int pageSize = 20)
        {
            var verifications = await _context.CompanyVerifications
                .Include(v => v.Company)
                .Include(v => v.VerifiedByNavigation)
                .Where(v => v.Status == "pending")
                .OrderBy(v => v.SubmittedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return verifications.Select(v => new CompanyVerificationDto
            {
                Id = v.Id,
                CompanyId = v.CompanyId,
                CompanyName = v.Company.Name,
                VerifiedBy = v.VerifiedBy,
                VerifiedByName = v.VerifiedByNavigation?.FullName,
                DocumentType = v.DocumentType ?? "",
                DocumentUrl = v.DocumentUrl,
                Status = v.Status ?? "pending",
                Notes = v.Notes,
                SubmittedAt = v.SubmittedAt,
                VerifiedAt = v.VerifiedAt
            }).ToList();
        }

        /// <summary>
        /// [ADMIN] Duyệt hoặc từ chối xác minh
        /// </summary>
       // File: Services/Implementations/CompanyService.cs

// ... (các hàm khác) ...

/// <summary>
/// UC22: Admin duyệt hoặc từ chối yêu cầu xác minh công ty
/// </summary>
public async Task<CompanyVerificationDto> ProcessVerification(int adminUserId, int verificationId, ProcessVerificationRequest request)
{
    var verification = await _context.CompanyVerifications
        .Include(v => v.Company)
        .FirstOrDefaultAsync(v => v.Id == verificationId);

    if (verification == null)
    {
        throw new KeyNotFoundException("Không tìm thấy yêu cầu xác minh");
    }

    if (verification.Status != "pending")
    {
        throw new InvalidOperationException("Yêu cầu này đã được xử lý");
    }

    // Cập nhật trạng thái
    var action = request.Action.ToLower();

    // FIX LOGIC 1: Thống nhất chỉ dùng 'approve' và 'reject'
    if (action != "approve" && action != "reject") 
    {
        throw new InvalidOperationException("Action phải là 'approve' hoặc 'reject'.");
    }

    // FIX LOGIC 2 (SQL CHECK CONSTRAINT):
    // Gán trạng thái cho Verification record (Bảng company_verifications)
    // CSDL chấp nhận 'approved' hoặc 'rejected', nên ta phải gán giá trị có 'd'
    verification.Status = action == "approve" ? "approved" : "rejected"; 
    
    verification.VerifiedBy = adminUserId;
    verification.VerifiedAt = DateTime.UtcNow;
    verification.Notes = request.Notes;

    // Cập nhật trạng thái của Công ty (Bảng companies)
    if (verification.Status == "approved")
    {
        // Gán 'verified' cho Hồ sơ Công ty (VerificationStatus)
        verification.Company.VerificationStatus = "verified"; 
    } 
    else if (verification.Status == "rejected") 
    {
        // Gán 'rejected' cho Hồ sơ Công ty
        verification.Company.VerificationStatus = "rejected";
    }

    await _context.SaveChangesAsync();

    var admin = await _context.Users.FindAsync(adminUserId);

    return new CompanyVerificationDto
    {
        Id = verification.Id,
        CompanyId = verification.CompanyId,
        CompanyName = verification.Company.Name,
        VerifiedBy = verification.VerifiedBy,
        VerifiedByName = admin?.FullName,
        DocumentType = verification.DocumentType ?? "",
        DocumentUrl = verification.DocumentUrl,
        Status = verification.Status ?? "", // Trả về 'approved' hoặc 'rejected'
        Notes = verification.Notes,
        SubmittedAt = verification.SubmittedAt,
        VerifiedAt = verification.VerifiedAt
    };
}

// ... (các hàm khác) ...

        // TIẾP TỤC Ở PART 3...
        // ============================================
// THÊM VÀO CUỐI CLASS CompanyService
// SAU PHẦN ProcessVerification()
// ============================================

// ============================================
// QUẢN LÝ ĐỊA ĐIỂM
// ============================================

/// <summary>
/// Lấy danh sách địa điểm của công ty
/// </summary>
public async Task<List<CompanyLocationDto>> GetCompanyLocations(int companyId)
{
    var locations = await _context.CompanyLocations
        .Where(l => l.CompanyId == companyId)
        .OrderByDescending(l => l.IsHeadquarter)
        .ThenBy(l => l.CreatedAt)
        .AsNoTracking()
        .ToListAsync();

    return locations.Select(l => new CompanyLocationDto
    {
        Id = l.Id,
        CompanyId = l.CompanyId,
        Address = l.Address ?? "",
        City = l.City,
        Country = l.Country,
        IsHeadquarter = l.IsHeadquarter ?? false,
        CreatedAt = l.CreatedAt
    }).ToList();
}

/// <summary>
/// Thêm địa điểm mới
/// </summary>
public async Task<CompanyLocationDto> AddLocation(int userId, AddLocationRequest request)
{
    var company = await _context.Companies
        .FirstOrDefaultAsync(c => c.UserId == userId);

    if (company == null)
    {
        throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
    }

    // Nếu set IsHeadquarter = true, cập nhật các địa điểm khác
    if (request.IsHeadquarter)
    {
        var existingLocations = await _context.CompanyLocations
            .Where(l => l.CompanyId == company.Id)
            .ToListAsync();

        foreach (var loc in existingLocations)
        {
            loc.IsHeadquarter = false;
        }
    }

    var newLocation = new CompanyLocation
    {
        CompanyId = company.Id,
        Address = request.Address,
        City = request.City,
        Country = request.Country ?? "Vietnam",
        IsHeadquarter = request.IsHeadquarter,
        CreatedAt = DateTime.UtcNow
    };

    _context.CompanyLocations.Add(newLocation);
    await _context.SaveChangesAsync();

    return new CompanyLocationDto
    {
        Id = newLocation.Id,
        CompanyId = newLocation.CompanyId,
        Address = newLocation.Address ?? "",
        City = newLocation.City,
        Country = newLocation.Country,
        IsHeadquarter = newLocation.IsHeadquarter ?? false,
        CreatedAt = newLocation.CreatedAt
    };
}

/// <summary>
/// Cập nhật địa điểm
/// </summary>
public async Task<CompanyLocationDto> UpdateLocation(int userId, int locationId, UpdateLocationRequest request)
{
    var company = await _context.Companies
        .FirstOrDefaultAsync(c => c.UserId == userId);

    if (company == null)
    {
        throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
    }

    var location = await _context.CompanyLocations
        .FirstOrDefaultAsync(l => l.Id == locationId && l.CompanyId == company.Id);

    if (location == null)
    {
        throw new KeyNotFoundException("Không tìm thấy địa điểm");
    }

    // Nếu set IsHeadquarter = true, cập nhật các địa điểm khác
    if (request.IsHeadquarter && !(location.IsHeadquarter ?? false))
    {
        var otherLocations = await _context.CompanyLocations
            .Where(l => l.CompanyId == company.Id && l.Id != locationId)
            .ToListAsync();

        foreach (var loc in otherLocations)
        {
            loc.IsHeadquarter = false;
        }
    }

    // Cập nhật thông tin
    location.Address = request.Address;
    location.City = request.City;
    location.Country = request.Country;
    location.IsHeadquarter = request.IsHeadquarter;

    await _context.SaveChangesAsync();

    return new CompanyLocationDto
    {
        Id = location.Id,
        CompanyId = location.CompanyId,
        Address = location.Address ?? "",
        City = location.City,
        Country = location.Country,
        IsHeadquarter = location.IsHeadquarter ?? false,
        CreatedAt = location.CreatedAt
    };
}

/// <summary>
/// Xóa địa điểm
/// </summary>
public async Task DeleteLocation(int userId, int locationId)
{
    var company = await _context.Companies
        .FirstOrDefaultAsync(c => c.UserId == userId);

    if (company == null)
    {
        throw new KeyNotFoundException("Không tìm thấy thông tin công ty");
    }

    var location = await _context.CompanyLocations
        .FirstOrDefaultAsync(l => l.Id == locationId && l.CompanyId == company.Id);

    if (location == null)
    {
        throw new KeyNotFoundException("Không tìm thấy địa điểm");
    }

    _context.CompanyLocations.Remove(location);
    await _context.SaveChangesAsync();
}

// ============================================
// SEARCH & FILTER (PUBLIC)
// ============================================

/// <summary>
/// Tìm kiếm công ty
/// </summary>
public async Task<List<CompanyDto>> SearchCompanies(
    string? keyword, 
    string? industry, 
    string? size, 
    int page = 1, 
    int pageSize = 20)
{
    var query = _context.Companies
        .Include(c => c.Jobs)
        .Include(c => c.CompanyLocations)
        .AsQueryable();

    // Filter theo keyword (tên công ty, mô tả)
    if (!string.IsNullOrWhiteSpace(keyword))
    {
        keyword = keyword.ToLower();
        query = query.Where(c => 
            c.Name.ToLower().Contains(keyword) || 
            (c.Description != null && c.Description.ToLower().Contains(keyword))
        );
    }

    // Filter theo ngành nghề
    if (!string.IsNullOrWhiteSpace(industry))
    {
        query = query.Where(c => c.Industry == industry);
    }

    // Filter theo quy mô
    if (!string.IsNullOrWhiteSpace(size))
    {
        query = query.Where(c => c.Size == size);
    }

    // Chỉ lấy công ty đã verified hoặc pending (không lấy rejected)
    query = query.Where(c => c.VerificationStatus == "verified" || c.VerificationStatus == "pending");

    // Phân trang
    var companies = await query
        .OrderByDescending(c => c.VerificationStatus == "verified") // Verified lên trước
        .ThenByDescending(c => c.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return companies.Select(c => new CompanyDto
    {
        Id = c.Id,
        UserId = c.UserId,
        Name = c.Name,
        Logo = c.Logo,
        Industry = c.Industry,
        Size = c.Size,
        Website = c.Website,
        VerificationStatus = c.VerificationStatus ?? "pending",
        CreatedAt = c.CreatedAt,
        TotalJobs = c.Jobs.Count(j => j.Status == "active"),
        TotalLocations = c.CompanyLocations.Count
    }).ToList();
}

/// <summary>
/// Lấy tất cả công ty
/// </summary>
public async Task<List<CompanyDto>> GetAllCompanies(int page = 1, int pageSize = 20)
{
    return await SearchCompanies(null, null, null, page, pageSize);
}

// ============================================
// HELPER METHODS: FILE HANDLING
// ============================================

/// <summary>
/// Validate image file
/// </summary>
private void ValidateImageFile(IFormFile file, int maxSizeMB)
{
    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

    if (!allowedExtensions.Contains(extension))
    {
        throw new InvalidOperationException("Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif)");
    }

    if (file.Length > maxSizeMB * 1024 * 1024)
    {
        throw new InvalidOperationException($"Kích thước file không được vượt quá {maxSizeMB}MB");
    }
}

/// <summary>
/// Save file to disk
/// </summary>
private async Task<string> SaveFile(IFormFile file, string folder, string fileName)
{
    var uploadPath = Path.Combine(_env.WebRootPath, "uploads", folder);

    // Tạo thư mục nếu chưa tồn tại
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

    return $"/uploads/{folder}/{fileName}";
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
        // Log error but don't throw
        // TODO: Add logging
    }
}
    }
}
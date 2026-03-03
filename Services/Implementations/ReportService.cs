using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Data;
using TimDongDoi.API.DTOs.Report;
using TimDongDoi.API.Models;
using TimDongDoi.API.Services.Interfaces;

namespace TimDongDoi.API.Services.Implementations;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReportDto> CreateReport(int reporterId, CreateReportRequest request)
    {
        // Validate type
        var validTypes = new[] { "user", "job", "review", "project" };
        if (!validTypes.Contains(request.Type))
            throw new InvalidOperationException("Invalid report type. Must be: user, job, review, project");

        // Validate target tồn tại theo type
        switch (request.Type)
        {
            case "user":
                if (request.ReportedUserId == null)
                    throw new InvalidOperationException("ReportedUserId is required for type 'user'");
                if (request.ReportedUserId == reporterId)
                    throw new InvalidOperationException("Cannot report yourself");
                var userExists = await _context.Users.AnyAsync(u => u.Id == request.ReportedUserId);
                if (!userExists) throw new KeyNotFoundException("Reported user not found");
                break;

            case "job":
                if (request.ReportedJobId == null)
                    throw new InvalidOperationException("ReportedJobId is required for type 'job'");
                var jobExists = await _context.Jobs.AnyAsync(j => j.Id == request.ReportedJobId);
                if (!jobExists) throw new KeyNotFoundException("Reported job not found");
                break;

            case "review":
                if (request.ReportedReviewId == null)
                    throw new InvalidOperationException("ReportedReviewId is required for type 'review'");
                var reviewExists = await _context.Reviews.AnyAsync(r => r.Id == request.ReportedReviewId);
                if (!reviewExists) throw new KeyNotFoundException("Reported review not found");
                break;

            case "project":
                if (request.ReportedProjectId == null)
                    throw new InvalidOperationException("ReportedProjectId is required for type 'project'");
                var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ReportedProjectId);
                if (!projectExists) throw new KeyNotFoundException("Reported project not found");
                break;
        }

        // Kiểm tra đã report chưa (tránh spam)
        var alreadyReported = await _context.Reports.AnyAsync(r =>
            r.ReporterId == reporterId &&
            r.Type == request.Type &&
            r.ReportedUserId == request.ReportedUserId &&
            r.ReportedJobId == request.ReportedJobId &&
            r.ReportedReviewId == request.ReportedReviewId &&
            r.ReportedProjectId == request.ReportedProjectId &&
            r.Status == "pending");

        if (alreadyReported)
            throw new InvalidOperationException("You already have a pending report for this item");

        var report = new Report
        {
            ReporterId = reporterId,
            Type = request.Type,
            ReportedUserId = request.ReportedUserId,
            ReportedJobId = request.ReportedJobId,
            ReportedReviewId = request.ReportedReviewId,
            ReportedProjectId = request.ReportedProjectId,
            Reason = request.Reason,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        var reporter = await _context.Users.FindAsync(reporterId);

        return MapToDto(report, reporter?.FullName ?? string.Empty);
    }

    public async Task<ReportListResponse> GetMyReports(int userId, int page, int pageSize)
    {
        var query = _context.Reports
            .Where(r => r.ReporterId == userId);

        var totalCount = await query.CountAsync();

        var reports = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var reporter = await _context.Users.FindAsync(userId);
        var reporterName = reporter?.FullName ?? string.Empty;

        return new ReportListResponse
        {
            Reports = reports.Select(r => MapToDto(r, reporterName)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ReportListResponse> GetAllReports(
        int page, int pageSize, string? status, string? type)
    {
        var query = _context.Reports.AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);

        if (!string.IsNullOrEmpty(type))
            query = query.Where(r => r.Type == type);

        var totalCount = await query.CountAsync();

        var reports = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Lấy tên reporters
        var reporterIds = reports.Select(r => r.ReporterId).Distinct().ToList();
        var reporters = await _context.Users
            .Where(u => reporterIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.FullName ?? string.Empty);

        return new ReportListResponse
        {
            Reports = reports.Select(r =>
                MapToDto(r, reporters.GetValueOrDefault(r.ReporterId, string.Empty))).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ReportDto> GetReportById(int reportId)
    {
        var report = await _context.Reports.FindAsync(reportId)
            ?? throw new KeyNotFoundException("Report not found");

        var reporter = await _context.Users.FindAsync(report.ReporterId);
        return MapToDto(report, reporter?.FullName ?? string.Empty);
    }

    public async Task<ReportDto> HandleReport(int adminId, int reportId, HandleReportRequest request)
    {
        var validStatuses = new[] { "resolved", "dismissed" };
        if (!validStatuses.Contains(request.Status))
            throw new InvalidOperationException("Status must be: resolved or dismissed");

        var report = await _context.Reports.FindAsync(reportId)
            ?? throw new KeyNotFoundException("Report not found");

        if (report.Status != "pending")
            throw new InvalidOperationException("Report has already been handled");

        report.Status = request.Status;
        report.HandledBy = adminId;
        report.AdminNote = request.AdminNote;
        report.ResolvedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var reporter = await _context.Users.FindAsync(report.ReporterId);
        return MapToDto(report, reporter?.FullName ?? string.Empty);
    }

    private static ReportDto MapToDto(Report report, string reporterName) => new()
    {
        Id = report.Id,
        ReporterId = report.ReporterId,
        ReporterName = reporterName,
        Type = report.Type,
        ReportedUserId = report.ReportedUserId,
        ReportedJobId = report.ReportedJobId,
        ReportedReviewId = report.ReportedReviewId,
        ReportedProjectId = report.ReportedProjectId,
        Reason = report.Reason,
        Status = report.Status,
        HandledBy = report.HandledBy,
        AdminNote = report.AdminNote,
        CreatedAt = report.CreatedAt ?? DateTime.UtcNow,
        ResolvedAt = report.ResolvedAt
    };
}
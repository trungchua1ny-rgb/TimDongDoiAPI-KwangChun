using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Models;

namespace TimDongDoi.API.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<UserSkill> UserSkills { get; set; } = null!;
    // --- Các DbSet (Giữ nguyên) ---
    public virtual DbSet<AdminLog> AdminLogs { get; set; }
    public virtual DbSet<Application> Applications { get; set; }
    public virtual DbSet<ApplicationTest> ApplicationTests { get; set; }
    // ... (Giữ nguyên các DbSet khác) ...
    public virtual DbSet<User> Users { get; set; }
    // ... (Giữ nguyên các DbSet khác) ...
    // Thêm các DbSet này
public virtual DbSet<Company> Companies { get; set; }
public virtual DbSet<CompanyLocation> CompanyLocations { get; set; }
public virtual DbSet<CompanyVerification> CompanyVerifications { get; set; }
public virtual DbSet<UserExperience> UserExperiences { get; set; }
public virtual DbSet<UserEducation> UserEducations { get; set; }
public virtual DbSet<Job> Jobs { get; set; }
public virtual DbSet<JobSkill> JobSkills { get; set; }
public virtual DbSet<SavedJob> SavedJobs { get; set; }
public virtual DbSet<Test> Tests { get; set; }
public virtual DbSet<TestQuestion> TestQuestions { get; set; }
public virtual DbSet<JobTest> JobTests { get; set; }
public virtual DbSet<Interview> Interviews { get; set; }
public virtual DbSet<Project> Projects { get; set; }
public virtual DbSet<ProjectPosition> ProjectPositions { get; set; }
public virtual DbSet<ProjectPositionSkill> ProjectPositionSkills { get; set; }
public virtual DbSet<ProjectApplication> ProjectApplications { get; set; }
public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
public virtual DbSet<Review> Reviews { get; set; }
public virtual DbSet<Notification> Notifications { get; set; }
public virtual DbSet<Message> Messages { get; set; }
public virtual DbSet<Report> Reports { get; set; }
    // ❌ ĐÃ XÓA: Phương thức OnConfiguring đã bị xóa. Ứng dụng sẽ đọc chuỗi kết nối
    // từ Program.cs (builder.Services.AddDbContext)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        // Trong file AppDbContext.cs, bên trong hàm OnModelCreating

modelBuilder.Entity<Message>(entity =>
{
    // Liên kết Message.FromUser (người gửi) với User.MessageFromUsers (danh sách tin nhắn đã gửi)
    entity.HasOne(m => m.FromUser)
          .WithMany(u => u.MessageFromUsers) // <-- Phải khớp với tên trong User.cs
          .HasForeignKey(m => m.FromUserId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Messages_FromUser");

    // Liên kết Message.ToUser (người nhận) với User.MessageToUsers (danh sách tin nhắn đã nhận)
    entity.HasOne(m => m.ToUser)
          .WithMany(u => u.MessageToUsers) // <-- Phải khớp với tên trong User.cs
          .HasForeignKey(m => m.ToUserId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Messages_ToUser");
});
        // ... (Giữ nguyên các cấu hình Entity khác) ...

        // --- CHÚ Ý: CHỈNH SỬA ENTITY USER ---
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FB30C90E4");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164ED331F57").IsUnique();

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.Role, "idx_users_role");

            entity.HasIndex(e => e.Status, "idx_users_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AboutMe).HasColumnName("about_me");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CvFile)
                .HasMaxLength(255)
                .HasColumnName("cv_file");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(100)
                .HasColumnName("job_title");

            // ❌ ĐÃ XÓA DÒNG GÂY LỖI CS1061:
            // entity.Property(e => e.Password)
            //     .HasMaxLength(255)
            //     .HasColumnName("password"); 

            // ✅ BỔ SUNG CẤU HÌNH CHO 2 TRƯỜNG MỚI (nếu cần đặt tên cột khác)
            // entity.Property(e => e.PasswordHash) 
            //     .HasColumnName("password_hash");
            // entity.Property(e => e.PasswordSalt)
            //     .HasColumnName("password_salt");

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("user")
                .HasColumnName("role");
            entity.Property(e => e.SalaryExpectation).HasColumnName("salary_expectation");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
                        // Thêm vào cuối phương thức OnModelCreating, trước OnModelCreatingPartial
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("companies");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasMaxLength(200).HasColumnName("name");
            entity.Property(e => e.Logo).HasMaxLength(255).HasColumnName("logo");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Website).HasMaxLength(255).HasColumnName("website");
            entity.Property(e => e.Industry).HasMaxLength(100).HasColumnName("industry");
            entity.Property(e => e.Size).HasMaxLength(50).HasColumnName("size");
            entity.Property(e => e.FoundedYear).HasColumnName("founded_year");
            entity.Property(e => e.VerificationStatus).HasMaxLength(20).HasDefaultValue("pending").HasColumnName("verification_status");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.User).WithOne(p => p.Company)
                .HasForeignKey<Company>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        });

        // ... (Giữ nguyên các cấu hình Entity khác) ...

        modelBuilder.Entity<UserSkill>(entity =>
    {
        entity.ToTable("user_skills");

        // Map tên cột
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.SkillId).HasColumnName("skill_id");
        entity.Property(e => e.Level).HasColumnName("level");
        entity.Property(e => e.YearsExperience).HasColumnName("years_experience");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
    });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("skills");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Popularity).HasColumnName("popularity");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });
        // ============================================
        // THÊM VÀO CUỐI OnModelCreating()
        // TRƯỚC DÒNG: OnModelCreatingPartial(modelBuilder);
        // ============================================

        // ==================== COMPANY VERIFICATION ====================
        // ==================== COMPANY VERIFICATION ====================
        // THAY THẾ đoạn cũ bằng code này
        modelBuilder.Entity<CompanyVerification>(entity =>
        {
            entity.ToTable("company_verifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");

            // ⚠️ Map VerifiedBy vào cột verified_by (cột mới)
            entity.Property(e => e.VerifiedBy).HasColumnName("verified_by");

            entity.Property(e => e.DocumentType).HasMaxLength(50).HasColumnName("document_type");
            entity.Property(e => e.DocumentUrl).HasMaxLength(255).HasColumnName("document_url");
            entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("pending").HasColumnName("status");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())").HasColumnName("submitted_at");

            // ⚠️ Map VerifiedAt vào cột verified_at (cột mới)
            entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

            // Relationships
            entity.HasOne(d => d.Company).WithMany(p => p.CompanyVerifications)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.VerifiedByNavigation).WithMany(p => p.CompanyVerifications)
                .HasForeignKey(d => d.VerifiedBy)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<CompanyLocation>(entity =>
        {
            // 1. Ánh xạ tên bảng
            entity.ToTable("company_locations");

            // 2. (Khuyên dùng) Ánh xạ luôn các cột
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.IsHeadquarter).HasColumnName("is_headquarter");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });
        // File: Data/AppDbContext.cs (Trong OnModelCreating)

        // ... (Giữ nguyên các cấu hình Entity khác) ...
    
        // =============================================================
        // CẤU HÌNH Job (FIX LỖI 500 MỚI)
        // =============================================================
        modelBuilder.Entity<Job>(entity =>
        {
            // 1. Ánh xạ tên bảng
            entity.ToTable("jobs");

            // 2. Ánh xạ tên cột (Dựa trên lỗi của bạn)
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Requirements).HasColumnName("requirements");
            entity.Property(e => e.Benefits).HasColumnName("benefits");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.SalaryMin).HasColumnName("salary_min");
            entity.Property(e => e.SalaryMax).HasColumnName("salary_max");
            entity.Property(e => e.SalaryCurrency).HasColumnName("salary_currency");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.LocationType).HasColumnName("location_type");
            entity.Property(e => e.Positions).HasColumnName("positions");
            entity.Property(e => e.Deadline).HasColumnName("deadline");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Views).HasColumnName("views");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });
        
    // ============================================
// THÊM VÀO CUỐI OnModelCreating()
// TRƯỚC DÒNG: OnModelCreatingPartial(modelBuilder);
// ============================================

// ==================== JOB ====================
modelBuilder.Entity<Job>(entity =>
{
    entity.ToTable("jobs");
    
    entity.Property(e => e.Id).HasColumnName("id");
    entity.Property(e => e.CompanyId).HasColumnName("company_id");
    entity.Property(e => e.Title).HasMaxLength(200).HasColumnName("title");
    entity.Property(e => e.Description).HasColumnName("description");
    entity.Property(e => e.Requirements).HasColumnName("requirements");
    entity.Property(e => e.Benefits).HasColumnName("benefits");
    entity.Property(e => e.Type).HasMaxLength(20).HasColumnName("type");
    entity.Property(e => e.Level).HasMaxLength(20).HasColumnName("level");
    entity.Property(e => e.SalaryMin).HasColumnName("salary_min");
    entity.Property(e => e.SalaryMax).HasColumnName("salary_max");
    entity.Property(e => e.SalaryCurrency).HasMaxLength(10).HasDefaultValue("VND").HasColumnName("salary_currency");
    entity.Property(e => e.Location).HasMaxLength(255).HasColumnName("location");
    entity.Property(e => e.LocationType).HasMaxLength(20).HasColumnName("location_type");
    entity.Property(e => e.Positions).HasDefaultValue(1).HasColumnName("positions");
    entity.Property(e => e.Deadline).HasColumnName("deadline");
    entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("open").HasColumnName("status");
    entity.Property(e => e.Views).HasDefaultValue(0).HasColumnName("views");
    entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnName("created_at");
    entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnName("updated_at");

    // Relationships
    entity.HasOne(d => d.Company).WithMany(p => p.Jobs)
        .HasForeignKey(d => d.CompanyId)
        .OnDelete(DeleteBehavior.Cascade);
});

// ==================== JOB SKILL ====================
modelBuilder.Entity<JobSkill>(entity =>
{
    entity.ToTable("job_skills");
    
    entity.Property(e => e.Id).HasColumnName("id");
    entity.Property(e => e.JobId).HasColumnName("job_id");
    entity.Property(e => e.SkillId).HasColumnName("skill_id");
    entity.Property(e => e.IsRequired).HasDefaultValue(true).HasColumnName("is_required");
    entity.Property(e => e.Level).HasMaxLength(20).HasColumnName("level");

    // Relationships
    entity.HasOne(d => d.Job).WithMany(p => p.JobSkills)
        .HasForeignKey(d => d.JobId)
        .OnDelete(DeleteBehavior.Cascade);

    entity.HasOne(d => d.Skill).WithMany(p => p.JobSkills)
        .HasForeignKey(d => d.SkillId)
        .OnDelete(DeleteBehavior.Cascade);
});

        // ==================== SAVED JOB ====================
        modelBuilder.Entity<SavedJob>(entity =>
        {
            entity.ToTable("saved_jobs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnName("created_at");

            // Relationships
            entity.HasOne(d => d.User).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Job).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.NoAction);

            // Unique constraint
            entity.HasIndex(e => new { e.UserId, e.JobId }).IsUnique();
        });
// ============================================
// THÊM VÀO CUỐI OnModelCreating()
// SAU PHẦN JOB, JOB_SKILLS, SAVED_JOBS
// TRƯỚC DÒNG: OnModelCreatingPartial(modelBuilder);
// ============================================

// ==================== APPLICATION ====================
modelBuilder.Entity<Application>(entity =>
{
    entity.ToTable("applications");
    
    entity.Property(e => e.Id).HasColumnName("id");
    entity.Property(e => e.UserId).HasColumnName("user_id");
    entity.Property(e => e.JobId).HasColumnName("job_id");
    entity.Property(e => e.CoverLetter).HasColumnName("cover_letter");
    entity.Property(e => e.CvFile).HasMaxLength(255).HasColumnName("cv_file");
    entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("pending").HasColumnName("status");
    entity.Property(e => e.RejectReason).HasColumnName("reject_reason");
    entity.Property(e => e.AppliedAt).HasDefaultValueSql("(getdate())").HasColumnName("applied_at");
    entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnName("updated_at");

    // Relationships
    entity.HasOne(d => d.User).WithMany(p => p.Applications)
        .HasForeignKey(d => d.UserId)
        .OnDelete(DeleteBehavior.NoAction);

    entity.HasOne(d => d.Job).WithMany(p => p.Applications)
        .HasForeignKey(d => d.JobId)
        .OnDelete(DeleteBehavior.NoAction);

    // Unique constraint
    entity.HasIndex(e => new { e.UserId, e.JobId }).IsUnique();
});
// ... (Giữ nguyên các cấu hình Entity khác) ...
        OnModelCreatingPartial(modelBuilder);
    }   

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
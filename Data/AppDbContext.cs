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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
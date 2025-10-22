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

    // ❌ ĐÃ XÓA: Phương thức OnConfiguring đã bị xóa. Ứng dụng sẽ đọc chuỗi kết nối
    // từ Program.cs (builder.Services.AddDbContext)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
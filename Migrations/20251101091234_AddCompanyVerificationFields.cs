using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimDongDoi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyVerificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Job_JobId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTests_Test_TestId",
                table: "ApplicationTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_users_UserId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyLocation_Company_CompanyId",
                table: "CompanyLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyVerification_Company_CompanyId",
                table: "CompanyVerification");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyVerification_users_ApprovedByNavigationId",
                table: "CompanyVerification");

            migrationBuilder.DropForeignKey(
                name: "FK_Interview_Applications_ApplicationId",
                table: "Interview");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Company_CompanyId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkill_Job_JobId",
                table: "JobSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkill_Skill_SkillId",
                table: "JobSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTest_Job_JobId",
                table: "JobTest");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTest_Test_TestId",
                table: "JobTest");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_users_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_users_UserId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplication_ProjectPosition_PositionId",
                table: "ProjectApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplication_Project_ProjectId",
                table: "ProjectApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplication_users_UserId",
                table: "ProjectApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMember_ProjectPosition_PositionId",
                table: "ProjectMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMember_Project_ProjectId",
                table: "ProjectMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMember_users_UserId",
                table: "ProjectMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPosition_Project_ProjectId",
                table: "ProjectPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPositionSkill_ProjectPosition_PositionId",
                table: "ProjectPositionSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPositionSkill_Skill_SkillId",
                table: "ProjectPositionSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Job_ReportedJobId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Project_ReportedProjectId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Review_ReportedReviewId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Applications_ApplicationId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_ProjectMember_ProjectMemberId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedJob_Job_JobId",
                table: "SavedJob");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedJob_users_UserId",
                table: "SavedJob");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Company_CompanyId",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestion_Test_TestId",
                table: "TestQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEducation_users_UserId",
                table: "UserEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExperience_users_UserId",
                table: "UserExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Skill_SkillId",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_users_UserId",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExperience",
                table: "UserExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEducation",
                table: "UserEducation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Test",
                table: "Test");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedJob",
                table: "SavedJob");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPositionSkill",
                table: "ProjectPositionSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPosition",
                table: "ProjectPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectApplication",
                table: "ProjectApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTest",
                table: "JobTest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkill",
                table: "JobSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Job",
                table: "Job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interview",
                table: "Interview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyVerification",
                table: "CompanyVerification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyLocation",
                table: "CompanyLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Company",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "CompanyVerification");

            migrationBuilder.DropColumn(
                name: "BusinessLicense",
                table: "CompanyVerification");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "CompanyVerification");

            migrationBuilder.RenameTable(
                name: "UserSkill",
                newName: "user_skills");

            migrationBuilder.RenameTable(
                name: "UserExperience",
                newName: "UserExperiences");

            migrationBuilder.RenameTable(
                name: "UserEducation",
                newName: "UserEducations");

            migrationBuilder.RenameTable(
                name: "TestQuestion",
                newName: "TestQuestions");

            migrationBuilder.RenameTable(
                name: "Test",
                newName: "Tests");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "skills");

            migrationBuilder.RenameTable(
                name: "SavedJob",
                newName: "SavedJobs");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.RenameTable(
                name: "ProjectPositionSkill",
                newName: "ProjectPositionSkills");

            migrationBuilder.RenameTable(
                name: "ProjectPosition",
                newName: "ProjectPositions");

            migrationBuilder.RenameTable(
                name: "ProjectMember",
                newName: "ProjectMembers");

            migrationBuilder.RenameTable(
                name: "ProjectApplication",
                newName: "ProjectApplications");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "JobTest",
                newName: "JobTests");

            migrationBuilder.RenameTable(
                name: "JobSkill",
                newName: "JobSkills");

            migrationBuilder.RenameTable(
                name: "Job",
                newName: "Jobs");

            migrationBuilder.RenameTable(
                name: "Interview",
                newName: "Interviews");

            migrationBuilder.RenameTable(
                name: "CompanyVerification",
                newName: "company_verifications");

            migrationBuilder.RenameTable(
                name: "CompanyLocation",
                newName: "CompanyLocations");

            migrationBuilder.RenameTable(
                name: "Company",
                newName: "companies");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "user_skills",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "user_skills",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_skills",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "YearsExperience",
                table: "user_skills",
                newName: "years_experience");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_skills",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "user_skills",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "user_skills",
                newName: "skill_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "user_skills",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_UserId",
                table: "user_skills",
                newName: "IX_user_skills_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_SkillId",
                table: "user_skills",
                newName: "IX_user_skills_skill_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserExperience_UserId",
                table: "UserExperiences",
                newName: "IX_UserExperiences_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEducation_UserId",
                table: "UserEducations",
                newName: "IX_UserEducations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestion_TestId",
                table: "TestQuestions",
                newName: "IX_TestQuestions_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_Test_CompanyId",
                table: "Tests",
                newName: "IX_Tests_CompanyId");

            migrationBuilder.RenameColumn(
                name: "Popularity",
                table: "skills",
                newName: "popularity");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "skills",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "skills",
                newName: "icon");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "skills",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "skills",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "skills",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_SavedJob_UserId",
                table: "SavedJobs",
                newName: "IX_SavedJobs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SavedJob_JobId",
                table: "SavedJobs",
                newName: "IX_SavedJobs_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProjectMemberId",
                table: "Reviews",
                newName: "IX_Reviews_ProjectMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ApplicationId",
                table: "Reviews",
                newName: "IX_Reviews_ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ReportedReviewId",
                table: "Reports",
                newName: "IX_Reports_ReportedReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ReportedProjectId",
                table: "Reports",
                newName: "IX_Reports_ReportedProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ReportedJobId",
                table: "Reports",
                newName: "IX_Reports_ReportedJobId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPositionSkill_SkillId",
                table: "ProjectPositionSkills",
                newName: "IX_ProjectPositionSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPositionSkill_PositionId",
                table: "ProjectPositionSkills",
                newName: "IX_ProjectPositionSkills_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPosition_ProjectId",
                table: "ProjectPositions",
                newName: "IX_ProjectPositions_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMember_UserId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMember_ProjectId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMember_PositionId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplication_UserId",
                table: "ProjectApplications",
                newName: "IX_ProjectApplications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplication_ProjectId",
                table: "ProjectApplications",
                newName: "IX_ProjectApplications_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplication_PositionId",
                table: "ProjectApplications",
                newName: "IX_ProjectApplications_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_UserId",
                table: "Projects",
                newName: "IX_Projects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTest_TestId",
                table: "JobTests",
                newName: "IX_JobTests_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTest_JobId",
                table: "JobTests",
                newName: "IX_JobTests_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkill_SkillId",
                table: "JobSkills",
                newName: "IX_JobSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkill_JobId",
                table: "JobSkills",
                newName: "IX_JobSkills_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_CompanyId",
                table: "Jobs",
                newName: "IX_Jobs_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Interview_ApplicationId",
                table: "Interviews",
                newName: "IX_Interviews_ApplicationId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "company_verifications",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "company_verifications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "company_verifications",
                newName: "submitted_at");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "company_verifications",
                newName: "company_id");

            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "company_verifications",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "ApprovedByNavigationId",
                table: "company_verifications",
                newName: "verified_by");

            migrationBuilder.RenameColumn(
                name: "ApprovedAt",
                table: "company_verifications",
                newName: "verified_at");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyVerification_CompanyId",
                table: "company_verifications",
                newName: "IX_company_verifications_company_id");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyVerification_ApprovedByNavigationId",
                table: "company_verifications",
                newName: "IX_company_verifications_verified_by");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyLocation_CompanyId",
                table: "CompanyLocations",
                newName: "IX_CompanyLocations_CompanyId");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "companies",
                newName: "website");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "companies",
                newName: "size");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "companies",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "companies",
                newName: "logo");

            migrationBuilder.RenameColumn(
                name: "Industry",
                table: "companies",
                newName: "industry");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "companies",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "companies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VerificationStatus",
                table: "companies",
                newName: "verification_status");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "companies",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "companies",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "FoundedYear",
                table: "companies",
                newName: "founded_year");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "companies",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Company_UserId",
                table: "companies",
                newName: "IX_companies_user_id");

            migrationBuilder.AlterColumn<string>(
                name: "level",
                table: "user_skills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "company_verifications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "submitted_at",
                table: "company_verifications",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "document_type",
                table: "company_verifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "document_url",
                table: "company_verifications",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "website",
                table: "companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "size",
                table: "companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "companies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "logo",
                table: "companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "industry",
                table: "companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "verification_status",
                table: "companies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "companies",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_skills",
                table: "user_skills",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExperiences",
                table: "UserExperiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEducations",
                table: "UserEducations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestions",
                table: "TestQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tests",
                table: "Tests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_skills",
                table: "skills",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedJobs",
                table: "SavedJobs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPositionSkills",
                table: "ProjectPositionSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPositions",
                table: "ProjectPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectApplications",
                table: "ProjectApplications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTests",
                table: "JobTests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_company_verifications",
                table: "company_verifications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyLocations",
                table: "CompanyLocations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_companies",
                table: "companies",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<int>(type: "int", nullable: false),
                    Message1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_FromUser",
                        column: x => x.FromUserId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Messages_ToUser",
                        column: x => x.ToUserId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToUserId",
                table: "Messages",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTests_Tests_TestId",
                table: "ApplicationTests",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_companies_users_user_id",
                table: "companies",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_company_verifications_companies_company_id",
                table: "company_verifications",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_company_verifications_users_verified_by",
                table: "company_verifications",
                column: "verified_by",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyLocations_companies_CompanyId",
                table: "CompanyLocations",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Applications_ApplicationId",
                table: "Interviews",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_companies_CompanyId",
                table: "Jobs",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_Jobs_JobId",
                table: "JobSkills",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_skills_SkillId",
                table: "JobSkills",
                column: "SkillId",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTests_Jobs_JobId",
                table: "JobTests",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTests_Tests_TestId",
                table: "JobTests",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplications_ProjectPositions_PositionId",
                table: "ProjectApplications",
                column: "PositionId",
                principalTable: "ProjectPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplications_Projects_ProjectId",
                table: "ProjectApplications",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplications_users_UserId",
                table: "ProjectApplications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_ProjectPositions_PositionId",
                table: "ProjectMembers",
                column: "PositionId",
                principalTable: "ProjectPositions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_users_UserId",
                table: "ProjectMembers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPositions_Projects_ProjectId",
                table: "ProjectPositions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPositionSkills_ProjectPositions_PositionId",
                table: "ProjectPositionSkills",
                column: "PositionId",
                principalTable: "ProjectPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPositionSkills_skills_SkillId",
                table: "ProjectPositionSkills",
                column: "SkillId",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_users_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Jobs_ReportedJobId",
                table: "Reports",
                column: "ReportedJobId",
                principalTable: "Jobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Projects_ReportedProjectId",
                table: "Reports",
                column: "ReportedProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Reviews_ReportedReviewId",
                table: "Reports",
                column: "ReportedReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Applications_ApplicationId",
                table: "Reviews",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProjectMembers_ProjectMemberId",
                table: "Reviews",
                column: "ProjectMemberId",
                principalTable: "ProjectMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_Jobs_JobId",
                table: "SavedJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_users_UserId",
                table: "SavedJobs",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Tests_TestId",
                table: "TestQuestions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_companies_CompanyId",
                table: "Tests",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_skills_skills_skill_id",
                table: "user_skills",
                column: "skill_id",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_skills_users_user_id",
                table: "user_skills",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducations_users_UserId",
                table: "UserEducations",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExperiences_users_UserId",
                table: "UserExperiences",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTests_Tests_TestId",
                table: "ApplicationTests");

            migrationBuilder.DropForeignKey(
                name: "FK_companies_users_user_id",
                table: "companies");

            migrationBuilder.DropForeignKey(
                name: "FK_company_verifications_companies_company_id",
                table: "company_verifications");

            migrationBuilder.DropForeignKey(
                name: "FK_company_verifications_users_verified_by",
                table: "company_verifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyLocations_companies_CompanyId",
                table: "CompanyLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Applications_ApplicationId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_companies_CompanyId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_Jobs_JobId",
                table: "JobSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_skills_SkillId",
                table: "JobSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTests_Jobs_JobId",
                table: "JobTests");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTests_Tests_TestId",
                table: "JobTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplications_ProjectPositions_PositionId",
                table: "ProjectApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplications_Projects_ProjectId",
                table: "ProjectApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectApplications_users_UserId",
                table: "ProjectApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_ProjectPositions_PositionId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_users_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPositions_Projects_ProjectId",
                table: "ProjectPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPositionSkills_ProjectPositions_PositionId",
                table: "ProjectPositionSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPositionSkills_skills_SkillId",
                table: "ProjectPositionSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_users_UserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Jobs_ReportedJobId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Projects_ReportedProjectId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Reviews_ReportedReviewId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Applications_ApplicationId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProjectMembers_ProjectMemberId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_Jobs_JobId",
                table: "SavedJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_users_UserId",
                table: "SavedJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Tests_TestId",
                table: "TestQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_companies_CompanyId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_user_skills_skills_skill_id",
                table: "user_skills");

            migrationBuilder.DropForeignKey(
                name: "FK_user_skills_users_user_id",
                table: "user_skills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEducations_users_UserId",
                table: "UserEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExperiences_users_UserId",
                table: "UserExperiences");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExperiences",
                table: "UserExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEducations",
                table: "UserEducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_skills",
                table: "user_skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tests",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestions",
                table: "TestQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_skills",
                table: "skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedJobs",
                table: "SavedJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPositionSkills",
                table: "ProjectPositionSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPositions",
                table: "ProjectPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectApplications",
                table: "ProjectApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTests",
                table: "JobTests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interviews",
                table: "Interviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyLocations",
                table: "CompanyLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_company_verifications",
                table: "company_verifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_companies",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "document_type",
                table: "company_verifications");

            migrationBuilder.DropColumn(
                name: "document_url",
                table: "company_verifications");

            migrationBuilder.RenameTable(
                name: "UserExperiences",
                newName: "UserExperience");

            migrationBuilder.RenameTable(
                name: "UserEducations",
                newName: "UserEducation");

            migrationBuilder.RenameTable(
                name: "user_skills",
                newName: "UserSkill");

            migrationBuilder.RenameTable(
                name: "Tests",
                newName: "Test");

            migrationBuilder.RenameTable(
                name: "TestQuestions",
                newName: "TestQuestion");

            migrationBuilder.RenameTable(
                name: "skills",
                newName: "Skill");

            migrationBuilder.RenameTable(
                name: "SavedJobs",
                newName: "SavedJob");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameTable(
                name: "ProjectPositionSkills",
                newName: "ProjectPositionSkill");

            migrationBuilder.RenameTable(
                name: "ProjectPositions",
                newName: "ProjectPosition");

            migrationBuilder.RenameTable(
                name: "ProjectMembers",
                newName: "ProjectMember");

            migrationBuilder.RenameTable(
                name: "ProjectApplications",
                newName: "ProjectApplication");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "JobTests",
                newName: "JobTest");

            migrationBuilder.RenameTable(
                name: "JobSkills",
                newName: "JobSkill");

            migrationBuilder.RenameTable(
                name: "Jobs",
                newName: "Job");

            migrationBuilder.RenameTable(
                name: "Interviews",
                newName: "Interview");

            migrationBuilder.RenameTable(
                name: "CompanyLocations",
                newName: "CompanyLocation");

            migrationBuilder.RenameTable(
                name: "company_verifications",
                newName: "CompanyVerification");

            migrationBuilder.RenameTable(
                name: "companies",
                newName: "Company");

            migrationBuilder.RenameIndex(
                name: "IX_UserExperiences_UserId",
                table: "UserExperience",
                newName: "IX_UserExperience_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEducations_UserId",
                table: "UserEducation",
                newName: "IX_UserEducation_UserId");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "UserSkill",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "UserSkill",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserSkill",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "years_experience",
                table: "UserSkill",
                newName: "YearsExperience");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserSkill",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "UserSkill",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "skill_id",
                table: "UserSkill",
                newName: "SkillId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "UserSkill",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_user_skills_user_id",
                table: "UserSkill",
                newName: "IX_UserSkill_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_user_skills_skill_id",
                table: "UserSkill",
                newName: "IX_UserSkill_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_Tests_CompanyId",
                table: "Test",
                newName: "IX_Test_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestions_TestId",
                table: "TestQuestion",
                newName: "IX_TestQuestion_TestId");

            migrationBuilder.RenameColumn(
                name: "popularity",
                table: "Skill",
                newName: "Popularity");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Skill",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "icon",
                table: "Skill",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "Skill",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Skill",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Skill",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_SavedJobs_UserId",
                table: "SavedJob",
                newName: "IX_SavedJob_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SavedJobs_JobId",
                table: "SavedJob",
                newName: "IX_SavedJob_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProjectMemberId",
                table: "Review",
                newName: "IX_Review_ProjectMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ApplicationId",
                table: "Review",
                newName: "IX_Review_ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportedReviewId",
                table: "Report",
                newName: "IX_Report_ReportedReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportedProjectId",
                table: "Report",
                newName: "IX_Report_ReportedProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportedJobId",
                table: "Report",
                newName: "IX_Report_ReportedJobId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_UserId",
                table: "Project",
                newName: "IX_Project_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPositionSkills_SkillId",
                table: "ProjectPositionSkill",
                newName: "IX_ProjectPositionSkill_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPositionSkills_PositionId",
                table: "ProjectPositionSkill",
                newName: "IX_ProjectPositionSkill_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPositions_ProjectId",
                table: "ProjectPosition",
                newName: "IX_ProjectPosition_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMember",
                newName: "IX_ProjectMember_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMember",
                newName: "IX_ProjectMember_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_PositionId",
                table: "ProjectMember",
                newName: "IX_ProjectMember_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplications_UserId",
                table: "ProjectApplication",
                newName: "IX_ProjectApplication_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplications_ProjectId",
                table: "ProjectApplication",
                newName: "IX_ProjectApplication_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectApplications_PositionId",
                table: "ProjectApplication",
                newName: "IX_ProjectApplication_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTests_TestId",
                table: "JobTest",
                newName: "IX_JobTest_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTests_JobId",
                table: "JobTest",
                newName: "IX_JobTest_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkills_SkillId",
                table: "JobSkill",
                newName: "IX_JobSkill_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_JobSkills_JobId",
                table: "JobSkill",
                newName: "IX_JobSkill_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_CompanyId",
                table: "Job",
                newName: "IX_Job_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Interviews_ApplicationId",
                table: "Interview",
                newName: "IX_Interview_ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyLocations_CompanyId",
                table: "CompanyLocation",
                newName: "IX_CompanyLocation_CompanyId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "CompanyVerification",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CompanyVerification",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "submitted_at",
                table: "CompanyVerification",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "CompanyVerification",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "verified_by",
                table: "CompanyVerification",
                newName: "ApprovedByNavigationId");

            migrationBuilder.RenameColumn(
                name: "verified_at",
                table: "CompanyVerification",
                newName: "ApprovedAt");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "CompanyVerification",
                newName: "TaxCode");

            migrationBuilder.RenameIndex(
                name: "IX_company_verifications_verified_by",
                table: "CompanyVerification",
                newName: "IX_CompanyVerification_ApprovedByNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_company_verifications_company_id",
                table: "CompanyVerification",
                newName: "IX_CompanyVerification_CompanyId");

            migrationBuilder.RenameColumn(
                name: "website",
                table: "Company",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "size",
                table: "Company",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Company",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "logo",
                table: "Company",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "industry",
                table: "Company",
                newName: "Industry");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Company",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Company",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "verification_status",
                table: "Company",
                newName: "VerificationStatus");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Company",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Company",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "founded_year",
                table: "Company",
                newName: "FoundedYear");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Company",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_companies_user_id",
                table: "Company",
                newName: "IX_Company_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "UserSkill",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CompanyVerification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "pending");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "CompanyVerification",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "ApprovedBy",
                table: "CompanyVerification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessLicense",
                table: "CompanyVerification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "CompanyVerification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VerificationStatus",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "pending");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Company",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExperience",
                table: "UserExperience",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEducation",
                table: "UserEducation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Test",
                table: "Test",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedJob",
                table: "SavedJob",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPositionSkill",
                table: "ProjectPositionSkill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPosition",
                table: "ProjectPosition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectApplication",
                table: "ProjectApplication",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTest",
                table: "JobTest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkill",
                table: "JobSkill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Job",
                table: "Job",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interview",
                table: "Interview",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyLocation",
                table: "CompanyLocation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyVerification",
                table: "CompanyVerification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Company",
                table: "Company",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Job_JobId",
                table: "Applications",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTests_Test_TestId",
                table: "ApplicationTests",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_users_UserId",
                table: "Company",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyLocation_Company_CompanyId",
                table: "CompanyLocation",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyVerification_Company_CompanyId",
                table: "CompanyVerification",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyVerification_users_ApprovedByNavigationId",
                table: "CompanyVerification",
                column: "ApprovedByNavigationId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interview_Applications_ApplicationId",
                table: "Interview",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Company_CompanyId",
                table: "Job",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkill_Job_JobId",
                table: "JobSkill",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkill_Skill_SkillId",
                table: "JobSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTest_Job_JobId",
                table: "JobTest",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTest_Test_TestId",
                table: "JobTest",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_users_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_users_UserId",
                table: "Project",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplication_ProjectPosition_PositionId",
                table: "ProjectApplication",
                column: "PositionId",
                principalTable: "ProjectPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplication_Project_ProjectId",
                table: "ProjectApplication",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectApplication_users_UserId",
                table: "ProjectApplication",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMember_ProjectPosition_PositionId",
                table: "ProjectMember",
                column: "PositionId",
                principalTable: "ProjectPosition",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMember_Project_ProjectId",
                table: "ProjectMember",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMember_users_UserId",
                table: "ProjectMember",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPosition_Project_ProjectId",
                table: "ProjectPosition",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPositionSkill_ProjectPosition_PositionId",
                table: "ProjectPositionSkill",
                column: "PositionId",
                principalTable: "ProjectPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPositionSkill_Skill_SkillId",
                table: "ProjectPositionSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Job_ReportedJobId",
                table: "Report",
                column: "ReportedJobId",
                principalTable: "Job",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Project_ReportedProjectId",
                table: "Report",
                column: "ReportedProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Review_ReportedReviewId",
                table: "Report",
                column: "ReportedReviewId",
                principalTable: "Review",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Applications_ApplicationId",
                table: "Review",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ProjectMember_ProjectMemberId",
                table: "Review",
                column: "ProjectMemberId",
                principalTable: "ProjectMember",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJob_Job_JobId",
                table: "SavedJob",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJob_users_UserId",
                table: "SavedJob",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Company_CompanyId",
                table: "Test",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestion_Test_TestId",
                table: "TestQuestion",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducation_users_UserId",
                table: "UserEducation",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExperience_users_UserId",
                table: "UserExperience",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Skill_SkillId",
                table: "UserSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_users_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

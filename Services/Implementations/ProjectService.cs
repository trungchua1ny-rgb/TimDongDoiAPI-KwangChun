using Microsoft.EntityFrameworkCore;
using TimDongDoi.API.Data;
using TimDongDoi.API.DTOs.Project;
using TimDongDoi.API.Models;
using TimDongDoi.API.Services.Interfaces;

namespace TimDongDoi.API.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        #region PROJECT CRUD

        public async Task<ProjectDto> CreateProject(int userId, CreateProjectRequest request)
        {
            var validTypes = new[] { "startup", "freelance", "side_project", "open_source" };
            if (!validTypes.Contains(request.Type))
                throw new InvalidOperationException("Invalid project type");

            var validLocationTypes = new[] { "remote", "onsite", "hybrid" };
            if (!validLocationTypes.Contains(request.LocationType))
                throw new InvalidOperationException("Invalid location type");

            var validCompensationTypes = new[] { "equity", "revenue_share", "paid", "volunteer" };
            if (!validCompensationTypes.Contains(request.CompensationType))
                throw new InvalidOperationException("Invalid compensation type");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = new Project
                {
                    UserId = userId,
                    Title = request.Title,
                    Description = request.Description,
                    Type = request.Type,
                    DurationMonths = request.DurationMonths,
                    LocationType = request.LocationType,
                    CompensationType = request.CompensationType,
                    CompensationDetails = request.CompensationDetails,
                    Status = "open",
                    Views = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                var founder = new ProjectMember
                {
                    ProjectId = project.Id,
                    UserId = userId,
                    PositionId = null,
                    RoleType = "founder",
                    Status = "active",
                    JoinedAt = DateOnly.FromDateTime(DateTime.Now),
                    LeftAt = null
                };
                _context.ProjectMembers.Add(founder);
                await _context.SaveChangesAsync();

                if (request.Positions != null && request.Positions.Any())
                {
                    foreach (var posDto in request.Positions)
                    {
                        var position = new ProjectPosition
                        {
                            ProjectId = project.Id,
                            Role = posDto.Role,
                            Quantity = posDto.Quantity,
                            Requirements = posDto.Requirements,
                            Status = "open",
                            CreatedAt = DateTime.Now
                        };
                        _context.ProjectPositions.Add(position);
                        await _context.SaveChangesAsync();

                        if (posDto.RequiredSkillIds != null && posDto.RequiredSkillIds.Any())
                        {
                            foreach (var skillId in posDto.RequiredSkillIds)
                            {
                                var skillExists = await _context.Skills.AnyAsync(s => s.Id == skillId);
                                if (!skillExists)
                                    throw new KeyNotFoundException($"Skill with ID {skillId} not found");

                                _context.ProjectPositionSkills.Add(new ProjectPositionSkill
                                {
                                    PositionId = position.Id,
                                    SkillId = skillId,
                                    IsRequired = true
                                });
                            }
                        }

                        if (posDto.PreferredSkillIds != null && posDto.PreferredSkillIds.Any())
                        {
                            foreach (var skillId in posDto.PreferredSkillIds)
                            {
                                var skillExists = await _context.Skills.AnyAsync(s => s.Id == skillId);
                                if (!skillExists)
                                    throw new KeyNotFoundException($"Skill with ID {skillId} not found");

                                _context.ProjectPositionSkills.Add(new ProjectPositionSkill
                                {
                                    PositionId = position.Id,
                                    SkillId = skillId,
                                    IsRequired = false
                                });
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return await GetProjectById(project.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ProjectDto> GetProjectById(int projectId)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.ProjectPositions)
                    .ThenInclude(pos => pos.ProjectPositionSkills)
                        .ThenInclude(s => s.Skill)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            var projectToUpdate = await _context.Projects.FindAsync(projectId);
            if (projectToUpdate != null)
            {
                projectToUpdate.Views = (projectToUpdate.Views ?? 0) + 1;
                await _context.SaveChangesAsync();
            }

            var positions = project.ProjectPositions.Select(pos => new PositionDto
            {
                Id = pos.Id,
                ProjectId = pos.ProjectId,
                Role = pos.Role,
                Quantity = pos.Quantity ?? 1,
                Requirements = pos.Requirements,
                Status = pos.Status ?? "open",
                CreatedAt = pos.CreatedAt ?? DateTime.Now,
                Skills = pos.ProjectPositionSkills.Select(s => new PositionSkillDto
                {
                    SkillId = s.SkillId,
                    SkillName = s.Skill.Name,
                    IsRequired = s.IsRequired ?? true
                }).ToList(),
                TotalApplications = _context.ProjectApplications.Count(a => a.PositionId == pos.Id),
                CurrentMembers = _context.ProjectMembers.Count(m => m.PositionId == pos.Id && m.Status == "active")
            }).ToList();

            var stats = await GetStatsForProject(projectId);

            return new ProjectDto
            {
                Id = project.Id,
                UserId = project.UserId,
                Title = project.Title,
                Description = project.Description,
                Type = project.Type,
                DurationMonths = project.DurationMonths,
                LocationType = project.LocationType,
                CompensationType = project.CompensationType,
                CompensationDetails = project.CompensationDetails,
                Status = project.Status ?? "open",
                Views = projectToUpdate?.Views ?? project.Views ?? 0,
                CreatedAt = project.CreatedAt ?? DateTime.Now,
                UpdatedAt = project.UpdatedAt ?? DateTime.Now,
                Owner = new UserBasicDto
                {
                    Id = project.User.Id,
                    FullName = project.User.FullName ?? "",
                    Avatar = project.User.Avatar,
                    JobTitle = project.User.JobTitle
                },
                Positions = positions,
                Stats = stats
            };
        }

        public async Task<ProjectDto> GetMyProject(int userId, int projectId)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new KeyNotFoundException("Project not found or you don't have permission");

            return await GetProjectById(projectId);
        }

        public async Task<List<ProjectListDto>> GetMyProjects(int userId, string? status, int page, int pageSize)
        {
            var query = _context.Projects
                .AsNoTracking()
                .Where(p => p.UserId == userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(p => p.Status == status);

            var projects = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.User)
                .ToListAsync();

            return projects.Select(p => new ProjectListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Type = p.Type,
                LocationType = p.LocationType,
                CompensationType = p.CompensationType,
                Status = p.Status ?? "open",
                Views = p.Views ?? 0,
                CreatedAt = p.CreatedAt ?? DateTime.Now,
                Owner = new UserBasicDto
                {
                    Id = p.User.Id,
                    FullName = p.User.FullName ?? "",
                    Avatar = p.User.Avatar,
                    JobTitle = p.User.JobTitle
                },
                OpenPositions = _context.ProjectPositions.Count(pos => pos.ProjectId == p.Id && pos.Status == "open"),
                TotalApplications = _context.ProjectApplications.Count(a => a.ProjectId == p.Id)
            }).ToList();
        }

        public async Task<ProjectDto> UpdateProject(int userId, int projectId, UpdateProjectRequest request)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            if (project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission");

            if (request.Title != null) project.Title = request.Title;
            if (request.Description != null) project.Description = request.Description;
            if (request.Type != null) project.Type = request.Type;
            if (request.DurationMonths.HasValue) project.DurationMonths = request.DurationMonths;
            if (request.LocationType != null) project.LocationType = request.LocationType;
            if (request.CompensationType != null) project.CompensationType = request.CompensationType;
            if (request.CompensationDetails != null) project.CompensationDetails = request.CompensationDetails;

            project.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return await GetProjectById(projectId);
        }

        public async Task DeleteProject(int userId, int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            if (project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission");

            var activeMembersCount = await _context.ProjectMembers
                .CountAsync(m => m.ProjectId == projectId && m.Status == "active" && m.RoleType != "founder");

            if (activeMembersCount > 0)
                throw new InvalidOperationException("Cannot delete project with active members");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        #endregion
        #region PROJECT STATUS

        public async Task<ProjectDto> CloseProject(int userId, int projectId)
        {
            var project = await VerifyProjectOwnership(userId, projectId);
            project.Status = "closed";
            project.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return await GetProjectById(projectId);
        }

        public async Task<ProjectDto> ReopenProject(int userId, int projectId)
        {
            var project = await VerifyProjectOwnership(userId, projectId);
            if (project.Status != "closed")
                throw new InvalidOperationException("Only closed projects can be reopened");

            project.Status = "open";
            project.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return await GetProjectById(projectId);
        }

        public async Task<ProjectDto> MarkAsInProgress(int userId, int projectId)
        {
            var project = await VerifyProjectOwnership(userId, projectId);
            project.Status = "in_progress";
            project.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return await GetProjectById(projectId);
        }

        public async Task<ProjectDto> MarkAsCompleted(int userId, int projectId)
        {
            var project = await VerifyProjectOwnership(userId, projectId);
            project.Status = "completed";
            project.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return await GetProjectById(projectId);
        }

        public async Task<ProjectStatsDto> GetProjectStats(int userId, int projectId)
        {
            await VerifyProjectOwnership(userId, projectId);
            return await GetStatsForProject(projectId);
        }

        #endregion

        #region POSITION MANAGEMENT

        public async Task<PositionDto> AddPosition(int userId, int projectId, CreatePositionDto request)
        {
            var project = await VerifyProjectOwnership(userId, projectId);

            var position = new ProjectPosition
            {
                ProjectId = projectId,
                Role = request.Role,
                Quantity = request.Quantity,
                Requirements = request.Requirements,
                Status = "open",
                CreatedAt = DateTime.Now
            };

            _context.ProjectPositions.Add(position);
            await _context.SaveChangesAsync();

            if (request.RequiredSkillIds != null && request.RequiredSkillIds.Any())
            {
                foreach (var skillId in request.RequiredSkillIds)
                {
                    _context.ProjectPositionSkills.Add(new ProjectPositionSkill
                    {
                        PositionId = position.Id,
                        SkillId = skillId,
                        IsRequired = true
                    });
                }
            }

            if (request.PreferredSkillIds != null && request.PreferredSkillIds.Any())
            {
                foreach (var skillId in request.PreferredSkillIds)
                {
                    _context.ProjectPositionSkills.Add(new ProjectPositionSkill
                    {
                        PositionId = position.Id,
                        SkillId = skillId,
                        IsRequired = false
                    });
                }
            }

            await _context.SaveChangesAsync();
            return await GetPositionDtoById(position.Id);
        }

        public async Task<List<PositionDto>> GetProjectPositions(int projectId)
        {
            var positions = await _context.ProjectPositions
                .AsNoTracking()
                .Where(p => p.ProjectId == projectId)
                .Include(p => p.ProjectPositionSkills)
                    .ThenInclude(s => s.Skill)
                .ToListAsync();

            return positions.Select(pos => new PositionDto
            {
                Id = pos.Id,
                ProjectId = pos.ProjectId,
                Role = pos.Role,
                Quantity = pos.Quantity ?? 1,
                Requirements = pos.Requirements,
                Status = pos.Status ?? "open",
                CreatedAt = pos.CreatedAt ?? DateTime.Now,
                Skills = pos.ProjectPositionSkills.Select(s => new PositionSkillDto
                {
                    SkillId = s.SkillId,
                    SkillName = s.Skill.Name,
                    IsRequired = s.IsRequired ?? true
                }).ToList(),
                TotalApplications = _context.ProjectApplications.Count(a => a.PositionId == pos.Id),
                CurrentMembers = _context.ProjectMembers.Count(m => m.PositionId == pos.Id && m.Status == "active")
            }).ToList();
        }

        public async Task<PositionDto> UpdatePosition(int userId, int positionId, UpdatePositionRequest request)
        {
            var position = await _context.ProjectPositions
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == positionId);

            if (position == null)
                throw new KeyNotFoundException("Position not found");

            if (position.Project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission");

            if (request.Role != null) position.Role = request.Role;
            if (request.Quantity.HasValue) position.Quantity = request.Quantity;
            if (request.Requirements != null) position.Requirements = request.Requirements;

            await _context.SaveChangesAsync();
            return await GetPositionDtoById(positionId);
        }

        public async Task DeletePosition(int userId, int positionId)
        {
            var position = await _context.ProjectPositions
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == positionId);

            if (position == null)
                throw new KeyNotFoundException("Position not found");

            if (position.Project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission");

            var hasPendingApps = await _context.ProjectApplications
                .AnyAsync(a => a.PositionId == positionId && a.Status == "pending");

            if (hasPendingApps)
                throw new InvalidOperationException("Cannot delete position with pending applications");

            _context.ProjectPositions.Remove(position);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region POSITION SKILLS

        public async Task<List<PositionSkillDto>> GetPositionSkills(int positionId)
        {
            var skills = await _context.ProjectPositionSkills
                .AsNoTracking()
                .Where(s => s.PositionId == positionId)
                .Include(s => s.Skill)
                .ToListAsync();

            return skills.Select(s => new PositionSkillDto
            {
                SkillId = s.SkillId,
                SkillName = s.Skill.Name,
                IsRequired = s.IsRequired ?? true
            }).ToList();
        }

        public async Task AddPositionSkill(int userId, int positionId, AddPositionSkillRequest request)
        {
            var position = await _context.ProjectPositions
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == positionId);

            if (position == null)
                throw new KeyNotFoundException("Position not found");

            if (position.Project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission");

            var skillExists = await _context.Skills.AnyAsync(s => s.Id == request.SkillId);
            if (!skillExists)
                throw new KeyNotFoundException("Skill not found");

            var alreadyExists = await _context.ProjectPositionSkills
                .AnyAsync(s => s.PositionId == positionId && s.SkillId == request.SkillId);

            if (alreadyExists)
                throw new InvalidOperationException("Skill already added to this position");

            _context.ProjectPositionSkills.Add(new ProjectPositionSkill
            {
                PositionId = positionId,
                SkillId = request.SkillId,
                IsRequired = request.IsRequired
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeletePositionSkill(int userId, int positionId, int skillId)
        {
            var position = await _context.ProjectPositions
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == positionId);

            if (position == null)
                throw new KeyNotFoundException("Position not found");

            if (position.Project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission");

            var positionSkill = await _context.ProjectPositionSkills
                .FirstOrDefaultAsync(s => s.PositionId == positionId && s.SkillId == skillId);

            if (positionSkill == null)
                throw new KeyNotFoundException("Skill not found in this position");

            _context.ProjectPositionSkills.Remove(positionSkill);
            await _context.SaveChangesAsync();
        }

        #endregion
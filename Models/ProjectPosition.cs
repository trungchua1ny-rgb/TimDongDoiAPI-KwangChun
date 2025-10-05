using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class ProjectPosition
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string Role { get; set; } = null!;

    public int? Quantity { get; set; }

    public string? Requirements { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<ProjectApplication> ProjectApplications { get; set; } = new List<ProjectApplication>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<ProjectPositionSkill> ProjectPositionSkills { get; set; } = new List<ProjectPositionSkill>();
}

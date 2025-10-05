using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class Project
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Type { get; set; }

    public int? DurationMonths { get; set; }

    public string? LocationType { get; set; }

    public string? CompensationType { get; set; }

    public string? CompensationDetails { get; set; }

    public string? Status { get; set; }

    public int? Views { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProjectApplication> ProjectApplications { get; set; } = new List<ProjectApplication>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<ProjectPosition> ProjectPositions { get; set; } = new List<ProjectPosition>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual User User { get; set; } = null!;
}

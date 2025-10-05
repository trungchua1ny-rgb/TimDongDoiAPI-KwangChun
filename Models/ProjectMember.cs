using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class ProjectMember
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public int? PositionId { get; set; }

    public string? RoleType { get; set; }

    public string? Status { get; set; }

    public DateOnly? JoinedAt { get; set; }

    public DateOnly? LeftAt { get; set; }

    public virtual ProjectPosition? Position { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}

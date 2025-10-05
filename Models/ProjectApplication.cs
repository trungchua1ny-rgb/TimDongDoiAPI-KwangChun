using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class ProjectApplication
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int PositionId { get; set; }

    public int UserId { get; set; }

    public string? CoverLetter { get; set; }

    public string? PortfolioLink { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProjectPosition Position { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

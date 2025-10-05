using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class ProjectPositionSkill
{
    public int Id { get; set; }

    public int PositionId { get; set; }

    public int SkillId { get; set; }

    public bool? IsRequired { get; set; }

    public virtual ProjectPosition Position { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class UserSkill
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SkillId { get; set; }

    public string? Level { get; set; }

    public decimal? YearsExperience { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Skill Skill { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

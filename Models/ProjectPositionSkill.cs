using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimDongDoi.API.Models
{
    [Table("project_position_skills")]
    public class ProjectPositionSkill
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("position_id")]
        public int PositionId { get; set; }

        [Required]
        [Column("skill_id")]
        public int SkillId { get; set; }

        [Column("is_required")]
        public bool IsRequired { get; set; } = true;

        // Navigation properties
        [ForeignKey("PositionId")]
        public virtual ProjectPosition? Position { get; set; }

        [ForeignKey("SkillId")]
        public virtual Skill? Skill { get; set; }
    }
}
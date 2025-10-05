using System;

namespace TimDongDoi.API.DTOs.User
{
    public class UserExperienceDto
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Position { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SmartEventPlanningPlatform.Models
{
    public class ActivityHistory
    {
        [Key]
        public int ActivityId { get; set; }

        // Foreign Key to User table
        public int UserId { get; set; }
        public string ActivityType { get; set; }
        public int PointsEarned { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
    }
}

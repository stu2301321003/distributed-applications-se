using System.ComponentModel.DataAnnotations;
using VacationManager.Commons.Enums;
using VacationManager.Users.Entities;

namespace VacationManager.Leaves.Entities
{
    public class Leave
    {
        public int Id { get; set; }
        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new();
        public LeaveType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

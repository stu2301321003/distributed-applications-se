using System.ComponentModel.DataAnnotations;

namespace VacationManager.UI.Leaves.Models
{
    public class LeaveCreateModel
    {
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public LeaveType Type { get; set; }
        public string? Description { get; set; }
    }
}

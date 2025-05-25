using System.ComponentModel.DataAnnotations;
using VacationManager.Commons.Enums;

namespace VacationManager.Leaves.Models
{
    public class LeaveUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public LeaveType Type { get; set; }
    }
}

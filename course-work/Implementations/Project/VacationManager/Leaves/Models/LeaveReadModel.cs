using VacationManager.Commons.Enums;

namespace VacationManager.Leaves.Models
{
    public class LeaveReadModel
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int UserId { get; set; }
        public LeaveType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

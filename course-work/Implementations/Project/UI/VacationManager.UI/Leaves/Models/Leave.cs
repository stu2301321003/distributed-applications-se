﻿using System.ComponentModel.DataAnnotations;
using VacationManager.UI.Users.Models;

namespace VacationManager.UI.Leaves.Models
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
        public string? Description { get; set; }
        public bool? IsAccepted { get; set; }
        public string? ResponseMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

﻿namespace VacationManager.Teams.Models
{
    public class TeamReadModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public int? CompanyId { get; set; }
        public int EmployeesCount { get;set; }
        public DateTime CreatedAt { get; set; }
    }
}

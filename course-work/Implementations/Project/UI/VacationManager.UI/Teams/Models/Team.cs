using System.ComponentModel.DataAnnotations;
using VacationManager.UI.Users.Models;
using VacationManager.UI.Companies.Models;

namespace VacationManager.UI.Teams.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required, Length(2, 35)]
        public string Name { get; set; } = null!;

        public int? ManagerId { get; set; }

        public User? Manager { get; set; } = new();

        public int CompanyId { get; set; }

        public Company Company { get; set; } = new();


        public List<User> Employees { get; set; } = [];
        public DateTime CreatedAt { get; set; }

    }
}

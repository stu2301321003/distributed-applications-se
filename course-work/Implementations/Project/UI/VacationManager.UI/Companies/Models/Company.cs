using System.ComponentModel.DataAnnotations;
using VacationManager.UI.Teams.Models;
using VacationManager.UI.Users.Models;

namespace VacationManager.UI.Companies.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required, Length(2, 35)]
        public string Name { get; set; } = null!;

        public List<Team> Teams { get; set; } = [];

        public int CeoId { get; set; }
        public User Ceo { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}

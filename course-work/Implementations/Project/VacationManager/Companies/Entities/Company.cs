using System.ComponentModel.DataAnnotations;
using VacationManager.Teams.Entities;
using VacationManager.Users.Entities;

namespace VacationManager.Companies.Entities
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

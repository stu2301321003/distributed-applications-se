using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VacationManager.UI.Users.Models;

namespace VacationManager.UI.Teams.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required, Length(2, 35)]
        public string Name { get; set; } = null!;

        public int ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public User Manager { get; set; } = new();

        public List<User> Users { get; set; } = [];
        public DateTime CreatedAt { get; set; }

    }
}

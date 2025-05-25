using System.ComponentModel.DataAnnotations;
using VacationManager.Auth.Models;
using VacationManager.Teams.Entities;

namespace VacationManager.Users.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, Length(2, 25)]
        public string Name { get; set; } = null!;

        [Required, Length(2, 35)]
        public string LastName { get; set; } = null!;

        [EmailAddress, Length(5, 50)]
        public string Email { get; set; } = null!;

        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Password { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Salt { get; set; } = null!;

        public Roles Role { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

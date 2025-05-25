using System.ComponentModel.DataAnnotations;
using VacationManager.Auth.Models;

namespace VacationManager.Users.Models
{
    public class VerifyUserModel
    {
        [EmailAddress, Length(5, 50)]
        public string Email { get; set; } = null!;

        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        public Roles Role { get; set; }
        public int? TeamId { get; set; }
    }
}

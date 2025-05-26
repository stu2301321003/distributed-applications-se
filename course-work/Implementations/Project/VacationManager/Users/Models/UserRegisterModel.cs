using System.ComponentModel.DataAnnotations;

namespace VacationManager.Users.Models
{
    public class UserRegisterModel()
    {
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
    }
}

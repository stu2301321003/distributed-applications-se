using System.ComponentModel.DataAnnotations;

namespace VacationManager.Users.Models
{
    public class UpdateUser
    {
        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Password { get; set; } = null!;
    }

}

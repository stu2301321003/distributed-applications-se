using System.ComponentModel.DataAnnotations;

namespace VacationManager.UI.Api.Models
{
    public class UserLoginModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

}

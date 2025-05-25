using Microsoft.AspNetCore.Authentication;

namespace VacationManager.Auth.Models
{
    public class AuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "MyAuthenticationScheme"; //todo get from appsettings
        public string TokenHeaderName { get; set; } = "MyToken";//todo get from appsettings
    }

}
